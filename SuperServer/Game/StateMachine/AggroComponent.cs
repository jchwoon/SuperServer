using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.StateMachine
{
    public class AggroComponent
    {
        Dictionary<int, float> _attackers = new Dictionary<int, float>();


        public int GetTargetIdFromAttackers()
        {
            return _attackers.OrderByDescending(x => x.Value).Select(x => x.Key).FirstOrDefault();
        }
        public void OnDamage(int objectId, float damage)
        {
            if (_attackers.ContainsKey(objectId))
                _attackers[objectId] += damage;
            else
                _attackers.Add(objectId, damage);
        }
    }
}
