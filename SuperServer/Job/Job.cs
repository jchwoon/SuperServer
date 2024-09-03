using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Job
{
    public interface IJob
    {
        public abstract void Execute();
    }
    public class Job : IJob
    {
        Action _action;
        public Job(Action action)
        {
            _action = action;
        }
        public void Execute()
        {
            _action.Invoke();
        }
    }
}
