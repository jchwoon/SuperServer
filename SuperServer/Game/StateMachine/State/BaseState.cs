using SuperServer.Game.Object;
using SuperServer.Game.Room;
using SuperServer.Game.Skill;
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
            //스킬을 사용중인지
            if (_machine.Owner.SkillComponent.CheckLastSkillIsUsing() == true)
                return;

            _machine.Target = _machine.FindTarget();

            //타겟이 있고 거리가 되면 Skill 거리가 안되면 Move
            if (_machine.Target != null)
            {
                float dist = Vector3.Distance(_machine.Owner.Position, _machine.Target.Position);
                BaseSkill skill = _machine.Owner.SkillComponent.GetCanUseSkill(_machine.Target);

                if (dist > (skill == null ? 1.0f : _machine.Owner.SkillComponent.GetSkillRange(skill)))
                {
                    _machine.FindPathAndMove(_machine.Owner.Position, _machine.Target.Position, chase:true);
                    return;
                }
                _machine.ChangeState(_machine.SkillState);
                return;
            }

            //자신의 스포닝풀 범위를 넘었으면 복귀
            if (_machine.OverPoolRange && _owner.AggroComponent.FirstAggroPos.HasValue)
            {
                _machine.FindPathAndMove(_machine.Owner.Position, _owner.AggroComponent.FirstAggroPos.Value, chase:true);
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
