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
        protected JobTimer _jobTimer = new JobTimer();

        public void Push(Action action) { Push(new Job(action)); }
        public IJob PushAfter(int tickAfter, Action action) { return PushAfter(tickAfter, new Job(action)); }


        public IJob PushAfter(int tickAfter, IJob job)
        {
            _jobTimer.Push(job, tickAfter);
            return job;
        }
        public virtual void Push(IJob job)
        {
            lock (_lock)
            {
                _jobQueue.Enqueue(job);
            }

        }

        public virtual void Execute()
        {
            _jobTimer.Flush();
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
