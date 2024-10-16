using Google.Protobuf.Struct;
using SuperServer.Utils;
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
            _machine.PatrolPos = null;
            CalculateUpdateTick();
        }
        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            if (_machine.Owner.SkillComponent.CheckLastSkillIsUsing() == true)
                return;

            CheckIsOverOfPoolRange();
            CheckArrivalFirstAggroPos();
            CalculateUpdateTick();
        }

        private void CalculateUpdateTick()
        {
            float nextDist = _machine.ToNextPosDist;
            StatInfo info = _machine.Owner.StatComponent.StatInfo;
            float speed = _machine.IsChaseMode() ? info.ChaseSpeed : info.MoveSpeed;
            _machine.UpdateTick = (int)((nextDist / speed) * 1000);
        }

        private void CheckIsOverOfPoolRange()
        {
            if (_owner.PoolData == null)
                return;
            if (_owner.AggroComponent.FirstAggroPos.HasValue == false)
                return;

            float range = _owner.PoolData.SpawnRange;
            float dist = Vector3.Distance(_owner.Position, _poolCenter);

            //복귀 후 스텟 정상화
            if (dist > range)
            {
                _owner.AggroComponent.Clear();
                _machine.Target = null;
                _machine.OverPoolRange = true;
            }
        }

        private void CheckArrivalFirstAggroPos()
        {
            if (_owner.AggroComponent.FirstAggroPos.HasValue == false)
                return;

            float distSqr = (_owner.Position - _owner.AggroComponent.FirstAggroPos.Value).MagnitudeSqr();
            if (distSqr <= 0.1f)
            {
                //Todo 원래 상태로 회복
                _owner.AggroComponent.FirstAggroPos = null;
                _machine.OverPoolRange = false;
            }
        }
    }
}
