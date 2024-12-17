using Google.Protobuf.Enum;
using SuperServer.Commander;
using SuperServer.Game.Object;
using SuperServer.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SuperServer.Game.StateMachine
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void Update();
    }
    public class StateMachine<T> where T : Creature
    {
        public virtual T Owner { get; protected set; }
        public IState CurrentState { get; private set; }
        public int UpdateTick { get; set; } = 1000;
        IJob _machineJob;

        public virtual void ChangeState(IState changeState)
        {
            if (CurrentState == changeState) return;
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            CurrentState = changeState;
            CurrentState.Enter();

        }

        public virtual void Update()
        {
            if (CurrentState == null)
                return;

            if (Owner.Room == null)
                return;


            CurrentState.Update();

            _machineJob = GameCommander.Instance.PushAfter(UpdateTick, Update);
        }

        public virtual void OnDie()
        {
            
        }
    }
}
