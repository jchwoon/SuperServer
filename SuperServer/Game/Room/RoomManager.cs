using SuperServer.Data;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Room
{
    public class RoomManager : Singleton<RoomManager>
    {
        Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
        static object _lock = new object();
        
        public int MaxRoomCount
        {
            get
            {
                return _rooms.Count;
            }
        }

        public void PreLoadRoom()
        {
            foreach (int roomId in DataManager.RoomDict.Keys)
            {
                GameRoom room = CreateRoom(roomId);
                room.Init();
            }
        }

        public GameRoom CreateRoom(int roomId)
        {
            GameRoom room;
            lock (_lock)
            {
                if (_rooms.TryGetValue(roomId, out room) == true)
                    return room;

                room = new GameRoom(roomId);
                _rooms.Add(roomId, room);
            }

            return room;
        }

        public GameRoom GetRoom(int roomId)
        {
            GameRoom room;
            lock (_lock)
            {
                if (_rooms.TryGetValue(roomId, out room) == true)
                    return room;
            }

            return null;
        }
    }
}
