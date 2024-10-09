using Google.Protobuf.Enum;
using SuperServer.Data;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Skill
{
    public class SkillComponent
    {
        Dictionary<int, BaseSkill> _skills = new Dictionary<int, BaseSkill>();
        public Creature Owner { get; private set; }
        public int NormalSkillId { get; private set; }

        public SkillComponent(Creature owner)
        {
            Owner = owner;
        }

        public void RegisterSkill(HeroData heroData)
        {
            foreach (int id in heroData.SkillIds)
            {
                if (DataManager.SkillDict.TryGetValue(id, out SkillData skillData) == true)
                {
                    if (_skills.ContainsKey(id))
                        continue;

                    BaseSkill skill = null;
                    switch (skillData.SkillType)
                    {
                        case ESkillType.Normal:
                            skill = new NormalSkill(Owner, skillData, id);
                            NormalSkillId = skill.SkillId;
                            break;
                    }
                    if (skill != null)
                        _skills.Add(id, skill);
                }
            }
        }
        public void UseSKill(int skillId, int targetId)
        {
            BaseSkill skill;
            if (_skills.TryGetValue(skillId, out skill) == false)
                return;

            skill.UseSkill(targetId);
        }
    }
}
