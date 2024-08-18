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
            ConfigManager.LoadConfigData();


            IPAddress hostIP = IPAddress.Parse(ConfigManager.Config.ip);
            IPEndPoint endPoint = new IPEndPoint(hostIP, ConfigManager.Config.port);

            _listener.Open(endPoint, () => { return new ClientSession(); });
        }
    }
}