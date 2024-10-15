using SuperServer.Game.Object;
using SuperServer.Game.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.StateMachine.State
{
    public class SkillState : BaseState
    {
        public SkillState(MonsterMachine machine) : base(machine)
        {
        }

        public override void Update()
        {
            base.Update();
            Console.WriteLine("Skill");

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

            //Monster owner = _machine.Owner;
            //BaseSkill skill = owner.SkillComponent.GetCanUseSkillAtReservedSkills(_machine.Target);
            //if (skill != null)
            //{
            //    owner.SendReqUseSkill(skill.SkillId, _machine.Target.ObjectId);
            //    return;
            //}
        }


    }
}
