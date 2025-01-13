using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Game.Skill.Effect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Skill
{
    public class PassiveSkill : BaseSkill
    {
        public PassiveSkillData PassiveSkillData { get; protected set; }
        public PassiveSkill(Creature owner, PassiveSkillData skillData, int templateId, int skillLevel) : base(owner, skillData, templateId, skillLevel)
        {
            PassiveSkillData = skillData;
        }

        public override void UpdateSkillLevel(int level)
        {
            base.UpdateSkillLevel(level);

            foreach (int id in PassiveSkillData.EffectIds)
            {
                DataManager.EffectDict.TryGetValue(id, out EffectData effectData);
                if (effectData != null)
                {
                    //이전 레벨에 관련된 패시브 먼저 해제
                    Owner.EffectComponent.ReleaseEffect(effectData.TemplateId);

                    if (level <= 0) break;
                    EffectDataEx effectEx = new EffectDataEx()
                    {
                        effectData = effectData,
                        level = CurrentSkillLevel,
                    };
                    Owner.EffectComponent.ApplyEffect(Owner, effectEx);
                }
            }
        }
    }
}
