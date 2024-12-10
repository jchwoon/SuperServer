using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Party
{
    public class PartyManager : Singleton<PartyManager>
    {
        public static long _partyId = 1;
        Dictionary<long, Party> _parties = new Dictionary<long, Party>();

        public void CreateParty(Hero owner)
        {
            if (owner == null)
                return;
            long partyId = GeneratePartyId();
            Party party = new Party(partyId, owner);

            _parties.Add(partyId, party);
        }

        public static long GeneratePartyId()
        {
            return Interlocked.Increment(ref _partyId);
        }
    }
}
