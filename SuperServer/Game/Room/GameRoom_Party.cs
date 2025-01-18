using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using SuperServer.Game.Party;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Room
{
    public partial class GameRoom
    {
        public void HandleCreateParty(Hero hero, int roomId)
        {
            if (hero == null)
                return;

            PartyManager.Instance.CreateParty(hero, roomId);
        }

        public void HandleJoinParty(Hero joiner, long partyId, int roomId)
        {
            if (joiner == null)
                return;

            PartyManager.Instance.RequestJoinParty(joiner, partyId, roomId);
        }

        public void HandleReqAllPartyInfosByRoomId(Hero requester, int roomId)
        {
            if (requester == null)
                return;

            List<Party.Party> parties = PartyManager.Instance.GetAllPartiesByRoomId(roomId);
            if (parties == null)
                return;

            ResAllPartyInfoToC resAllPartyInfoPacket = new ResAllPartyInfoToC();
            for (int i = 0; i <  parties.Count; i++)
            {
                resAllPartyInfoPacket.PartyInfos.Add(new PartyInfo());
                for (int j = 0; j < parties[i].Members.Count; j++)
                {
                    Hero member = parties[i].Members[j];
                    MemberInfo info = new MemberInfo()
                    {
                        Class = member.ClassType,
                        Level = member.GrowthComponent.Level,
                        Name = member.Nickname
                    };
                    resAllPartyInfoPacket.PartyInfos[i].MemberInfos.Add(info);
                }
            }
            requester?.Session.Send(resAllPartyInfoPacket);
        }
    }
}
