using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using SuperServer.Commander;
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
    public class Hero : BaseObject
    {
        public int HeroId { get; private set; }
        public HeroInfo HeroInfo { get; private set; }
        public MyHeroInfo MyHeroInfo { get; private set; }
        public GameRoom Room { get; private set; }
        public ClientSession Session { get; private set; }

        public void SetInfo(DBHero hero, LobbyHero lobbyHero, ClientSession session)
        {
            Session = session;
            HeroId = hero.DBHeroId;
            HeroInfo = new HeroInfo()
            {
                LobbyHeroInfo = lobbyHero.LobbyHeroInfo,
                StatInfo = SetStat(hero),
                ObjectInfo = SetObjectInfo(hero)

            };
            MyHeroInfo = new MyHeroInfo()
            {
                Exp = hero.HeroStat.Exp,
                HeroInfo = HeroInfo,
            };

            SetRoom(hero);
        }

        private StatInfo SetStat(DBHero dbHero)
        {
            StatInfo statInfo = new StatInfo()
            {
                AtkDamage = dbHero.HeroStat.AtkDamage,
                AtkSpeed = dbHero.HeroStat.AtkSpeed,
                Defence = dbHero.HeroStat.Defense,
                Hp = dbHero.HeroStat.HP,
                MaxHp = dbHero.HeroStat.MaxHp,
                Mp = dbHero.HeroStat.MP,
                MaxMp = dbHero.HeroStat.MaxMp,
                MoveSpeed = dbHero.HeroStat.MoveSpeed
            };

            return statInfo;
        }

        private ObjectInfo SetObjectInfo(DBHero dbHero)
        {
            ObjectInfo objectInfo = new ObjectInfo()
            {
                ObjectId = ObjectId,
                PosInfo = SetPosInfo(dbHero),
                RoomId = dbHero.RoomId
            };

            return objectInfo;
        }

        private PosInfo SetPosInfo(DBHero dbHero)
        {
            PosInfo posInfo = new PosInfo()
            {
                PosX = 0,
                PosY = 0,
                PosZ = 0,
                RotY = 0
            };
            return posInfo;
        }

        private void SetRoom(DBHero dbHero)
        {
            if (dbHero.RoomId <= 0 || dbHero.RoomId > RoomManager.Instance.MaxRoomCount)
                return;

            GameRoom room = RoomManager.Instance.GetRoom(dbHero.RoomId);
            Room = room;
            Room.RoomId = dbHero.RoomId;

            GameCommander.Instance.Push(() =>
            {
                room.EnterRoom<Hero>(this);
            });
        }
    }
}
