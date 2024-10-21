using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SuperServer.Job
{
    public interface IJob
    {
        public bool IsCancel { get; set; }
        public abstract void Execute();
    }
    public class Job : IJob
    {
        Action _action;
        public bool IsCancel { get; set; }
        public Job(Action action)
        {
            _action = action;
        }
        public void Execute()
        {
            if (IsCancel == false)
            {

                _action.Invoke();
            }
            else
            {
                Console.WriteLine("cancle");
            }
        }
    }
    public class Job<T1> : IJob
    {
        Action<T1> _action;
        T1 _t1;
        public bool IsCancel { get; set; }

        public Job(Action<T1> action, T1 t1)
        {
            _action = action;
            _t1 = t1;
        }

        public void Execute()
        {
            if (IsCancel == false)
                _action.Invoke(_t1);
        }
    }

    public class Job<T1, T2> : IJob
    {
        Action<T1, T2> _action;
        T1 _t1;
        T2 _t2;
        public bool IsCancel { get; set; }

        public Job(Action<T1, T2> action, T1 t1, T2 t2)
        {
            _action = action;
            _t1 = t1;
            _t2 = t2;
        }

        public void Execute()
        {
            if (IsCancel == false)
                _action.Invoke(_t1, _t2);
        }
    }

    public class Job<T1, T2, T3> : IJob
    {
        Action<T1, T2, T3> _action;
        T1 _t1;
        T2 _t2;
        T3 _t3;
        public bool IsCancel { get; set; }

        public Job(Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            _action = action;
            _t1 = t1;
            _t2 = t2;
            _t3 = t3;
        }

        public void Execute()
        {
            if (IsCancel == false)
                _action.Invoke(_t1, _t2, _t3);
        }
    }
}
