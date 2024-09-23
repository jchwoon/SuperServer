using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Data
{
    public class BaseStatData
    {
        public int MaxHp;
        public int MaxMp;
        public float MoveSpeed;
        public int AtkDamage;
        public int Defence;
        public float AtkSpeed;
    }
    public class HeroStatData : BaseStatData
    {
        public int Level;
        public float Exp;
    }

    [Serializable]
    public class HeroStatDataLoader : ILoader<int, HeroStatData>
    {
        public List<HeroStatData> stats = new List<HeroStatData>();

        public Dictionary<int, HeroStatData> MakeDict()
        {
            Dictionary<int, HeroStatData> dict = new Dictionary<int, HeroStatData>();
            foreach (HeroStatData stat in stats)
            {
                dict.Add(stat.Level, stat);
            }


            return dict;
        }
    }

    public class RoomData
    {
        public int RoomId;
        public string Name;
    }

    [Serializable]
    public class RoomDataLoader : ILoader<int, RoomData>
    {
        public List<RoomData> rooms = new List<RoomData>();

        public Dictionary<int, RoomData> MakeDict()
        {
            Dictionary<int, RoomData> dict = new Dictionary<int, RoomData>();
            foreach (RoomData room in rooms)
            {
                dict.Add(room.RoomId, room);
            }


            return dict;
        }
    }

    public class MonsterData : BaseStatData
    {
        public int MonsterId;
        public int RoomId;
        public string Name;
        public string PrefabName;
        public int Level;
        public int Exp;
        public int Gold;
    }

    [Serializable]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters = new List<MonsterData>();

        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach (MonsterData monster in monsters)
            {
                dict.Add(monster.MonsterId, monster);
            }

            return dict;
        }
    }
    [Serializable]
    public class SpawnData
    {
        public int SpawnId;
        public int MaxEntityCount;
        public int MonsterId;
        public float RespawnTime;
        public float PosX;
        public float PosY;
        public float PosZ;
        public float SpawnRange;
    }
    [Serializable]
    public class SpawningPoolData
    {
        public int RoomId;
        public List<SpawnData> SpawnData;
    }

    [Serializable]
    public class SpawningPoolLoader : ILoader<int, SpawningPoolData>
    {
        public List<SpawningPoolData> spawningPools = new List<SpawningPoolData>();
        public Dictionary<int, SpawningPoolData> MakeDict()
        {
            Dictionary<int, SpawningPoolData> dict = new Dictionary<int, SpawningPoolData>();
            foreach(SpawningPoolData pool in spawningPools)
            {
                dict.Add(pool.RoomId, pool);
            }

            return dict;
        }
    }
}
