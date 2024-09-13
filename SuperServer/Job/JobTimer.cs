using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Job
{
    struct JobTimerElem : IComparable<JobTimerElem>
    {
        public long execTime;
        public IJob job;

        public int CompareTo(JobTimerElem other)
        {
            return (int)(execTime - other.execTime);
        }
    }

    public class JobTimer
    {
        PriorityQueue<JobTimerElem> _pq = new PriorityQueue<JobTimerElem>();
        object _lock = new object();

        public int Count { get { lock (_lock) { return _pq.Count; } } }

        public void Push(IJob job, float tickAfter = 0)
        {
            JobTimerElem jobElement;
            jobElement.execTime = (long)(System.Environment.TickCount64 + tickAfter);
            jobElement.job = job;

            lock (_lock)
            {
                _pq.Push(jobElement);
            }
        }

        public void Flush()
        {
            while (true)
            {
                long now = System.Environment.TickCount64;

                JobTimerElem jobElement;

                lock (_lock)
                {
                    if (_pq.Count == 0)
                        break;

                    jobElement = _pq.Peek();
                    if (jobElement.execTime > now)
                        break;

                    _pq.Pop();
                }

                jobElement.job.Execute();
            }
        }
    }
}