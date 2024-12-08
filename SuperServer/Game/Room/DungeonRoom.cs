using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using SuperServer.Data;
using SuperServer.Game.Map;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Room
{
    public class DungeonRoom : GameRoom
    {
        Dictionary<int, Hero> _heroes = new Dictionary<int, Hero>();
        Dictionary<int, Monster> _monsters = new Dictionary<int, Monster>();
        Dictionary<int, DropItem> _dropItems = new Dictionary<int, DropItem>();

        public DungeonRoom(int roomId) : base(roomId)
        {

        }

    }
}
