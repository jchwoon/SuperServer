using Google.Protobuf.Enum;
using SuperServer.Game.Object;
using SuperServer.Game.Skill;
using SuperServer.Game.StateMachine.State;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.StateMachine
{
    public class MonsterMachine : StateMachine<Monster>
    {
        public IdleState IdleState { get; set; }
        public MoveState MoveState { get; set; }
        public SkillState SkillState { get; set; }
        public DieState DieState { get; set; }
        public Vector3? PatrolPos { get; set; }
        public BaseObject Target { get; set; }
        public float ToNextPosDist { get; private set; }
        public BaseSkill CurrentSkill { get;  set; }
        public bool isBackToOriginPos { get; set; }
        public MonsterMachine(Monster monster)
        {
            Owner = monster;
            InitState();
        }

        private void InitState()
        {
            IdleState = new IdleState(this);
            MoveState = new MoveState(this);
            SkillState = new SkillState(this);
            DieState = new DieState(this);
        }
        public Creature FindTarget()
        {
            List<int> targetIds = Owner.AggroComponent.GetAttackerIdsSortByDamage();

            foreach (int id in targetIds)
            {
                Hero target = Owner.Room?.FindHeroById(id);

                if (target.CurrentState == ECreatureState.Die)
                {
                    Owner.AggroComponent.ClearTarget(target);
                    continue;
                }
                return target;
            }

            if (Owner.MonsterData.AggroType != EMonsterAggroType.Auto)
                return null;

            float detectionRangeSqr = Owner.MonsterData.DetectionRange * Owner.MonsterData.DetectionRange;
            List<Hero> heroes = Owner.Room.FindHeroInInterestRegion(Owner.Position)
                .Where(h => (h.Position - Owner.Position).MagnitudeSqr() <= detectionRangeSqr)
                .OrderBy(h => (h.Position - Owner.Position).MagnitudeSqr()).ToList();

            foreach (Hero hero in heroes)
            {
                if (hero.CurrentState == ECreatureState.Die) continue;
                return hero;
            }

            return null;
        }

        public bool FindPathAndMove(Vector3 start, Vector3 dest, bool chase = false)
        {
            Vector3Int rounStart = Vector3Int.Vector3ToVector3Int(start);
            Vector3Int roundDest = Vector3Int.Vector3ToVector3Int(dest);
            List<Vector3Int> path = Owner.Room?.Map.FindPath(rounStart, roundDest);

            //0번째는 본인 위치 1번부터 다음위치
            if (path == null || path.Count <= 1)
            {
                ChangeState(IdleState);
                return false;
            }

            ToNextPosDist = Vector3Int.Distance(Vector3Int.Vector3ToVector3Int(Owner.Position), path[1]);

            ChangeState(MoveState);
            Owner.Room?.Map.ApplyMove(Owner, path[1]);
            Owner.BroadcastMove(null, moveType : chase == true ? EMoveType.Chase : EMoveType.None);
            return true;
        }

        public void CheckArrivalFirstAggroPos()
        {
            if (Owner.AggroComponent.FirstAggroPos.HasValue == false || isBackToOriginPos == false)
                return;

            float distSqr = (Owner.Position - Owner.AggroComponent.FirstAggroPos.Value).MagnitudeSqr();
            if (distSqr <= 0.1f)
            {
                Owner.Reset();
                Owner.AggroComponent.FirstAggroPos = null;
                isBackToOriginPos = false;
            }
        }

        public bool IsChaseMode()
        {
            if (Target != null || isBackToOriginPos)
                return true;

            return false;
        }

        public void OnDamage()
        {
            if (Target == null)
                CurrentState.Update();
        }

        public override void OnDie()
        {
            base.OnDie();

            if (Target != null)
                Target = null;
            ChangeState(DieState);
        }
    }
}
