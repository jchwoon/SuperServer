using SuperServer.Game.Object;
using SuperServer.Game.Skill;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public override void Enter()
        {
            base.Enter();
            _machine.UpdateTick = (int)(_machine.Owner.MonsterData.AtkSpeed * 1000);
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

            if (_machine.Target == null)
                return;

            if (_machine.CurrentSkill != null)
            {
                _machine.CurrentSkill.UseSkill(_machine.Target.ObjectId);
                return;
            }
            else
                _machine.Owner.SkillComponent.UseNormalSkill(_machine.Target.ObjectId);

        }


    }
}
