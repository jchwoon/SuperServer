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
        public int Exp;
        public int MaxHp;
        public int MaxMp;
        public float MoveSpeed;
        public int AtkDamage;
        public int Defence;
        public float BaseAtkSpeed;
        public int AddAtkSpeedMultiplier;
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
        public float StartPosX;
        public float StartPosY;
        public float StartPosZ;
        public List<int> Npcs;
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
        public List<int> SkillIds;
        public float DetectionRange;
        public EMonsterAggroType AggroType;
        public EMonsterGrade MonsterGrade;
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
        public float RespawnTime;
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
        public int TemplateId;
        public int NextComboSkillTemplateId;
        public ESkillType SkillType;
        public ESkillProjectileType SkillProjectileType;
        public ESkillSlotType SkillSlotType;
        public ESkillAreaType SkillAreaType;
        public ESkillUsageTargetType SkillUsageTargetType;
        public float SectorAngle;
        public bool CanCancel;
        public bool IsComboSkill;
        public bool IsMoveSkill;
        public string SkillName;
        public string AnimName;
        public float SkillRange;
        public int MaxTargetInRange;
        public int CostMp;
        public float CoolTime;
        public float Dist;
        public float AnimTime;
        public int EffectId;
        public float EffectDelayRatio;
        public float ComboTime;
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
                dict.Add(skill.TemplateId, skill);
            }

            return dict;
        }
    }

    public struct AddStatInfo
    {
        public EStatType StatType;
        public float Value;
        public bool Multiplier;
    }

    public class EffectData
    {
        public int EffectId;
        public float DamageRatio;
        public List<AddStatInfo> AddStatValues;
        public EEffectType EffectType;
        public EEffectDurationType EffectDurationType;
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

    public class RewardData
    {
        public int RewardId;
        public int ItemId;
        public int Count;
    }

    [Serializable]
    public class RewardDataLoader : ILoader<int, RewardData>
    {
        public List<RewardData> rewards = new List<RewardData>();
        public Dictionary<int, RewardData> MakeDict()
        {
            Dictionary<int, RewardData> dict = new Dictionary<int, RewardData>();

            foreach(RewardData reward in rewards)
            {
                dict.Add(reward.RewardId, reward);
            }

            return dict;
        }
    }

    public struct RewardInfo
    {
        public int RewardId;
        public float Probability;
    }
    public class RewardTableData
    {
        public int RewardTableId;
        public int MonsterId;
        public int RewardGold;
        public int RewardExp;
        public List<RewardInfo> RewardInfos;
    }

    [Serializable]
    public class RewardTableDataLoadaer : ILoader<int, RewardTableData>
    {
        public List<RewardTableData> rewardTables = new List<RewardTableData>();
        public Dictionary<int, RewardTableData> MakeDict()
        {
            Dictionary<int, RewardTableData> dict = new Dictionary<int, RewardTableData>();

            foreach (RewardTableData rewardTableData in rewardTables)
            {
                dict.Add(rewardTableData.RewardTableId, rewardTableData);
            }

            return dict;
        }
    }

    public class ItemData : BaseData
    {
        public int ItemId;
        public int EffectId;
        public string DescId;
        public bool Stackable;
        public int MaxStack;
        public string Name;
        public string ImageName;
        public EItemType ItemType;
    }
    
    public class ConsumableData : ItemData
    {
        public float CoolTime;
        public EConsumableType ConsumableType;
    }

    [Serializable]
    public class ConsumableDataLoader : ILoader<int, ConsumableData>
    {
        public List<ConsumableData> items = new List<ConsumableData>();

        public Dictionary<int, ConsumableData> MakeDict()
        {
            Dictionary<int, ConsumableData> dict = new Dictionary<int, ConsumableData>();

            foreach (ConsumableData consumableData in items)
            {
                dict.Add(consumableData.ItemId, consumableData);
            }

            return dict;
        }
    }

    public class EquipmentData : ItemData
    {
        public EHeroClassType ClassType;
        public EEquipItemType EquipItemType;
        public int RequiredLevel;
    }

    [Serializable]
    public class EquipmentDataLoader : ILoader<int, EquipmentData>
    {
        public List<EquipmentData> items = new List<EquipmentData>();

        public Dictionary<int, EquipmentData> MakeDict()
        {
            Dictionary<int, EquipmentData> dict = new Dictionary<int, EquipmentData>();

            foreach (EquipmentData equipmentData in items)
            {
                dict.Add(equipmentData.ItemId, equipmentData);
            }

            return dict;
        }
    }
    public class EtcData : ItemData
    {
    }

    [Serializable]
    public class EtcDataLoader : ILoader<int, EtcData>
    {
        public List<EtcData> items = new List<EtcData>();

        public Dictionary<int, EtcData> MakeDict()
        {
            Dictionary<int, EtcData> dict = new Dictionary<int, EtcData>();

            foreach (EtcData etcData in items)
            {
                dict.Add(etcData.ItemId, etcData);
            }

            return dict;
        }
    }
    public class NPCData
    {
        public int npcId;
        public int roomId;
        public float xPos;
        public float yPos;
        public float zPos;
    }

    [Serializable]
    public class NPCDataLoader : ILoader<int, NPCData>
    {
        public List<NPCData> npcs = new List<NPCData>();
        public Dictionary<int, NPCData> MakeDict()
        {
            Dictionary<int, NPCData> dict = new Dictionary<int, NPCData>();

            foreach (NPCData npcDatas in npcs)
            {
                dict.Add(npcDatas.npcId, npcDatas);
            }

            return dict;
        }
    }
}
