using Google.Protobuf.Enum;
using SuperServer.Data;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Skill.Effect
{
    public interface IEffectPolicy
    {
        public void Attach(Creature owner, Creature provider, EffectDataEx data);
        public void Detach(Creature owner, EffectDataEx data);
    }

    public struct EffectDataEx
    {
        public EffectData effectData;
        public int level;
    }
    public class Effect
    {
        public long EffectId { get; private set; }
        public IEffectPolicy Policy { get; private set; }
        public Creature Owner { get; private set; }
        public Creature Provider { get; private set; }
        public EffectDataEx EffectDataEx { get; private set; }
        public Effect(long effectId, Creature owner, Creature provider, EffectDataEx data, IEffectPolicy policy)
        {
            EffectId = effectId;
            Policy = policy;
            Owner = owner;
            Provider = provider;
            EffectDataEx = data;
        }

        public void Apply()
        {
            Policy?.Attach(Owner, Provider, EffectDataEx);
        }

        public void Release()
        {
            Policy?.Detach(Owner, EffectDataEx);
        }
    }

    public class DamageEffectPolicy : IEffectPolicy
    {
        public void Attach(Creature owner, Creature provider, EffectDataEx data)
        {
            float damage = provider.StatComponent.StatInfo.AtkDamage * data.effectData.DamageRatio;
            owner.OnDamage(provider, damage);
        }

        public void Detach(Creature owner, EffectDataEx data)
        {
            
        }
    }

    public class HealEffectPolicy : IEffectPolicy
    {
        public void Attach(Creature owner, Creature provider, EffectDataEx data)
        {
            foreach (AddStatInfo info in data.effectData.AddStatValues)
            {
                owner.AddStat(info.StatType, info.Value, EFontType.Heal);
            }
        }

        public void Detach(Creature owner, EffectDataEx data)
        {

        }
    }
    //단순히 스텟만 적용되는 Effect Ex) 아이템 장착
    public class AddStatEffectPolicy : IEffectPolicy
    {
        public void Attach(Creature owner, Creature provider, EffectDataEx data)
        {
            foreach (AddStatInfo info in data.effectData.AddStatValues)
            {
                float value = info.Value;
                owner.AddStat(info.StatType, value, sendPacket:false);
            }
            owner.BroadcastStat();
        }

        public void Detach(Creature owner, EffectDataEx data)
        {
            foreach (AddStatInfo info in data.effectData.AddStatValues)
            {
                float addValue = info.Value;
                owner.AddStat(info.StatType, -addValue, sendPacket: false);
            }
            owner.BroadcastStat();
        }
    }

    public class ShieldEffectPolicy : IEffectPolicy
    {
        public void Attach(Creature owner, Creature provider, EffectDataEx data)
        {
            EEffectScalingType scalingType = data.effectData.EffectScalingType;
            int value = 0;
            switch(scalingType)
            {
                case EEffectScalingType.Maxhp:
                    float maxHp = provider.StatComponent.GetStat(EStatType.MaxHp);
                    value = (int)(maxHp * (data.effectData.HealthRatio + (data.level * data.effectData.GapPerLevel)));
                    break;
            }
            owner.ShieldComponent.ApplyShield(data.effectData, value);
        }

        public void Detach(Creature owner, EffectDataEx data)
        {
            owner.ShieldComponent.RemoveShield(data.effectData.TemplateId);
        }
    }

    public class AggroEffectPolicy : IEffectPolicy
    {
        public void Attach(Creature owner, Creature provider, EffectDataEx data)
        {

        }

        public void Detach(Creature owner, EffectDataEx data)
        {

        }
    }
}
