using Google.Protobuf.Enum;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        protected ECreatureState CreatureState { get; private set; }

        public virtual void ChangeState(IState changeState)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            CurrentState = changeState;

            CurrentState.Enter();
        }

        public void Update()
        {
            if (CurrentState == null)
                return;

            if (Owner.Room == null)
                return;

            CurrentState.Update();
        }
    }
}
