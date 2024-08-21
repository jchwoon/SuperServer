using System.IO;
using System.Net;
using ServerCore;
using SuperServer.Data;

namespace SuperServer
{
    class Program
    {
        static Listener _listener = new Listener();
        static void Main(string[] args)
        {
            ConfigManager.LoadConfigData(path:"../../.././config.json");


            IPAddress hostIP = IPAddress.Parse(ConfigManager.Config.ip);
            IPEndPoint endPoint = new IPEndPoint(hostIP, ConfigManager.Config.port);

            _listener.Open(endPoint, () => { return new ClientSession(); });


            Console.WriteLine("Listen...");
            while (true)
            {

            }
        }
    }
}