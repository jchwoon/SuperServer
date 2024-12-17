using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.StateMachine.State
{
    public class HitState : BaseState
    {
        public HitState(MonsterMachine machine) : base(machine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _machine.UpdateTick = 500;
            _machine.Owner.CurrentState = ECreatureState.Hit;
            _machine.Owner.SkillComponent.CancelCurrentRegisterSkill();
        }
        public override void Exit() { }
        public override void Update()
        {
            base.Update();
        }
    }
}
