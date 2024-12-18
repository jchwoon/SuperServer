﻿using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Enum;
using System.Threading;

namespace SuperServer.Game.Object
{
    public class ObjectManager : Singleton<ObjectManager>
    {
        object _lock = new object();
        Queue<int> _objIdQueue;
        int _nextId;

        Dictionary<int, Hero> _heroes = new Dictionary<int, Hero>();
        Dictionary<int, Monster> _monsters = new Dictionary<int, Monster>();
        Dictionary<int, DropItem> _dropItems = new Dictionary<int, DropItem>();
        Dictionary<int, NPC> _npcs = new Dictionary<int, NPC>();

        public void PreGenerateId(int capacity)
        {
            lock (_lock)
            {
                _objIdQueue = new Queue<int>(capacity);

                for (int i = 1; i <= capacity; i++)
                {
                    _objIdQueue.Enqueue(i);
                }
                _nextId = capacity + 1;
            }
        }
        public T Spawn<T>() where T : BaseObject, new()
        {
            T obj = new T();

            Type type = typeof(T);

            if (type == typeof(Hero))
            {
                Hero hero = obj as Hero;
                hero.ObjectId = GenerateId();
                hero.ObjectType = EObjectType.Hero;
                _heroes.Add(hero.ObjectId, hero);
            }
            else if (type == typeof(Monster))
            {
                Monster monster = obj as Monster;
                monster.ObjectId = GenerateId();
                monster.ObjectType = EObjectType.Monster;
                _monsters.Add(monster.ObjectId,monster);
            }
            else if (type == typeof(DropItem))
            {
                DropItem dropItem = obj as DropItem;
                dropItem.ObjectId = GenerateId();
                dropItem.ObjectType = EObjectType.DropItem;
                _dropItems.Add(dropItem.ObjectId, dropItem);
            }
            else if(type == typeof(NPC))
            {
                NPC npc = obj as NPC;
                npc.ObjectId = GenerateId();
                npc.ObjectType = EObjectType.Npc;
                _npcs.Add(npc.ObjectId, npc);
            }

            return obj;
        }

        public void DeSpawn(int objId)
        {
            lock (_lock)
            {
                //해당 오브젝트 리셋
                _objIdQueue.Enqueue(objId);
            }
        }

        private int GenerateId()
        {
            lock(_lock)
            {
                if (_objIdQueue.Count == 0)
                {
                    return _nextId++;
                }

                return _objIdQueue.Dequeue();
            }

        }
    }
}
