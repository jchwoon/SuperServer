﻿using Google.Protobuf.Protocol;
using Microsoft.EntityFrameworkCore;
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

        public List<DBHero> LoadHero(int accountId)
        {
            using (GameDBContext db = new GameDBContext())
            {
                List<DBHero> heros = db.Heroes.Where(h => h.AccountId == accountId).ToList();
                return heros;
            }
        }

        public DBHero CreateHero(int accountId, ReqCreateHeroToS packet)
        {
            using (GameDBContext db = new GameDBContext())
            {
                DBHero dbHero = db.Heroes.Where(h => h.HeroName == packet.Nickname).FirstOrDefault();
                if (dbHero != null)
                    return null;

                HeroStatData heroStat;
                if (DataManager.HeroStatDict.TryGetValue(1, out heroStat) == false)
                    return null;

                RoomData roomData;
                if (DataManager.RoomDict.TryGetValue(1, out roomData) == false)
                    return null;

                DBHero hero = new DBHero()
                {
                    HeroName = packet.Nickname,
                    AccountId = accountId,
                    Class = packet.ClassType,
                    CreateAt = DateTime.Now,
                    Level = 1,
                    RoomId = 1,
                    Exp = 0,
                    PosX = roomData.StartPosX,
                    PosY = roomData.StartPosY,
                    PosZ = roomData.StartPosZ,
                    EquipmentSlotCount = InventoryComponent.DefaultSlotCount,
                    ConsumableSlotCount = InventoryComponent.DefaultSlotCount,
                    ETCSlotCount = InventoryComponent.DefaultSlotCount,
                };
                hero.HeroStat = new Stats()
                {
                    MP = heroStat.MaxMp,
                    HP = heroStat.MaxHp,
                    MaxMp = heroStat.MaxMp,
                    MaxHp = heroStat.MaxHp,
                    AtkDamage = heroStat.AtkDamage,
                    AtkSpeed = heroStat.BaseAtkSpeed,
                    MoveSpeed = heroStat.MoveSpeed,
                    Defence = heroStat.Defence,
                    AddAtkSpeedMultiplier = heroStat.AddAtkSpeedMultiplier
                };


                db.Heroes.Add(hero);
                if (Extensions.SaveChangeEx(db) == true)
                    return hero;

                return null;
            }
        }

        public bool DeleteHero(int heroId)
        {
            using (GameDBContext db = new GameDBContext())
            {
                DBHero dbHero = db.Heroes.Where(h => h.DBHeroId == heroId).FirstOrDefault();
                if (dbHero == null)
                    return false;

                db.Heroes.Remove(dbHero);

                if (Extensions.SaveChangeEx(db) == true)
                    return true;

                return false;
            }
        }

        public DBHero GetHero(int heroId)
        {
            using (GameDBContext db = new GameDBContext())
            {
                DBHero dbHero = db.Heroes.
                    Where(h => h.DBHeroId == heroId).
                    Include(h => h.Items).
                    FirstOrDefault();
                if (dbHero == null)
                    return null;

                return dbHero;
            }
        }
    }
}
