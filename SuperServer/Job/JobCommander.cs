using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Job
{
    public class JobCommander
    {
        protected object _lock = new object();
        protected Queue<IJob> _jobQueue = new Queue<IJob>();
        public virtual void Push(Action action)
        {
            Job job = new Job(action);
            lock (_lock)
            {
                _jobQueue.Enqueue(job);
            }

        }

        public virtual void Execute()
        {
            while (true)
            {
                IJob job = Pop();
                if (job == null)
                    return;

                job.Execute();
            }
        }

        private IJob Pop()
        {
            lock (_lock)
            {
                if (_jobQueue.Count == 0)
                    return null;

                return _jobQueue.Dequeue();
            }
        }
    }
}
