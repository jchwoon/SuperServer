﻿using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Utils;
using Google.Protobuf.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.StateMachine.State
{
    public class IdleState : BaseState
    {
        public IdleState(MonsterMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _machine.UpdateTick = 1000;
            _machine.Owner.CurrentState = ECreatureState.Idle;
        }
        public override void Exit() { }
        public override void Update()
        {
            base.Update();

            if (_machine.Owner.SkillComponent.CheckLastSkillIsUsing() == true)
                return;

            //_machine.CheckArrivalFirstAggroPos();
            _machine.PatrolPos = GetPatrolRandomPos();
        }

        //랜덤 위치를 찾으며 순회하기
        private Vector3? GetPatrolRandomPos()
        {
            float range = _owner.PoolData.SpawnRange;
            //스포닝 풀 범위는 넘지 않기
            //0 ~ 9
            int rand = _random.Next(0, 10);
            if (rand < 4)
                return null;

            float randX = (_random.NextSingle() * 2) - 1;
            float randZ = (_random.NextSingle() * 2) - 1;

            Vector3 randPos = _owner.Position + (new Vector3(randX, 0, randZ).Normalize());
            
            Vector3 centerToDest = (randPos - _poolCenter);
            float centerToDestDist = centerToDest.Magnitude();
            if (centerToDestDist > range)
                return null;

            if (_owner.Room.Map.CanGo(randPos.Z, randPos.X) == false)
                return null;

            return randPos;
        }
    }
}
