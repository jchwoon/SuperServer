using Google.Protobuf.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Data
{
    public interface ILoader<Kye, Value>
    {
        Dictionary<Kye, Value> MakeDict();
    }
    public class DataManager
    {
        public static Dictionary<int, HeroStatData> HeroStatDict { get; private set; } = new Dictionary<int, HeroStatData>();
        public static Dictionary<int, RoomData> RoomDict { get; private set; } = new Dictionary<int, RoomData>();
        public static Dictionary<int, MonsterData> MonsterDict { get; private set; } = new Dictionary<int, MonsterData>();
        public static Dictionary<int, SpawningPoolData> SpawningPoolDict { get; private set; } = new Dictionary<int, SpawningPoolData>();
        public static Dictionary<int, SkillData> HeroSkillDict { get; private set; } = new Dictionary<int, SkillData>();
        public static Dictionary<int, SkillData> MonsterSkillDict { get; private set; } = new Dictionary<int, SkillData>();
        public static Dictionary<int, SkillData> SkillDict { get; private set; } = new Dictionary<int, SkillData>();
        public static Dictionary<EHeroClassType, HeroData> HeroDict { get; private set; } = new Dictionary<EHeroClassType, HeroData>();
        public static Dictionary<int, EffectData> EffectDict { get; private set; } = new Dictionary<int, EffectData>();
        public static Dictionary<int, RewardData> RewardDict { get; private set; } = new Dictionary<int, RewardData>();
        public static Dictionary<int, RewardTableData> RewardTableDict { get; private set; } = new Dictionary<int, RewardTableData>();
        public static Dictionary<int, ItemData> ItemDict { get; private set; } = new Dictionary<int, ItemData>();
        public static Dictionary<int, ConsumableData> ConsumableDict { get; private set; } = new Dictionary<int, ConsumableData>();
        public static Dictionary<int, EquipmentData> EquipmentDict { get; private set; } = new Dictionary<int, EquipmentData>();
        public static Dictionary<int, EtcData> EtcDict { get; private set; } = new Dictionary<int, EtcData>();
        public static Dictionary<int, NPCData> NpcDict { get; private set; } = new Dictionary<int, NPCData>();

        public static void Init()
        {
            HeroStatDict = LoadJson<HeroStatDataLoader, int, HeroStatData>("HeroStatData").MakeDict();
            RoomDict = LoadJson<RoomDataLoader, int, RoomData>("RoomData").MakeDict();
            MonsterDict = LoadJson<MonsterDataLoader, int, MonsterData>("MonsterData").MakeDict();
            SpawningPoolDict = LoadJson<SpawningPoolLoader, int, SpawningPoolData>("SpawningPoolData").MakeDict();
            HeroDict = LoadJson<HeroDataLoader, EHeroClassType, HeroData>("HeroData").MakeDict();
            EffectDict = LoadJson<EffectDataLoader, int, EffectData>("EffectData").MakeDict();
            RewardDict = LoadJson<RewardDataLoader, int, RewardData>("RewardData").MakeDict();
            RewardTableDict = LoadJson<RewardTableDataLoadaer, int, RewardTableData>("RewardTableData").MakeDict();
            NpcDict = LoadJson<NPCDataLoader, int, NPCData>("NPCData").MakeDict();
            //Skill
            HeroSkillDict = LoadJson<SkillDataLoader, int, SkillData>("HeroSkillData").MakeDict();
            MonsterSkillDict = LoadJson<SkillDataLoader, int, SkillData>("MonsterSkillData").MakeDict();
            foreach (KeyValuePair<int, SkillData> skill in HeroSkillDict)
                SkillDict.Add(skill.Key, skill.Value);

            foreach (KeyValuePair<int, SkillData> skill in MonsterSkillDict)
                SkillDict.Add(skill.Key, skill.Value);
            //Item
            ConsumableDict = LoadJson<ConsumableDataLoader, int, ConsumableData>("ConsumableData").MakeDict();
            EquipmentDict = LoadJson<EquipmentDataLoader, int, EquipmentData>("EquipmentData").MakeDict();
            EtcDict = LoadJson<EtcDataLoader, int, EtcData>("EtcData").MakeDict();
            MakeItemDict();
        }

        private static Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
        {
            string text = File.ReadAllText($"{ConfigManager.Config.dataPath}/JSON/{path}.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Loader>(text);
        }

        private static void MakeItemDict()
        {
            //Consumable
            foreach (ConsumableData consumable in ConsumableDict.Values)
            {
                ItemDict.Add(consumable.ItemId, consumable);
            }
            //Equipment
            foreach (EquipmentData equipment in EquipmentDict.Values)
            {
                ItemDict.Add(equipment.ItemId, equipment);
            }
            //Etc
            foreach (EtcData etcData in EtcDict.Values)
            {
                ItemDict.Add(etcData.ItemId, etcData);
            }
        }
    }
}
