using Google.Protobuf.Struct;
using SuperServer.Data;
using SuperServer.Game.Stat;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Object
{
    public class Creature : BaseObject
    {
        public CreatureInfo CreatureInfo { get; private set; }
        public StatComponent StatComponent { get; private set; }
        public PoolData PoolData { get; protected set; }
        public Creature()
        {
            StatComponent = new StatComponent();
            CreatureInfo = new CreatureInfo();
            CreatureInfo.ObjectInfo = ObjectInfo;
            CreatureInfo.StatInfo = StatComponent.StatInfo;
        }
    }
}
