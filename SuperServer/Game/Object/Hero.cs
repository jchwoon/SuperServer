using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
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
            HeroId = hero.HeroId;
            HeroInfo = new HeroInfo()
            {
                LobbyHeroInfo = lobbyHero.LobbyHeroInfo,
                StatInfo = SetStat(hero),
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

        private void SetRoom(DBHero dbHero)
        {
            if (dbHero.RoomId <= 0 || dbHero.RoomId > RoomManager.Instance.MaxRoomCount)
            {
                Room.RoomId = 1;
                return;
            }

            Room.RoomId = dbHero.RoomId;

            GameRoom room = RoomManager.Instance.GetRoom(Room.RoomId);
            room.EnterRoom<Hero>(this);
        }
    }
}
