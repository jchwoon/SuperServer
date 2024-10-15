using Google.Protobuf.Enum;
using SuperServer.Game.Object;
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
        public Vector3? PatrolPos { get; set; }
        public BaseObject Target { get; set; }
        public float ToNextPosDist { get; private set; }
        public MonsterMachine(Monster monster)
        {
            Owner = monster;
            SetState();
        }

        private void SetState()
        {
            IdleState = new IdleState(this);
            MoveState = new MoveState(this);
            SkillState = new SkillState(this);
        }
        public Creature FindTarget()
        {
            int targetId = Owner.AggroComponent.GetTargetIdFromAttackers();
            Hero target = Owner.Room?.FindHeroById(targetId);
            if (target == null)
                return null;

            return target;
        }

        public void FindPathAndMove(Vector3 start, Vector3 dest)
        {
            Vector3Int rounStart = Vector3Int.Vector3ToVector3Int(start);
            Vector3Int roundDest = Vector3Int.Vector3ToVector3Int(dest);
            List<Vector3Int> path = Owner.Room?.Map.FindPath(rounStart, roundDest);

            //0번째는 본인 위치 1번부터 다음위치
            if (path == null || path.Count <= 1)
            {
                ChangeState(IdleState);
                return;
            }


            ToNextPosDist = Vector3Int.Distance(Vector3Int.Vector3ToVector3Int(Owner.Position), path[1]);

            ChangeState(MoveState);
            Owner.Room?.Map.ApplyMove(Owner, path[1]);
            Owner.BroadcastMove(null, moveType : Target == null ? EMoveType.None : EMoveType.Chase);
        }

        public void OnDamage(Creature attacker)
        {
            //Hit
            //어그로가 일단 끌려야함
            
        }
    }
}
