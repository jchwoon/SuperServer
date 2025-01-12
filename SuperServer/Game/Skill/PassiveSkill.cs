using SuperServer.Data;
using SuperServer.Game.Object;
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
    }
}
