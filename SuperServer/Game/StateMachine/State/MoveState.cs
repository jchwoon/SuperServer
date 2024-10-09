using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.StateMachine.State
{
    public class MoveState : BaseState
    {
        public MoveState(MonsterMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Update()
        {
            base.Update();

            _machine.Target = _machine.FindTarget();
            if (_machine.Target != null)
            {
                float dist = (_machine.Target.Position - _owner.Position).Magnitude();
                if (dist <= _owner.MonsterData.AtkRange)
                {
                    _machine.ChangeState(_machine.AttackState);
                    return;
                }
                else
                {
                    _machine.FindPathAndMove(_owner.Position, _machine.Target.Position);
                    return;
                }
            }
            if (_machine.PatrolPos.HasValue)
            {
                _machine.FindPathAndMove(_owner.Position, _machine.PatrolPos.Value);
                return;
            }
            _machine.ChangeState(_machine.IdleState);
        }
    }
}
