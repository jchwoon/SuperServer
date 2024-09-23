using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
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
        protected int _objectId;
        protected GameRoom _gameRoom;
        protected EObjectType _objectType;
        public ObjectInfo ObjectInfo { get; set; } = new ObjectInfo();
        public PosInfo PosInfo { get; set; } = new PosInfo();
        public GameRoom Room
        {
            get { return _gameRoom; }
            set { _gameRoom = value; ObjectInfo.RoomId = value.RoomId; }
        }
        public int ObjectId
        {
            get { return _objectId; }
            set { _objectId = value; ObjectInfo.ObjectId = value; }
        }
        public EObjectType ObjectType
        {
            get { return _objectType; }
            set { _objectType = value; ObjectInfo.ObjectType = value; }
        }

        public BaseObject()
        {
            ObjectInfo.PosInfo = PosInfo;
        }
    }
}
