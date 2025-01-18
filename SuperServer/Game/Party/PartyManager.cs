using Google.Protobuf.Protocol;
using SuperServer.Data;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Enum;

namespace SuperServer.Game.Party
{
    public class PartyManager : Singleton<PartyManager>
    {
        public static long _partyId = 1;
        Dictionary<int, List<Party>> _parties = new Dictionary<int, List<Party>>(); 

        //파티장에게 참가 요청 보내기

        public void Init()
        {
            List<RoomData> datas = DataManager.RoomDict.Values.Where(r => r.RoomType == ERoomType.Dungeon).ToList();
            foreach (RoomData room in datas)
            {
                _parties.Add(room.RoomId, new List<Party>());
            }
        }
        public void RequestJoinParty(Hero joiner, long partyId, int roomId)
        {
            Party party = FindPartyById(roomId, partyId);
            if (party == null)
                return;

            ReqPartyJoinApprovalToC joinApprovalPacket = new ReqPartyJoinApprovalToC();
            joinApprovalPacket.JoinerId = joiner.ObjectId;
            party.Leader.Session?.Send(joinApprovalPacket);
        }

        public void CreateParty(Hero leader, int roomId)
        {
            if (leader == null)
                return;

            if (_parties.ContainsKey(roomId) == false)
                return;

            long partyId = GeneratePartyId();
            Party party = new Party(partyId, roomId, leader);

            _parties[roomId].Add(party);
        }

        public static long GeneratePartyId()
        {
            return Interlocked.Increment(ref _partyId);
        }

        public List<Party> GetAllPartiesByRoomId(int roomId)
        {
            if (_parties.TryGetValue(roomId, out List<Party> parties) == false)
                return null;

            return parties;
        }

        private Party FindPartyById(int roomId, long partyId)
        {
            if (_parties.ContainsKey(roomId) == false)
                return null;

            return _parties[roomId].Where(p => p.PartyId == partyId).FirstOrDefault();
        }
    }
}
