using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using ServerCore;
using SuperServer.Commander;
using SuperServer.DB;
using SuperServer.Game;
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
        public void HandleReqHeroList(ReqHeroListToS packet)
        {
            //임시 나중에 인증서버
            AccountId = packet.AccountId;
            ResHeroListToC resHeroListPacket = new ResHeroListToC();
            //첫 접속이라면
            if (LobbyHeroes.Count == 0)
            {
                List<Hero> heros = DBCommander.Instance.LoadHero(packet.AccountId);
                foreach (Hero hero in heros)
                {
                    SetLobbyHero(hero);
                }
            }
            //이미 접속 상태라 캐싱이 되어 있다면
            foreach (LobbyHero hero in LobbyHeroes)
                resHeroListPacket.Lobbyheros.Add(hero.LobbyHeroInfo);

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
            
            Hero hero = DBCommander.Instance.CreateHero(AccountId, packet);
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
            ResDeleteHeroToC resDeleteHeroPacket = new ResDeleteHeroToC();
            int heroIdx = packet.HeroIdx;

            if (heroIdx < 0 || heroIdx >= LobbyHeroes.Count)
                return;

            LobbyHero heroInfo = LobbyHeroes[heroIdx];

            if (heroInfo == null)
                return;

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

        private void SetLobbyHero(Hero hero)
        {
            LobbyHero heroInfo = new LobbyHero();
            heroInfo.SetInfo(hero);
            LobbyHeroes.Add(heroInfo);
        }
    }
}
