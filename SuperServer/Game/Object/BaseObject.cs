using Google.Protobuf.Enum;
using SuperServer.Game.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Object
{
    public class BaseObject
    {
        public int ObjectId { get; set; }
        public EObjectType ObjectType { get; set; }
        public GameRoom Room { get; set; }
    }
}
