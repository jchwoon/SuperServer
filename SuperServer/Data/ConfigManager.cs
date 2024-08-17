using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Data
{
    public struct ConfigData
    {
        public string ip;
        public int port;
    }
    public class ConfigManager
    {
        public static ConfigData Config { get; private set; }
        

        public void LoadConfigData(string path = "./config.json")
        {
            string text = File.ReadAllText(path);
            Config = JsonConvert.DeserializeObject<ConfigData>(text);
        }
    }
}
