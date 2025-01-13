using Google.Protobuf.Enum;
using Microsoft.EntityFrameworkCore;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.DB;
using SuperServer.Game.Inventory;
using SuperServer.Game.Object;
using SuperServer.Game.Room;
using SuperServer.Game.Skill;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Logic
{
    public class DBLogic : Singleton<DBLogic>
    {
        public static long _itemDbIdGenerator = 0;
        public static long GenerateItemDbId() { return Interlocked.Increment(ref _itemDbIdGenerator); }

        public static void InitDbIds()
        {
            using (var context = new GameDBContext())
            {
                DBItem itemDb = context.Items.OrderByDescending(i => i.DBItemId).FirstOrDefault();
                if (itemDb != null)
                    Interlocked.Exchange(ref _itemDbIdGenerator, itemDb.DBItemId);
            }
        }

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
                dbHero.HeroStat.HP = hero.StatComponent.StatInfo.Hp;
                dbHero.HeroStat.MP = hero.StatComponent.StatInfo.Mp;

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

                bool success = db.SaveChangeEx();
                if (success == false)
                {
                    hero?.Session.CloseClientSocket();
                }
            }
        }

        public void ApplyChangedCountItem(long dbItemId, int addCount)
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
                if (success == false)
                {
                    Hero hero = ObjectManager.Instance.FindHeroByDbId(dbItem.OwnerDbId);
                    if (hero != null)
                    {
                        hero?.Session.CloseClientSocket();
                    }
                }
            }
        }

        public void AddNewItem(Hero hero, DBItem newItemDb)
        {
            using (GameDBContext db = new GameDBContext())
            {
                if (newItemDb != null)
                    db.Items.Add(newItemDb);

                bool success = db.SaveChangeEx();
                if (success == false)
                {
                    hero?.Session.CloseClientSocket();
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
                    db.Attach(dbItem);
                    db.Entry(dbItem).Property(nameof(dbItem.Count)).IsModified = true;
                }


                bool success = db.SaveChangeEx();
                if (success == false)
                {
                    inventory.Owner?.Session.CloseClientSocket();
                }
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
                if (success == false)
                {
                    hero?.Session.CloseClientSocket();
                }
            }
        }
        #region Skill
        public void SaveSkillList(Hero hero)
        {
            if (hero == null)
                return;

            Dictionary<int, int> skills = hero.SkillComponent.GetAllSkillLevels();

            using (GameDBContext db = new GameDBContext())
            {
                DBHero dbHero = db.Heroes.Where(h => h.DBHeroId == hero.DbHeroId).FirstOrDefault();
                if (dbHero == null)
                    return;

                dbHero.Skills = skills;

                bool success = db.SaveChangeEx();
                if (success == false)
                {
                    hero?.Session.CloseClientSocket();
                }
            }
        }

        public void SaveSkillPoint(Hero hero, int point)
        {
            if (hero == null)
                return;

            using (GameDBContext db = new GameDBContext())
            {
                DBHero dbHero = db.Heroes.Where(h => h.DBHeroId == hero.DbHeroId).FirstOrDefault();
                if (dbHero == null)
                    return;

                dbHero.SkillPoint = point;

                bool success = db.SaveChangeEx();
                if (success == false)
                {
                    hero?.Session.CloseClientSocket();
                }
            }
        }
        #endregion
    }
}
