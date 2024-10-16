using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using SuperServer.Commander;
using SuperServer.Game.Room;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Object
{
    public class BaseObject
    {
        private MoveToC _movePacket = new MoveToC() { PosInfo = new PosInfo() };
        private GetHitToC _getHitPacket = new GetHitToC();
        protected int _objectId;
        protected GameRoom _gameRoom;
        protected EObjectType _objectType;
        public ObjectInfo ObjectInfo { get; set; } = new ObjectInfo();
        public PosInfo PosInfo { get; set; } = new PosInfo();
        public Vector3 Position { get { return new Vector3(PosInfo.PosX, PosInfo.PosY, PosInfo.PosZ); } }
        public GameRoom Room
        {
            get { return _gameRoom; }
            set
            {
                _gameRoom = value;
                if (value != null)
                    ObjectInfo.RoomId = value.RoomId;
            }
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

        public void BroadcastMove(Vector3? destPos, EMoveType moveType = EMoveType.None)
        {
            if (destPos.HasValue)
            {
                _movePacket.PosInfo.PosX = destPos.Value.X;
                _movePacket.PosInfo.PosY = destPos.Value.Y;
                _movePacket.PosInfo.PosZ = destPos.Value.Z;
            }
            else
                _movePacket.PosInfo = PosInfo;
            _movePacket.ObjectId = ObjectId;
            _movePacket.MoveType = moveType;

            GameCommander.Instance.Push(() =>
            {
                Room?.Broadcast(_movePacket, Position);
            });
        }

        public void BroadcastGetHit(int objectId)
        {
            _getHitPacket.ObjectId = objectId;
            GameCommander.Instance.Push(() =>
            {
                Room?.Broadcast(_getHitPacket, Position);
            });
        }
    }
}
