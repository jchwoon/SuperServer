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

    public void Init(DBHero hero, LobbyHero lobbyHero, ClientSession session)
    {
        Session = session;
        DbHeroId = hero.DBHeroId;
        InterestRegion = new InterestRegion(this);
        Inventory = new InventoryComponent(this);
        GrowthComponent = new HeroGrowthComponent(this);

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
            HeroInfo = HeroInfo,
        };
        InitStat(hero);
        InitPosInfo(hero);
        InitSkill();
    }

    private void InitStat(DBHero dbHero)
    {
        StatComponent.SetHeroStat(dbHero.Level);
        if (dbHero.HeroStat.HP >= 0)
            StatComponent.StatInfo.Hp = dbHero.HeroStat.HP;
        if (dbHero.HeroStat.MP >= 0)
            StatComponent.StatInfo.Mp = dbHero.HeroStat.MP;
    }
    private void InitPosInfo(DBHero dbHero)
    {
        PosInfo.PosX = dbHero.PosX;
        PosInfo.PosY = dbHero.PosY;
        PosInfo.PosZ = dbHero.PosZ;
        PosInfo.RotY = dbHero.RotY;
    }

    private void InitSkill()
    {
        SkillComponent.RegisterSkill(HeroData.SkillIds);
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
        MyHeroInfo.Gold += tableData.RewardGold;

        RewardToC rewardPacket = new RewardToC();
        rewardPacket.Exp = tableData.RewardExp;
        rewardPacket.Gold = tableData.RewardGold;

        Session?.Send(rewardPacket);
    }

    public void SendDropItem(List<int> itemIds)
    {

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