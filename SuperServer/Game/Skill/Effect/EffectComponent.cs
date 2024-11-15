using SuperServer.Data;
using SuperServer.Game.Object;
using Google.Protobuf.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Skill.Effect
{
    public class EffectComponent
    {
        Dictionary<long, Effect> _effects = new Dictionary<long, Effect>();
        public Creature Owner;

        public static long _effectIdGenerator = 1;
        public static readonly Dictionary<EEffectType, IEffectPolicy> _policies = new Dictionary<EEffectType, IEffectPolicy>()
        {
            { EEffectType.Damage, new DamageEffectPolicy() },
            { EEffectType.Heal, new HealEffectPolicy() },
            { EEffectType.Addstat, new AddStatEffectPolicy() },
        };
        
        public EffectComponent(Creature owner)
        {
            Owner = owner;
        }

        public void ApplyEffect(Creature provider, EffectData data)
        {
            switch (data.EffectDurationType)
            {
                case EEffectDurationType.None:
                    ApplyInstantEffect(provider, data);
                    break;
                case EEffectDurationType.Temporary:
                    ApplyTemporaryEffect();
                    break;
                case EEffectDurationType.Permanent:
                    ApplyPermanentEffect(provider, data);
                    break;
            }
            
        }


        private void ApplyInstantEffect(Creature provider, EffectData data)
        {
            IEffectPolicy policy;
            if (_policies.TryGetValue(data.EffectType, out policy) == false)
                return;
            policy.Attach(Owner, provider, data);
        }

        private void ApplyTemporaryEffect()
        {

        }

        private void ApplyPermanentEffect(Creature provider, EffectData data)
        {
            IEffectPolicy policy;
            if (_policies.TryGetValue(data.EffectType, out policy) == false)
                return;
            long effectId = GenerateEffectId();
            Effect effect = GenerateEffect(effectId, Owner, provider, data, policy);
            _effects.Add(effectId, effect);

            effect.Apply();
        }

        public void ReleaseEffect(int effectId)
        {
            Effect effect = _effects.Values.FirstOrDefault(e => e.EffectData.EffectId == effectId);
            if (effect != null)
            {
                _effects.Remove(effect.EffectId);
                effect.Release();
            }
        }

        public List<Effect> GetAllEffect()
        {
            return _effects.Values.ToList();
        }

        public static long GenerateEffectId()
        {
            return Interlocked.Increment(ref _effectIdGenerator);
        }

        private Effect GenerateEffect(long effectId, Creature owner, Creature provider, EffectData data, IEffectPolicy policy)
        {
            Effect effect = new Effect(effectId, owner, provider, data, policy);
            return effect;
        }
    }
}
