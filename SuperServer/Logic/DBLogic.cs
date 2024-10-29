using SuperServer.Data;
using SuperServer.DB;
using SuperServer.Game.Room;
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
        public void SaveHero(Hero hero, GameRoom room)
        {
            if (hero == null)
                return;

            using (GameDBContext db = new GameDBContext())
            {
                DBHero dbHero = db.Heroes.Where(h => h.DBHeroId == hero.DbHeroId).FirstOrDefault();

                if (dbHero == null) return;

                //위치
                dbHero.RoomId = room.RoomId;
                dbHero.PosX = hero.PosInfo.PosX;
                dbHero.PosY = hero.PosInfo.PosY;
                dbHero.PosZ = hero.PosInfo.PosZ;
                dbHero.RotY = hero.PosInfo.RotY;

                //스텟
                dbHero.HeroStat.MaxHp = hero.StatComponent.StatInfo.MaxHp;
                dbHero.HeroStat.HP = hero.StatComponent.StatInfo.Hp;
                dbHero.HeroStat.MaxMp = hero.StatComponent.StatInfo.MaxMp;
                dbHero.HeroStat.MP = hero.StatComponent.StatInfo.Mp;
                dbHero.HeroStat.AtkSpeed = hero.StatComponent.StatInfo.AtkSpeed;
                dbHero.HeroStat.AtkDamage = hero.StatComponent.StatInfo.AtkDamage;
                dbHero.HeroStat.Defence = hero.StatComponent.StatInfo.Defence;
                dbHero.HeroStat.MoveSpeed = hero.StatComponent.StatInfo.MoveSpeed;

                dbHero.Exp = hero.MyHeroInfo.Exp;
                dbHero.Gold = hero.MyHeroInfo.Gold;
                dbHero.Level = hero.MyHeroInfo.HeroInfo.LobbyHeroInfo.Level;

                if (hero.StatComponent.StatInfo.Hp == 0)
                {
                    dbHero.PosX = room.RoomData.StartPosX;
                    dbHero.PosY = room.RoomData.StartPosY;
                    dbHero.PosZ = room.RoomData.StartPosZ;

                    dbHero.HeroStat.HP = hero.StatComponent.StatInfo.MaxHp;
                }

                if (db.SaveChangeEx() == false)
                {
                    Console.WriteLine("Failed Save Hero");
                }
            }
        }
    }
}
