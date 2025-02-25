﻿using Google.Protobuf.Enum;
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
        Dictionary<int, int> _attackers = new Dictionary<int, int>();
        //맨 처음 공격 당했을 때의 위치
        //public Vector3? FirstAggroPos { get; set; }
        public Creature Owner { get; private set; }
        public Creature ForceAggroTarget { get; private set; }
        public AggroComponent(Creature owner)
        {
            Owner = owner;
        }

        public List<int> GetAttackerIdsSortByDamage()
        {
            List<int> sortedIdByDamage = new List<int>();
            if (ForceAggroTarget != null)
            {
                sortedIdByDamage.Add(ForceAggroTarget.ObjectId);
            }
            foreach (int i in _attackers.OrderByDescending(x => x.Value).Select(x => x.Key))
            {
                sortedIdByDamage.Add(i);
            }

            return sortedIdByDamage;
        }

        public int GetTopDamageAttackerId()
        {
            return _attackers.OrderByDescending(x => x.Value).Select(x => x.Key).FirstOrDefault();
        }

        public void OnDamage(int objectId, int damage)
        {
            if (Owner.CurrentState == ECreatureState.Die)
                return;

            if (_attackers.ContainsKey(objectId))
                _attackers[objectId] += damage;
            else
                _attackers.Add(objectId, damage);
        }

        public void ForceAggro(Creature target)
        {
            if (Owner.CurrentState == ECreatureState.Die)
                return;

            ForceAggroTarget = target;
        }

        public void RemoveForceAggroTarget()
        {
            ForceAggroTarget = null;
        }

        public void Clear()
        {
            _attackers.Clear();
            //FirstAggroPos = null;
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
