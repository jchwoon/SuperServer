using Google.Protobuf.Struct;
using SuperServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Party
{
    public class Party
    {
        public PartyData PartyData { get; private set; }

        private List<HeroData> herosInParty = new List<HeroData>();

        public void Init(PartyData partyData)
        {
            PartyData = partyData;
            //PlayerJoinParty();

            if (partyData.isFull)
            {
                //파티 목록에 표시 안되도록.
            }
            

        }

        public void PlayerJoinParty(HeroData heroData)
        {
            string userName = heroData.PrefabName;
            herosInParty.Add(heroData);
        }

    }
}
