using Google.Protobuf.Struct;
using SuperServer.Game.Stat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Object
{
    public class Creature : BaseObject
    {
        public CreatureInfo CreatureInfo { get; private set; } = new CreatureInfo();
        public StatComponent StatComponent { get; private set; }
        public Creature()
        {
            StatComponent = new StatComponent();
            CreatureInfo.ObjectInfo = ObjectInfo;
            CreatureInfo.StatInfo = StatComponent.StatInfo;
        }
    }
}
