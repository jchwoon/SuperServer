using Google.Protobuf.Protocol;
using SuperServer.Data;
using SuperServer.DB;
using SuperServer.Job;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

        public List<Hero> LoadHero(int accountId)
        {
            using (GameDBContext db = new GameDBContext())
            {
                List<Hero> heros = db.Heros.Where(h => h.AccountId == accountId).ToList();
                return heros;
            }
        }

        public Hero CreateHero(int accountId, ReqCreateHeroToS packet)
        {
            using (GameDBContext db = new GameDBContext())
            {
                Hero dbHero = db.Heros.Where(h => h.HeroName == packet.Nickname).FirstOrDefault();
                if (dbHero != null)
                    return null;

                HeroStatData heroStat;
                if (DataManager.HeroDict.TryGetValue(1, out heroStat) == false)
                {
                    return null;
                }

                Hero hero = new Hero()
                {
                    HeroName = packet.Nickname,
                    AccountId = accountId,
                    Class = packet.ClassType,
                    CreateAt = DateTime.Now,
                    Level = 1,
                };
                hero.HeroStat = new Stats()
                {
                    MP = heroStat.MaxMp,
                    HP = heroStat.MaxHp,
                    MaxMp = heroStat.MaxMp,
                    MaxHp = heroStat.MaxHp,
                    AttackDamage = heroStat.AtkDamage,
                    AtkSpeed = heroStat.AtkSpeed,
                    MoveSpeed = heroStat.MoveSpeed,
                    Exp = heroStat.Exp,
                    Defense = heroStat.Defence
                };


                db.Heros.Add(hero);
                if (SExtension.SaveChangeEx(db) == true)
                    return hero;

                return null;
            }
        }

        public bool DeleteHero(int heroId)
        {
            using (GameDBContext db = new GameDBContext())
            {
                Hero dbHero = db.Heros.Where(h => h.HeroId == heroId).FirstOrDefault();
                if (dbHero == null)
                    return false;

                db.Heros.Remove(dbHero);

                if (SExtension.SaveChangeEx(db) == true)
                    return true;

                return false;
            }
        }
    }
}
