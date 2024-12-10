using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Party
{
    public class Party
    {
        List<Hero> members = new List<Hero>();
        public long PartyId { get; private set; }
        public Hero Owner { get; private set; } 

        public Party(long partyId, Hero owner)
        {
            PartyId = partyId;
            Owner = owner;
        }
    }
}
