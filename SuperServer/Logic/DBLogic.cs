using SuperServer.DB;
using SuperServer.Game.Object;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Logic
{
    public class DBLogic : Singleton<DBLogic>
    {
        public void SaveHero(Hero hero)
        {
            if (hero == null)
                return;

            using (GameDBContext db = new GameDBContext())
            {
                DBHero dbHero = db.Heroes.Where(h => h.DBHeroId == hero.HeroId).FirstOrDefault();

                if (dbHero == null) return;

                dbHero.PosX = hero.HeroInfo.CreatureInfo.ObjectInfo.PosInfo.PosX;
                dbHero.PosY = hero.HeroInfo.CreatureInfo.ObjectInfo.PosInfo.PosY;
                dbHero.PosZ = hero.HeroInfo.CreatureInfo.ObjectInfo.PosInfo.PosZ;
                dbHero.RotY = hero.HeroInfo.CreatureInfo.ObjectInfo.PosInfo.RotY;

                if (db.SaveChangeEx() == false)
                {
                    Console.WriteLine("Failed Save Hero");
                }
            }
        }
    }
}
