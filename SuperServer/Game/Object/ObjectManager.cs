using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Object
{
    public class ObjectManager : Singleton<ObjectManager>
    {
        object _lock = new object();
        Queue<int> _objIdQueue;
        int _nextId;

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

            lock (_lock)
            {
                obj.ObjectId = GenerateId();
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
                    _objIdQueue.Enqueue(_nextId);
                    return _nextId++;
                }

                return _objIdQueue.Dequeue();
            }

        }
    }
}
