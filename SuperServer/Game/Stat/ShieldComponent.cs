using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Game.Skill.Effect;
using SuperServer.Job;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Stat
{
    struct ShieldTimer : IComparable<ShieldTimer>
    {
        public long DurationTick;
        public int TemplateId;

        public int CompareTo(ShieldTimer other)
        {
            return (int)(DurationTick - other.DurationTick);
        }
    }
    public class ShieldComponent
    {
        Creature _owner;
        PriorityQueue<ShieldTimer> _shieldTimerQueue = new PriorityQueue<ShieldTimer>();
        Dictionary<int, Shield> _shields = new Dictionary<int, Shield>();
        //Dictionary<int, long> _shieldIds = new Dictionary<int, long>();
        
        public ShieldComponent(Creature owner)
        {
            _owner = owner;
        }
        public int OnDamage(int damage)
        {
            int remainDamage = damage;
            foreach (ShieldTimer shieldTimer in _shieldTimerQueue.GetAllDatas())
            {
                if (remainDamage <= 0) break;
                if (_shields.TryGetValue(shieldTimer.TemplateId, out Shield shield))
                {
                    int shieldValue = shield.ShieldValue;
                    shield.OnDamage(remainDamage);
                    remainDamage = Math.Max(0, remainDamage - shieldValue);

                    if (shield.IsBroken)
                        RemoveShield(shieldTimer.TemplateId);
                }
            }

            return remainDamage;
        }

        public void ApplyShield(EffectData effectData, int value)
        {
            int templateId = effectData.TemplateId;

            //같은 Effect가 들어오면 제거하기
            if (_shields.ContainsKey(templateId))
            {
                RemoveShield(templateId);
            }

            _shields[templateId] = new Shield(value);

            ShieldTimer shieldTimer = new ShieldTimer
            {
                DurationTick = System.Environment.TickCount64 + (long)(effectData.Duration * 1000),
                TemplateId = templateId
            };

            _shieldTimerQueue.Push(shieldTimer);
        }

        public void RemoveShield(int templateId)
        {
            _owner.EffectComponent.ReleaseEffect(templateId);
            _shields.Remove(templateId);

            while (_shieldTimerQueue.Count != 0)
            {
                ShieldTimer shieldTimer = _shieldTimerQueue.Peek();
                if (shieldTimer.DurationTick < System.Environment.TickCount64)
                {
                    _shieldTimerQueue.Pop();
                }
                else
                {
                    break;
                }
            }
        }
    }
}
