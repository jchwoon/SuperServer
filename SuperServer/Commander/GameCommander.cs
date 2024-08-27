using SuperServer.DB;
using SuperServer.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Commander
{
    public class GameCommander : JobCommander
    {
        private static GameCommander _instance;
        public static GameCommander Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameCommander();

                return _instance;
            }
        }

        public void Update()
        {
            Execute();
        }
    }
}
