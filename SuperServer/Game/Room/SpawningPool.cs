using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Utils;
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
        SpawningPoolData _spawningPoolData;
        Random _random = new Random();
        public void Init(GameRoom room)
        {
            //어디 위치에, 최대 몇마리를, 몬스터의 종류는
            _room = room;

            if (DataManager.SpawningPoolDict.TryGetValue(_room.RoomId, out _spawningPoolData) == false)
                return;

            foreach(PoolData poolData in _spawningPoolData.PoolDatas)
            {
                for (int i = 0; i < poolData.MaxEntityCount; i++)
                {
                    Monster monster = ObjectManager.Instance.Spawn<Monster>();
                    SetRandomPosInPool(monster, poolData);
                    monster.Init(poolData.MonsterId, poolData);
                    GameCommander.Instance.Push(() =>
                    {
                        _room.EnterRoom<Monster>(monster);
                    });
                }


            }
        }

        public void ReSpawn(Monster monster, PoolData poolData)
        {
            SetRandomPosInPool(monster, poolData);
            GameCommander.Instance.Push(() =>
            {
                _room.EnterRoom<Monster>(monster);
            });
        }

        private void SetRandomPosInPool(Creature creature, PoolData spawnData)
        {
            if (spawnData == null)
                return;

            float randX = (_random.NextSingle() * 2) - 1;
            float randZ = (_random.NextSingle() * 2) - 1;

            Vector3 center = new Vector3(spawnData.PosX, spawnData.PosY, spawnData.PosZ);
            Vector3 randDir = new Vector3(center.X + randX, center.Y, center.Z + randZ);

            Vector3 dir = (randDir - center).Normalize();
            float randDist = _random.NextSingle() * spawnData.SpawnRange;
            Vector3 randomPos = center + (dir * randDist);

            creature.PosInfo.PosX = randomPos.X;
            creature.PosInfo.PosY = randomPos.Y;
            creature.PosInfo.PosZ = randomPos.Z;
        }
    }
}
