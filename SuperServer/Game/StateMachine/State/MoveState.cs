using Google.Protobuf.Enum;
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
            _machine.Owner.CurrentState = ECreatureState.Move;
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

            //CheckIsOverOfPoolRange();
            //_machine.CheckArrivalFirstAggroPos();
            CalculateUpdateTick();
        }

        private void CalculateUpdateTick()
        {
            float nextDist = _machine.ToNextPosDist;
            StatInfo info = _machine.Owner.StatComponent.StatInfo;
            float speed = _machine.IsChaseMode() ? info.ChaseSpeed : info.MoveSpeed;
            _machine.UpdateTick = (int)((nextDist / speed) * 1000);
        }

        //private void CheckIsOverOfPoolRange()
        //{
        //    if (_owner.PoolData == null)
        //        return;
        //    if (_owner.AggroComponent.FirstAggroPos.HasValue == false)
        //        return;

        //    float range = _owner.PoolData.SpawnRange;
        //    float dist = Vector3.Distance(_owner.Position, _poolCenter);

        //    if (dist > range)
        //    {
        //        _owner.AggroComponent.ClearTarget(_machine?.Target);
        //        _machine.isBackToOriginPos = true;
        //    }
        //}


    }
}
