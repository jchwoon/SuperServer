using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using ServerCore;
using SuperServer.Commander;
using SuperServer.DB;
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
        List<LobbyHeroInfo> LobbyHeros { get; set; } = new(MAX_CREATE_HERO_NUM);
        public void HandleReqHeroList(ReqHeroListToS packet)
        {
            ResHeroListToC resHeroListPacket = new ResHeroListToC();
            if (LobbyHeros.Count == 0)
            {
                List<Hero> heros = DBCommander.Instance.LoadHeroDb(packet.AccountId);
                foreach (Hero hero in heros)
                {
                    LobbyHeroInfo heroInfo = new LobbyHeroInfo()
                    {
                        Nickname = hero.HeroName,
                        Gender = hero.Gender,
                        ClassType = hero.Class,
                        Level = hero.Level
                    };
                    resHeroListPacket.Lobbyheros.Add(heroInfo);
                    LobbyHeros.Add(heroInfo);
                }
            }

            foreach (LobbyHeroInfo hero in LobbyHeros)
                resHeroListPacket.Lobbyheros.Add(hero);

            Send(resHeroListPacket);
        }
    }
}
