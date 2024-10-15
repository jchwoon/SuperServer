using Google.Protobuf.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Data
{
    //Hero가 레벨당 능력치가 어떻게 되는지에 대한 데이터 HeroData아님!!!
    public class HeroStatData
    {
        public int Level;
        public float Exp;
        public int MaxHp;
        public int MaxMp;
        public float MoveSpeed;
        public int AtkDamage;
        public int Defence;
        public float AtkSpeed;
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
    public class BaseData
    {
        public string PrefabName;
    }

    public class MonsterData : BaseData
    {
        public int MonsterId;
        public int RoomId;
        public string Name;
        public int Level;
        public int Exp;
        public int Gold;
        public int MaxHp;
        public int MaxMp;
        public float MoveSpeed;
        public float ChaseSpeed;
        public int AtkDamage;
        public int Defence;
        public float AtkSpeed;
        public float Sight;
        public float AtkRange;
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

    public class PoolData
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

    public class SpawningPoolData
    {
        public int RoomId;
        public List<PoolData> PoolDatas;
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

    public class HeroData : BaseData
    {
        public EHeroClassType HeroClassId;
        public float ComboExitTime;
        public List<int> SkillIds;
    }
    [Serializable]
    public class HeroDataLoader : ILoader<EHeroClassType, HeroData>
    {
        public List<HeroData> heroes = new List<HeroData>();
        public Dictionary<EHeroClassType, HeroData> MakeDict()
        {
            Dictionary<EHeroClassType, HeroData> dict = new Dictionary<EHeroClassType, HeroData>();
            foreach (HeroData hero in heroes)
            {
                dict.Add(hero.HeroClassId, hero);
            }

            return dict;
        }
    }

    public class SkillData
    {
        public int SkillId;
        public ESkillType SkillType;
        public string SkillName;
        public string AnimName;
        public int SkillRange;
        public int CostMp;
        public float CoolTime;
        public float AnimTime;
        public string AnimParamName;
        public int EffectId;
    }

    [Serializable]
    public class SkillDataLoader : ILoader<int, SkillData>
    {
        public List<SkillData> skills = new List<SkillData>();

        public Dictionary<int, SkillData> MakeDict()
        {
            Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();

            foreach(SkillData skill in skills)
            {
                dict.Add(skill.SkillId, skill);
            }

            return dict;
        }
    }

    public class EffectData
    {
        public int EffectId;
        public float DamageRatio;
    }

    [Serializable]
    public class EffectDataLoader : ILoader<int, EffectData>
    {
        public List<EffectData> effects = new List<EffectData>();
        public Dictionary<int, EffectData> MakeDict()
        {
            Dictionary<int, EffectData> dict = new Dictionary<int, EffectData>();

            foreach(EffectData effect in effects)
            {
                dict.Add(effect.EffectId, effect);
            }

            return dict;
        }
    }
}
