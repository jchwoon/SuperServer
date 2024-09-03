using System.IO;
using System.Net;
using ServerCore;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Game.Room;
using SuperServer.Session;

namespace SuperServer
{
    class Program
    {
        static Listener _listener = new Listener();

        static void GameLoop()
        {
            while (true)
            {
                GameCommander.Instance.Update();
                Thread.Sleep(100);
            }
        }

        static void DBLoop()
        {
            while (true)
            {
                DBCommander.Instance.Update();
                Thread.Sleep(100);
            }
        }
        static void Main(string[] args)
        {
            ConfigManager.LoadConfigData();
            DataManager.Init();
            RoomManager.Instance.PreLoadRoom();
            ObjectManager.Instance.PreGenerateId(1000);

            IPAddress hostIP = IPAddress.Parse(ConfigManager.Config.ip);
            IPEndPoint endPoint = new IPEndPoint(hostIP, ConfigManager.Config.port);

            _listener.Open(endPoint, () => { return new ClientSession(); });


            Console.WriteLine("Listen...");

            Thread dbThread = new Thread(DBLoop);
            dbThread.Name = "dbThread";
            dbThread.Start();

            //MainLoop
            GameLoop();
        }
    }
}