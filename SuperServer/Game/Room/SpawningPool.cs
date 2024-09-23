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
    public class SpawningPool
    {
        GameRoom _room;
        public void Init(GameRoom room)
        {
            //어디 위치에, 최대 몇마리를, 몬스터의 종류는
            _room = room;

            SpawningPoolData spawningPoolData;
            if (DataManager.SpawningPoolDict.TryGetValue(_room.RoomId, out spawningPoolData) == false)
                return;

            foreach(SpawnData spawnData in spawningPoolData.SpawnData)
            {
                for (int i = 0; i < spawnData.MaxEntityCount; i++)
                {
                    Monster monster = ObjectManager.Instance.Spawn<Monster>();
                    monster.Init(spawnData.MonsterId);
                    GameCommander.Instance.Push(() =>
                    {
                        _room.EnterRoom<Monster>(monster);
                    });
                }


            }
        }
    }
}
