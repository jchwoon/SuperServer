using Google.Protobuf.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.StateMachine.State
{
    public class DieState : BaseState
    {
        public DieState(MonsterMachine machine) : base(machine)
        {
        }

        public override void Enter()
        {
            _machine.UpdateTick = 500;
            _machine.Owner.CurrentState = ECreatureState.Die;
        }
        public override void Exit() { }
        public override void Update()
        {

        }
    }
}
