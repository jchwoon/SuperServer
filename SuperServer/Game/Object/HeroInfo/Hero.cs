using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.DB;
using SuperServer.Game.Room;
using SuperServer.Session;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperServer.Game.Stat;
using Google.Protobuf.Protocol;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SuperServer.Logic;
using SuperServer.Game.Object.HeroInfo;


public class Hero : Creature
{
    public int DbHeroId { get; private set; }
    public HeroInfo HeroInfo { get; private set; }
    public MyHeroInfo MyHeroInfo { get; private set; }
    public ClientSession Session { get; private set; }
    public InterestRegion InterestRegion { get; private set; }
    public HeroData HeroData { get; private set; }
    public InventoryComponent Inventory { get; private set; }
    public HeroGrowthComponent GrowthComponent { get; private set; }
    public CurrencyComponent CurrencyComponent { get; private set; }

    public void Init(DBHero hero, LobbyHero lobbyHero, ClientSession session)
    {
        Session = session;
        DbHeroId = hero.DBHeroId;
        InterestRegion = new InterestRegion(this);
        Inventory = new InventoryComponent(this);
        GrowthComponent = new HeroGrowthComponent(this);
        CurrencyComponent = new CurrencyComponent(this);

        if (DataManager.HeroDict.TryGetValue(lobbyHero.LobbyHeroInfo.ClassType, out HeroData heroData) == true)
            HeroData = heroData;
        HeroInfo = new HeroInfo()
        {
            LobbyHeroInfo = lobbyHero.LobbyHeroInfo,
            CreatureInfo = CreatureInfo,
        };
        MyHeroInfo = new MyHeroInfo()
        {
            Gold = hero.Gold,
            Exp = hero.Exp,
            ConsumeInvenSlotCount = hero.ConsumableSlotCount,
            EquipInvenSlotCount = hero.EquipmentSlotCount,
            EtcInvenSlotCount = hero.ETCSlotCount,
            HeroInfo = HeroInfo,
        };
        InitStat(hero);
        InitPosInfo(hero);
        InitSkill(hero);
        InitInventory(hero);
    }

    private void InitStat(DBHero dbHero)
    {
        StatComponent.InitHeroStat(dbHero.Level);
        if (dbHero.HeroStat.HP >= 0)
            StatComponent.AddedStatInfo.Hp = dbHero.HeroStat.HP;
        if (dbHero.HeroStat.MP >= 0)
            StatComponent.AddedStatInfo.Mp = dbHero.HeroStat.MP;
    }
    private void InitPosInfo(DBHero dbHero)
    {
        PosInfo.PosX = dbHero.PosX;
        PosInfo.PosY = dbHero.PosY;
        PosInfo.PosZ = dbHero.PosZ;
        PosInfo.RotY = dbHero.RotY;
    }

    private void InitSkill(DBHero dbHero)
    {
        Dictionary<int, int> skills = new Dictionary<int, int>();
        
        if (dbHero.Skills.Count == 0)
        {
            foreach (int id in HeroData.SkillIds)
            {
                if (DataManager.SkillDict.TryGetValue(id, out SkillData skillData) == true)
                    skills.Add(id, skillData.InitLevel);
            }
            SkillComponent.RegisterSkill(skills);
            DBCommander.Instance.Push(DBLogic.Instance.SaveSkillList, this);
        }
        else
        {
            foreach (KeyValuePair<int, int> s in dbHero.Skills)
            {
                skills.Add(s.Key, s.Value);
            }
            SkillComponent.RegisterSkill(skills);
        }

        SkillComponent.InitSkillPoint(dbHero.SkillPoint);
    }

    private void InitInventory(DBHero dbHero)
    {
        Inventory.Init(dbHero);
    }
    public override void OnDie(Creature killer)
    {
        if (Room == null)
            return;

        if (CurrentState == ECreatureState.Die)
            return;

        base.OnDie(killer);

        StatComponent.SetStat(EStatType.Mp, StatComponent.GetStat(EStatType.MaxMp));
        ReserveRespawn();
    }
    public void GiveExpAndGold(RewardTableData tableData)
    {
        GrowthComponent.AddExp(tableData.RewardExp);
        CurrencyComponent.AddGold(tableData.RewardGold);

        RewardToC rewardPacket = new RewardToC();
        rewardPacket.Exp = tableData.RewardExp;
        rewardPacket.Gold = tableData.RewardGold;

        Session?.Send(rewardPacket);
    }
    public void LevelUp(int prevLevel, int currentLevel)
    {
        HeroStatData prevStatData;
        if (DataManager.HeroStatDict.TryGetValue(prevLevel, out prevStatData) == false)
            return;

        HeroStatData currentStatData;
        if (DataManager.HeroStatDict.TryGetValue(currentLevel, out currentStatData) == false)
            return;

        int addMaxHp = currentStatData.MaxHp - prevStatData.MaxHp;
        int addMaxMp = currentStatData.MaxMp - prevStatData.MaxMp;
        int addAtkDamage = currentStatData.AtkDamage - prevStatData.AtkDamage;
        int addDefence = currentStatData.Defence - prevStatData.Defence;

        AddStat(EStatType.MaxHp, addMaxHp, sendPacket:false);
        AddStat(EStatType.MaxMp, addMaxMp, sendPacket: false);
        AddStat(EStatType.Atk, addAtkDamage, sendPacket: false);
        AddStat(EStatType.Defence, addDefence, sendPacket: false);
    }

    public override void ReSpawn()
    {
        base.ReSpawn();
        CurrentState = ECreatureState.Idle;
        BroadcastStat();
    }

    private void ReserveRespawn()
    {
        GameRoom room;
        if (Room == null)
            room = RoomManager.Instance.GetRoom(1);
        else
            room = Room;


        GameCommander.Instance.PushAfter((int)HeroData.RespawnTime * 1000, room.ReSpawn, this);
    }
}