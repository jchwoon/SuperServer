using SuperServer.Game.Object;
using SuperServer.Game.Room;
using SuperServer.Game.Skill;
using SuperServer.Utils;
using Google.Protobuf.Enum;
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
            if (_owner.Room != null)
            {
                Update();
            }

        }

        public virtual void Exit()
        {
        }

        public virtual void Update()
        {
            if (_machine.Owner.CurrentState == ECreatureState.Die)
                return;

            if (_machine.CheckIsBehaviorLocking())
                return;
            //스킬을 사용중인지
            if (_machine.Owner.SkillComponent.CheckLastSkillIsUsing() == true)
                return;

            _machine.Target = _machine.FindTarget();

            //자신의 스포닝풀 범위를 넘었으면 복귀 or 어그로가 끌렸는데 갑자기 타겟이 없어졌으면 복귀
            //if (_owner.AggroComponent.FirstAggroPos.HasValue)
            //{
            //    if (_machine.isBackToOriginPos || _machine.Target == null)
            //    {
            //        _machine.FindPathAndMove(_machine._owner.Position, _owner.AggroComponent.FirstAggroPos.Value, chase: true);
            //        return;
            //    }
            //}

            //타겟이 있고 거리가 되면 Skill 거리가 안되면 Move
            if (_machine.Target != null)
            {
                BaseSkill skill = _machine.Owner.SkillComponent.GetCanUseSkill(_machine.Target);

                if (skill == null)
                {
                    _machine.FindPathAndMove(_machine.Owner.Position, _machine.Target.Position, chase:true);
                    return;
                }
                _machine.CurrentSkill = skill;
                _machine.ChangeState(_machine.SkillState);
                return;
            }

            //패트롤포스가 있으면 정찰
            if (_machine.PatrolPos.HasValue == true)
            {
                _machine.FindPathAndMove(_machine.Owner.Position, _machine.PatrolPos.Value);
                return;
            }

            //위의 모든 사항에 해당하지 않으면 Idle
            _machine.ChangeState(_machine.IdleState);
            return;
        }
    }
}
