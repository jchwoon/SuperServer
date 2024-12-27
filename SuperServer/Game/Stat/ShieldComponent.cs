using Google.Protobuf.Protocol;
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
        
        public ShieldComponent(Creature owner)
        {
            _owner = owner;
        }
        public int OnDamage(int damage)
        {
            int remainDamage = damage;
            List<ShieldTimer> timer = new List<ShieldTimer>();
            timer.AddRange(_shieldTimerQueue.GetAllDatas());
            foreach (ShieldTimer shieldTimer in timer)
            {
                if (remainDamage <= 0) break;
                if (_shields.TryGetValue(shieldTimer.TemplateId, out Shield shield))
                {
                    int shieldValue = shield.ShieldValue;
                    shield.OnDamage(remainDamage);
                    remainDamage = Math.Max(0, remainDamage - shieldValue);

                    if (shield.IsBroken)
                    {
                        RemoveShield(shieldTimer.TemplateId);
                    }
                    else
                    {
                        SendShieldValue();
                    }
                }
            }

            return remainDamage;
        }

        public void ApplyShield(EffectData effectData, int value)
        {
            int templateId = effectData.TemplateId;

            _shields[templateId] = new Shield(value);

            ShieldTimer shieldTimer = new ShieldTimer
            {
                DurationTick = System.Environment.TickCount64 + (long)(effectData.Duration * 1000),
                TemplateId = templateId
            };

            _shieldTimerQueue.Push(shieldTimer);

            SendShieldValue();
        }

        public void RemoveShield(int templateId)
        {
            if (_shields.TryGetValue(templateId, out Shield shield))
            {
                _shields.Remove(templateId);
                SendShieldValue();
                _owner.EffectComponent.ReleaseEffect(templateId);
            }

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

        private void SendShieldValue()
        {
            int totalShieldValue = 0;
            foreach (Shield shield in _shields.Values)
            {
                totalShieldValue += shield.ShieldValue;
            }

            ChangeShieldValueToC changeShieldPacket = new ChangeShieldValueToC();
            changeShieldPacket.ObjectId = _owner.ObjectId;
            changeShieldPacket.ShieldValue = totalShieldValue;

            _owner.Room?.Broadcast(changeShieldPacket, _owner.Position);
        }
    }
}
