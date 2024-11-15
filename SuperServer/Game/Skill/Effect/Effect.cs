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
        public void Attach(Creature owner, Creature provider, EffectData data);
        public void Detach(Creature owner, EffectData data);
    }
    public class Effect
    {
        public long EffectId { get; private set; }
        public IEffectPolicy Policy { get; private set; }
        public Creature Owner { get; private set; }
        public Creature Provider { get; private set; }
        public EffectData EffectData { get; private set; }
        public Effect(long effectId, Creature owner, Creature provider, EffectData data, IEffectPolicy policy)
        {
            EffectId = effectId;
            Policy = policy;
            Owner = owner;
            Provider = provider;
            EffectData = data;
        }

        public void Apply()
        {
            Policy?.Attach(Owner, Provider, EffectData);
        }

        public void Release()
        {
            Policy?.Detach(Owner, EffectData);
        }
    }

    public class DamageEffectPolicy : IEffectPolicy
    {
        public void Attach(Creature owner, Creature provider, EffectData data)
        {
            float damage = provider.StatComponent.StatInfo.AtkDamage * data.DamageRatio;
            owner.OnDamage(provider, damage);
        }

        public void Detach(Creature owner, EffectData data)
        {
            
        }
    }

    public class HealEffectPolicy : IEffectPolicy
    {
        public void Attach(Creature owner, Creature provider, EffectData data)
        {
            foreach (AddStatInfo info in data.AddStatValues)
            {
                owner.AddStat(info.StatType, info.Value);
            }
        }

        public void Detach(Creature owner, EffectData data)
        {

        }
    }
    //단순히 스텟만 적용되는 Effect Ex) 아이템 장착
    public class AddStatEffectPolicy : IEffectPolicy
    {
        public void Attach(Creature owner, Creature provider, EffectData data)
        {
            foreach (AddStatInfo info in data.AddStatValues)
            {
                float value = info.Value;
                owner.AddStat(info.StatType, value, sendPacket:false);
            }
            owner.BroadcastStat();
        }

        public void Detach(Creature owner, EffectData data)
        {
            foreach (AddStatInfo info in data.AddStatValues)
            {
                float addValue = info.Value;
                owner.AddStat(info.StatType, -addValue, sendPacket: false);
            }
            owner.BroadcastStat();
        }
    }
}
