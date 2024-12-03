using SuperServer.Data;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NPC : Creature
{
    public NPCData NPCData { get; private set; }
    public void Init(NPCData npcData)
    {
        NPCData = npcData;
        ObjectInfo.TemplateId = NPCData.npcId;
        SetPos();
    }

    public void SetPos()
    {
        PosInfo.PosX = NPCData.xPos;
        PosInfo.PosY = NPCData.yPos;
        PosInfo.PosZ = NPCData.zPos;
    }

}
