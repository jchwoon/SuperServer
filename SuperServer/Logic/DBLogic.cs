using Microsoft.EntityFrameworkCore;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.DB;
using SuperServer.Game.Inventory;
using SuperServer.Game.Room;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
                dbHero.HeroStat.AddAtkSpeedMultiplier = hero.StatComponent.StatInfo.AddAtkSpeedMultiplier;

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

        public void ApplyChangedCountItem(int dbItemId, int addCount)
        {
            using (GameDBContext db = new GameDBContext())
            {
                DBItem dbItem = db.Items.Where(i => i.DBItemId == dbItemId).FirstOrDefault();

                if (dbItem != null)
                {
                    dbItem.Count += addCount;
                    db.Entry(dbItem).Property(nameof(DBItem.Count)).IsModified = true;
                }

                bool success = db.SaveChangeEx();
            }
        }

        public void AddNewItem(Hero hero, DBItem newItemDb)
        {
            using (GameDBContext db = new GameDBContext())
            {
                if (newItemDb != null)
                    db.Items.Add(newItemDb);

                bool success = db.SaveChangeEx();
                if (success)
                {
                    GameCommander.Instance.Push(ItemFactory.Instance.ApplyNewItemToInven, hero, newItemDb);
                }
            }
        }

        public void UseConsumeItem(InventoryComponent inventory, Consumable consumeItem)
        {
            if (inventory == null || consumeItem == null)
                return;

            if (consumeItem.Count < 0)
                return;

            DBItem dbItem = new DBItem
            {
                DBItemId = consumeItem.Info.ItemDbId,
                Count = consumeItem.Count
            };

            using (GameDBContext db = new GameDBContext())
            {
                if (dbItem.Count == 0)
                {
                    db.Items.Remove(dbItem);
                }
                else
                {
                    //var record6 = new TestModel { ID = 4, Value = "EF Core !" };
                    //dbContext.Attach(record6); 
                    db.Attach(dbItem);
                    db.Entry(dbItem).Property(nameof(dbItem.Count)).IsModified = true;
                }


                bool success = db.SaveChangeEx();
            }
        }

        public void UpdateEquipItem(Hero hero, Item item)
        {
            if (hero == null || item == null)
                return;

            DBItem dbItem = new DBItem()
            {
                DBItemId = item.ItemDbId,
                SlotType = item.SlotType
            };

            using (GameDBContext db = new GameDBContext())
            {
                db.Attach(dbItem);
                db.Entry(dbItem).Property(nameof(dbItem.SlotType)).IsModified = true;

                bool success = db.SaveChangeEx();
            }
        }
    }
}
