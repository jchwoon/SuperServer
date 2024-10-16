﻿using Google.Protobuf.Struct;
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

        public override void Enter()
        {
            base.Enter();
            //거 = 속 * 시
            //시 = 거 / 속
            CalculateUpdateTick();
        }
        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            CalculateUpdateTick();

            _machine.Target = _machine.FindTarget();
            if (_machine.Target != null)
            {
                float dist = (_machine.Target.Position - _owner.Position).Magnitude();
                if (dist <= _owner.MonsterData.AtkRange)
                {
                    _machine.ChangeState(_machine.SkillState);
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

        private void CalculateUpdateTick()
        {
            float nextDist = _machine.ToNextPosDist;
            StatInfo info = _machine.Owner.StatComponent.StatInfo;
            float speed = _machine.Target == null ? info.MoveSpeed : info.ChaseSpeed;
            _machine.UpdateTick = (int)((nextDist / speed) * 1000);
        }
    }
}
