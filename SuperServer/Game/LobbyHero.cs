using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using SuperServer.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game
{
    public class LobbyHero
    {
        public int HeroId { get; private set; }
        public string Nickname { get; private set; }
        public EHeroClassType ClassType { get; private set; }
        public int Level { get; private set; }
        public LobbyHeroInfo LobbyHeroInfo { get; private set; }

        public void SetInfo(Hero hero)
        {
            HeroId = hero.HeroId;
            Nickname = hero.HeroName;
            ClassType = hero.Class;
            Level = hero.Level;
            LobbyHeroInfo = new LobbyHeroInfo()
            {
                ClassType = hero.Class,
                Level = hero.Level,
                Nickname = hero.HeroName,
            };
        }
    }
}
