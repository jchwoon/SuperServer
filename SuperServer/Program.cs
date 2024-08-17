using System.Net;
using SuperServer.Data;

namespace SuperServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigManager.LoadConfigData();


            IPAddress hostIP = IPAddress.Parse(ConfigManager.Config.ip);
            IPEndPoint endPoint = new IPEndPoint(hostIP, ConfigManager.Config.port);
        }
    }
}