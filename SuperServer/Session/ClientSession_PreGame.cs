using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using ServerCore;
using SuperServer.Commander;
using SuperServer.DB;
using SuperServer.Game.Object;
using SuperServer.Game.Room;
using SuperServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Session
{
    public partial class ClientSession : PacketSession
    {
        static readonly ushort MAX_CREATE_HERO_NUM = 5;
        List<LobbyHero> LobbyHeroes { get; set; } = new(MAX_CREATE_HERO_NUM);
        public Hero PlayingHero { get; set; } = new Hero();
        public void HandleReqHeroList(ReqHeroListToS packet)
        {
            //임시 나중에 인증서버
            AccountId = packet.AccountId;
            ResHeroListToC resHeroListPacket = new ResHeroListToC();
            //첫 접속이라면
            if (LobbyHeroes.Count == 0)
            {
                List<DBHero> heroes = DBCommander.Instance.LoadHero(packet.AccountId);
                foreach (DBHero hero in heroes)
                {
                    SetLobbyHero(hero);
                }
            }
            //이미 접속 상태라 캐싱이 되어 있다면
            foreach (LobbyHero hero in LobbyHeroes)
                resHeroListPacket.Lobbyheros.Add(hero.LobbyHeroInfo);

            Console.WriteLine("SendLobbyInfo");
            Send(resHeroListPacket);
        }

        public void HandleReqCreateHero(ReqCreateHeroToS packet)
        {
            ResCreateHeroToC resCreateHeroPacket = new ResCreateHeroToC();
            if (packet.Nickname.Length < 2 || packet.Nickname.Length > 8)
            {
                resCreateHeroPacket.Result = Google.Protobuf.Enum.ECreateHeroResult.FailMinmax;
                Send(resCreateHeroPacket);
                return;
            }
            
            DBHero hero = DBCommander.Instance.CreateHero(AccountId, packet);
            if (hero == null)
            {
                resCreateHeroPacket.Result = Google.Protobuf.Enum.ECreateHeroResult.FailOverlap;
                Send(resCreateHeroPacket);
                return;
            }

            SetLobbyHero(hero);
            resCreateHeroPacket.Result = Google.Protobuf.Enum.ECreateHeroResult.Success;
            Send(resCreateHeroPacket);
        }

        public void HandleReqDeleteHero(ReqDeleteHeroToS packet)
        {
            int heroIdx = packet.HeroIdx;

            if (heroIdx < 0 || heroIdx >= LobbyHeroes.Count)
                return;

            LobbyHero heroInfo = LobbyHeroes[heroIdx];

            if (heroInfo == null)
                return;

            ResDeleteHeroToC resDeleteHeroPacket = new ResDeleteHeroToC();

            if (DBCommander.Instance.DeleteHero(heroInfo.HeroId) == false)
            {
                resDeleteHeroPacket.IsSuccess = false;
                Send(resDeleteHeroPacket);
                return;
            }
            LobbyHeroes.Remove(LobbyHeroes[heroIdx]);
            resDeleteHeroPacket.IsSuccess = true;
            Send(resDeleteHeroPacket);
        }

        public void HandleReqEnterRoom(ReqEnterRoomToS packet)
        {
            int heroIdx = packet.HeroIdx;

            if (heroIdx < 0 || heroIdx >= LobbyHeroes.Count)
                return;

            LobbyHero lobbyHero = LobbyHeroes[heroIdx];
            int heroId = lobbyHero.HeroId;

            DBHero dbHero = DBCommander.Instance.GetHero(heroId);

            if (dbHero == null)
                return;

            SetPlayingHero(dbHero, lobbyHero);

            if (PlayingHero == null)
                return;
            
            GameRoom room = RoomManager.Instance.GetRoom(dbHero.RoomId);
            if (room == null)
            {
                room = RoomManager.Instance.GetRoom(1);
            }

            GameCommander.Instance.Push(room.EnterRoom<Hero>, PlayingHero);
        }

        private void SetLobbyHero(DBHero hero)
        {
            LobbyHero heroInfo = new LobbyHero();
            heroInfo.SetInfo(hero);
            LobbyHeroes.Add(heroInfo);
        }

        private Hero SetPlayingHero(DBHero dbHero, LobbyHero lobbyHero)
        {
            Hero hero = ObjectManager.Instance.Spawn<Hero>();
            hero.Init(dbHero, lobbyHero, this);
            PlayingHero = hero;
            return hero;
        }
    }
}
