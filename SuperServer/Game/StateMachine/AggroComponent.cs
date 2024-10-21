using SuperServer.Game.Object;
using SuperServer.Utils;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.StateMachine
{
    public class AggroComponent
    {
        Dictionary<int, float> _attackers = new Dictionary<int, float>();
        //맨 처음 공격 당했을 때의 위치
        public Vector3? FirstAggroPos { get; set; }
        public Creature Owner { get; private set; }
        public AggroComponent(Creature owner)
        {
            Owner = owner;
        }

        public int GetTargetIdFromAttackers()
        {
            return _attackers.OrderByDescending(x => x.Value).Select(x => x.Key).FirstOrDefault();
        }
        public void OnDamage(int objectId, float damage)
        {
            if (FirstAggroPos == null)
                FirstAggroPos = Owner.Position;
            if (_attackers.ContainsKey(objectId))
                _attackers[objectId] += damage;
            else
                _attackers.Add(objectId, damage);
        }

        public void Clear()
        {
            _attackers.Clear();
            FirstAggroPos = null;
        }
        public void ClearTarget(BaseObject creature)
        {
            if (creature != null && _attackers.ContainsKey(creature.ObjectId))
            {
                _attackers.Remove(creature.ObjectId);
            }
        }
    }
}
