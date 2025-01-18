using System.IO;
using System.Net;
using ServerCore;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Game.Party;
using SuperServer.Game.Room;
using SuperServer.Logic;
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
            DBLogic.InitDbIds();
            ObjectManager.Instance.PreGenerateId(1000);
            SessionManager.Instance.PreGenerateId(1000);
            RoomManager.Instance.PreLoadRoom();
            PartyManager.Instance.Init();

            IPAddress hostIP = IPAddress.Parse(ConfigManager.Config.ip);
            IPEndPoint endPoint = new IPEndPoint(hostIP, ConfigManager.Config.port);

            _listener.Open(endPoint, () => { return SessionManager.Instance.Generate(); });


            Console.WriteLine("Listen...");

            Task dbTask = new Task(DBLoop, TaskCreationOptions.LongRunning);
            dbTask.Start();

            //MainLoop
            GameLoop();
        }
    }
}