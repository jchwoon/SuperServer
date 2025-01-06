using SuperServer.Data;
using SuperServer.Game.Object;
using Google.Protobuf.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperServer.Commander;
using Google.Protobuf.Protocol;

namespace SuperServer.Game.Skill.Effect
{
    public class EffectComponent
    {
        Dictionary<long, Effect> _effects = new Dictionary<long, Effect>();
        public Creature Owner;

        public static long _effectId = 1;
        public static readonly Dictionary<EEffectType, IEffectPolicy> _policies = new Dictionary<EEffectType, IEffectPolicy>()
        {
            { EEffectType.Damage, new DamageEffectPolicy() },
            { EEffectType.Heal, new HealEffectPolicy() },
            { EEffectType.Addstat, new AddStatEffectPolicy() },
            { EEffectType.Shield, new ShieldEffectPolicy() },
            { EEffectType.Aggro, new AggroEffectPolicy() },
        };
        
        public EffectComponent(Creature owner)
        {
            Owner = owner;
        }

        public void ApplyEffect(Creature provider, EffectDataEx data)
        {
            if (data.effectData == null)
                return;

            switch (data.effectData.EffectDurationType)
            {
                case EEffectDurationType.None:
                    ApplyInstantEffect(provider, data);
                    break;
                case EEffectDurationType.Temporary:
                    ApplyTemporaryEffect(provider, data);
                    break;
                case EEffectDurationType.Permanent:
                    ApplyPermanentEffect(provider, data);
                    break;
            }
            
        }


        private void ApplyInstantEffect(Creature provider, EffectDataEx data)
        {
            IEffectPolicy policy;
            if (_policies.TryGetValue(data.effectData.EffectType, out policy) == false)
                return;
            policy.Attach(Owner, provider, data);
        }

        private void ApplyTemporaryEffect(Creature provider, EffectDataEx data)
        {
            IEffectPolicy policy;
            if (_policies.TryGetValue(data.effectData.EffectType, out policy) == false)
                return;
            long effectId = GenerateEffectId();
            Effect effect = GenerateEffect(effectId, Owner, provider, data, policy);

            if (data.effectData.Stackable == false)
            {
                ReleaseEffect(data.effectData.TemplateId);
            }
            _effects.Add(effectId, effect);
            effect.Apply();

            SendApplyEffect(effect);

            int durationTick = (int)(data.effectData.Duration * 1000);
            GameCommander.Instance.PushAfter(durationTick, () =>
            {
                ReleaseEffect(effectId);
            });
        }

        private void ApplyPermanentEffect(Creature provider, EffectDataEx data)
        {
            IEffectPolicy policy;
            if (_policies.TryGetValue(data.effectData.EffectType, out policy) == false)
                return;
            long effectId = GenerateEffectId();
            Effect effect = GenerateEffect(effectId, Owner, provider, data, policy);
            _effects.Add(effectId, effect);

            effect.Apply();

            SendApplyEffect(effect);
        }

        public void ReleaseEffect(int templateId)
        {
            Console.WriteLine(_effects.Count);
            Effect effect = _effects.Values.FirstOrDefault(e => e.EffectDataEx.effectData.TemplateId == templateId);
            if (effect != null)
            {
                Console.WriteLine(effect.EffectId);
                _effects.Remove(effect.EffectId);
                effect.Release();
                SendReleaseEffect(effect);
            }
        }

        public void ReleaseEffect(long effectId)
        {
            if (_effects.TryGetValue(effectId, out Effect effect))
            {
                _effects.Remove(effect.EffectId);
                effect.Release();
                SendReleaseEffect(effect);
            }
        }

        public List<Effect> GetAllEffect()
        {
            return _effects.Values.ToList();
        }

        public static long GenerateEffectId()
        {
            return Interlocked.Increment(ref _effectId);
        }

        private Effect GenerateEffect(long effectId, Creature owner, Creature provider, EffectDataEx data, IEffectPolicy policy)
        {
            Effect effect = new Effect(effectId, owner, provider, data, policy);
            return effect;
        }

        #region Packet Send
        private void SendApplyEffect(Effect effect)
        {
            if (effect == null) return;

            ApplyEffectToC apllyPacket = new ApplyEffectToC();
            apllyPacket.ObjectId = Owner.ObjectId;
            apllyPacket.TemplateId = effect.EffectDataEx.effectData.TemplateId;
            apllyPacket.EffectId = effect.EffectId;

            Owner.Room?.Broadcast(apllyPacket, Owner.Position);
        }

        private void SendReleaseEffect(Effect effect)
        {
            if (effect == null) return;

            ReleaseEffectToC releasePacket = new ReleaseEffectToC();
            releasePacket.ObjectId = Owner.ObjectId;
            releasePacket.EffectId = effect.EffectId;

            Owner.Room?.Broadcast(releasePacket, Owner.Position);
        }
        #endregion
    }
}
