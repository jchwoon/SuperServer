using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Session
{
    public class SessionManager : Singleton<SessionManager>
    {
        int _nextId = 0;

        Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
        object _lock = new object();
        Queue<int> _sessionIdQueue;


        public void PreGenerateId(int capacity)
        {
            lock (_lock)
            {
                _sessionIdQueue = new Queue<int>(capacity);

                for (int i = 1; i <= capacity; i++)
                {
                    _sessionIdQueue.Enqueue(i);
                }
                _nextId = capacity + 1;
            }
        }
        public ClientSession Generate()
        {
            lock (_lock)
            {
                int sessionId = GenerateId();
                ClientSession session = new ClientSession();

                session.SessionId = sessionId;
                _sessions.Add(sessionId, session);
                return session;
            }
        }

        public void Remove(ClientSession session)
        {
            lock(_lock)
            {
                _sessionIdQueue.Enqueue(session.SessionId);
                _sessions.Remove(session.SessionId);
            }
        }

        private int GenerateId()
        {
            lock (_lock)
            {
                if (_sessionIdQueue.Count == 0)
                {
                    return _nextId++;
                }

                return _sessionIdQueue.Dequeue();
            }

        }
    }
}
