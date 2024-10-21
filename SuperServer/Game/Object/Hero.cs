using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.DB;
using SuperServer.Game.Room;
using SuperServer.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Object
{
    public class Hero : Creature
    {
        public int DbHeroId { get; private set; }
        public HeroInfo HeroInfo { get; private set; }
        public MyHeroInfo MyHeroInfo { get; private set; }
        public ClientSession Session { get; private set; }
        public InterestRegion InterestRegion { get; private set; }
        public HeroData HeroData { get; private set; }

        public void Init(DBHero hero, LobbyHero lobbyHero, ClientSession session)
        {
            Session = session;
            DbHeroId = hero.DBHeroId;
            StatComponent.SetHeroStat(hero);
            InterestRegion = new InterestRegion(this);

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
            InitPosInfo(hero);
            InitSkill();
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
}
