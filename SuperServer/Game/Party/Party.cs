using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Party
{
    public class Party
    {
        //리더까지 포함된
        public List<Hero> Members = new List<Hero>();
        public long PartyId { get; private set; }
        public Hero Leader { get; private set; } 
        public int RoomId{ get; private set; } 

        public Party(long partyId, int roomId,  Hero leader)
        {
            PartyId = partyId;
            Leader = leader;
            RoomId = roomId;
            JoinParty(leader);
        }

        public void JoinParty(Hero applier)
        {
            Members.Add(applier);
        }
    }
}
