using Google.Protobuf.Protocol;
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
        public HashSet<BaseObject> CurrentInterestObj { get; private set; } = new HashSet<BaseObject>();
        public InterestRegion(Hero owner)
        {
            Owner = owner;
        }

        public void Update()
        {
            if (Owner == null || Owner.Room == null)
                return;

            HashSet<BaseObject> interestObjects = GetCreatureInInterestRegion();

            //신입생 스폰
            List<BaseObject> newObjects = interestObjects.Except(CurrentInterestObj).ToList();
            if (newObjects.Count > 0)
            {
                SpawnToC spawnPacket = new SpawnToC();
                foreach (BaseObject obj in newObjects)
                {
                    if (obj.ObjectType == EObjectType.Hero)
                    {
                        Hero hero = (Hero)obj;
                        HeroInfo info = new HeroInfo();
                        info.MergeFrom(hero.HeroInfo);
                        spawnPacket.Heroes.Add(info);
                    }
                    else if (obj.ObjectType == EObjectType.Monster)
                    {
                        Monster monster = (Monster)obj;
                        CreatureInfo info = new CreatureInfo();
                        info.MergeFrom(monster.CreatureInfo);
                        spawnPacket.Creatures.Add(info);
                    }
                    else if (obj.ObjectType == EObjectType.DropItem)
                    {
                        DropItem dropItem = (DropItem)obj;
                        if (dropItem.Owner == null || dropItem.Owner.ObjectId == Owner.ObjectId)
                        {
                            ObjectInfo info = new ObjectInfo();
                            info.MergeFrom(dropItem.ObjectInfo);
                            spawnPacket.Objects.Add(info);
                        }
                    }
                }
                Owner.Session?.Send(spawnPacket);
            }
            //관심 영역 밖 디스폰
            List<BaseObject> oldObjects = CurrentInterestObj.Except(interestObjects).ToList();
            if (oldObjects.Count > 0)
            {
                DeSpawnToC deSpawnPacket = new DeSpawnToC();
                foreach (BaseObject obj in oldObjects)
                {
                    deSpawnPacket.ObjectIds.Add(obj.ObjectId);
                }
                Owner.Session?.Send(deSpawnPacket);
            }

            CurrentInterestObj = interestObjects;

            GameCommander.Instance.PushAfter(100, Update);
        }

        private HashSet<BaseObject> GetCreatureInInterestRegion()
        {
            if (Owner == null || Owner.Room == null)
                return null;

            HashSet<BaseObject> interestObj = new HashSet<BaseObject>();


            List<BaseObject> objects = Owner.Room.GetAllObjects();

            //개선점
            foreach (BaseObject obj in objects)
            {
                if (obj.ObjectId == Owner.ObjectId) continue;
                float dist = (obj.Position - Owner.Position).MagnitudeSqr();

                if (dist  < GameRoom.SqrInterestRange)
                    interestObj.Add(obj);
            }

            return interestObj;
        }

        public void Clear()
        {
            CurrentInterestObj.Clear();
        }
    }
}
