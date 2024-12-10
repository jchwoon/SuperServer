using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
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
            _machine.Owner.CurrentState = ECreatureState.Skill;
            _machine.UpdateTick = (int)((1.0f / _machine.Owner.MonsterData.AtkSpeed) * 1000);
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

            //Todo
            _machine.CurrentSkill.UseSkill(_machine.Target.ObjectId, _machine.Target.ObjectId, null);
        }
    }
}
