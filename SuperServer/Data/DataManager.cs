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
        public static Dictionary<int, HeroStatData> HeroDict { get; private set; } = new Dictionary<int, HeroStatData>();
        public static Dictionary<int, RoomData> RoomDict { get; private set; } = new Dictionary<int, RoomData>();
        public static void Init()
        {
            HeroDict = LoadJson<HeroStatDataLoader, int, HeroStatData>("HeroStat").MakeDict();
            RoomDict = LoadJson<RoomDataLoader, int, RoomData>("RoomData").MakeDict();
        }

        private static Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
        {
            string text = File.ReadAllText($"{ConfigManager.Config.dataPath}/JSON/{path}.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Loader>(text);
        }
    }
}
