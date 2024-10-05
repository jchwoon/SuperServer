using SuperServer.Game.Object;
using SuperServer.Game.Room;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.StateMachine.State
{
    public class BaseState : IState
    {
        protected MonsterMachine _machine;
        protected Monster _owner;
        protected Vector3 _poolCenter;
        protected Random _random = new Random();
        public BaseState(MonsterMachine machine)
        {
            _machine = machine;
            _owner = _machine.Owner;
            _poolCenter = new Vector3(_owner.PoolData.PosX, _owner.PoolData.PosY, _owner.PoolData.PosZ);
        }
        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Update()
        {
        }
    }
}
