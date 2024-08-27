using SuperServer.DB;
using SuperServer.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Commander
{
    public class DBCommander : JobCommander
    {
        private static DBCommander _instance;
        public static DBCommander Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DBCommander();

                return  _instance;
            }
        }

        public void Update()
        {
            Execute();
        }

        public List<Hero> LoadHeroDb(int accountId)
        {
            using (GameDBContext db = new GameDBContext())
            {
                List<Hero> heros = db.Heros.Where(h => h.AccountId == accountId).ToList();
                return heros;
            }
        }
    }
}
