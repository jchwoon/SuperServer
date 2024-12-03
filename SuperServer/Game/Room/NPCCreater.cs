using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Room
{
    public class NPCCreater
    {
        public void Init(GameRoom room)
        {
            RoomData roomData;
            if (DataManager.RoomDict.TryGetValue(room.RoomId, out roomData))
            {
                foreach(int npcId in roomData.Npcs)
                {
                    NPC npc = ObjectManager.Instance.Spawn<NPC>();

                    if(DataManager.NpcDict.TryGetValue(npcId, out NPCData npcData))
                    {                        
                        npc.Init(npcData);
                        GameCommander.Instance.Push(() =>
                        {
                            room.EnterRoom<NPC>(npc);
                        });
                    }
                }
            }
        }
    }
}
