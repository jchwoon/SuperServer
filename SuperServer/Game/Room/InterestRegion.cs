﻿using Google.Protobuf.Protocol;
using SuperServer.Commander;
using SuperServer.Game.Object;
using Google.Protobuf.Enum;
using SuperServer.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Struct;
using System.Threading;

namespace SuperServer.Game.Room
{
    public class InterestRegion
    {
        public Hero Owner { get; private set; }
        public HashSet<Creature> CurrentInterestCreature { get; private set; } = new HashSet<Creature>();
        public InterestRegion(Hero owner)
        {
            Owner = owner;
        }

        public void Update()
        {
            if (Owner == null || Owner.Room == null)
                return;

            HashSet<Creature> interestCreatures = GetCreatureInInterestRegion();

            //신입생 스폰
            List<Creature> newCreatures = interestCreatures.Except(CurrentInterestCreature).ToList();
            if (newCreatures.Count > 0)
            {
                SpawnToC spawnPacket = new SpawnToC();
                foreach (Creature creature in newCreatures)
                {
                    if (creature.ObjectType == EObjectType.Hero)
                    {
                        Hero hero = (Hero)creature;
                        HeroInfo info = new HeroInfo();
                        info.MergeFrom(hero.HeroInfo);
                        spawnPacket.Heroes.Add(info);
                    }
                    else if (creature.ObjectType == EObjectType.Monster)
                    {
                        Monster monster = (Monster)creature;
                        CreatureInfo info = new CreatureInfo();
                        info.MergeFrom(monster.CreatureInfo);
                        spawnPacket.Creatures.Add(info);
                    }
                }
                Owner.Session?.Send(spawnPacket);
            }
            //관심 영역 밖 디스폰
            List<Creature> oldCreatures = CurrentInterestCreature.Except(interestCreatures).ToList();
            if (oldCreatures.Count > 0)
            {
                DeSpawnToC deSpawnPacket = new DeSpawnToC();
                foreach (Creature creature in oldCreatures)
                {
                    deSpawnPacket.ObjectIds.Add(creature.ObjectId);
                }
                Owner.Session?.Send(deSpawnPacket);
            }

            CurrentInterestCreature = interestCreatures;

            GameCommander.Instance.PushAfter(100, Update);
        }

        private HashSet<Creature> GetCreatureInInterestRegion()
        {
            if (Owner == null || Owner.Room == null)
                return null;

            HashSet<Creature> interestCreatures = new HashSet<Creature>();


            List<Creature> creatures = Owner.Room.GetCreatures();

            //개선점
            foreach (Creature creature in creatures)
            {
                if (creature.ObjectId == Owner.ObjectId) continue;
                float dist = (creature.Position - Owner.Position).MagnitudeSqr();

                if (dist  < GameRoom.SqrInterestRange)
                    interestCreatures.Add(creature);
            }

            return interestCreatures;
        }

        public void Clear()
        {
            CurrentInterestCreature.Clear();
        }
    }
}
