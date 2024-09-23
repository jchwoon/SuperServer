using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using SuperServer.Data;
using SuperServer.DB;
using SuperServer.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Object
{
    public class Monster : Creature
    {
        public int MonsterId { get; private set; }

        public void Init(int monsterId)
        {
            MonsterData monsterData;
            if (DataManager.MonsterDict.TryGetValue(monsterId, out monsterData) == false)
                return;

            MonsterId = monsterId;
            ObjectInfo.TemplateId = monsterId;
            StatComponent.InitSetStat(monsterData);
        }
        //PosInfo는 스폰위치가 결정될 때 또는 움직일 때(AI)
    }
}
