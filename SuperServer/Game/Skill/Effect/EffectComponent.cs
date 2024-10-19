using SuperServer.Data;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Skill.Effect
{
    public class EffectComponent
    {
        public Creature Owner;
        public EffectComponent(Creature owner)
        {
            Owner = owner;
        }

        public void ApplyEffect(Creature giver, EffectData data)
        {
            float damage = giver.StatComponent.StatInfo.AtkDamage * data.DamageRatio;
            Owner.OnDamage(giver, damage);
        }
    }
}
