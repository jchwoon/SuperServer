using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Job;
using SuperServer.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Skill
{
    public abstract class BaseSkill
    {
        private int skillId;

        public Creature Owner { get; protected set; }
        public int TemplateId { get; protected set; }
        public SkillData SkillData { get; protected set; }
        public int CurrentSkillLevel { get; protected set; }

        public BaseSkill(Creature owner, SkillData skillData, int templateId, int skillLevel)
        {
            Owner = owner;
            SkillData = skillData;
            TemplateId = templateId;
            CurrentSkillLevel = skillLevel;    
        }

        public ESkillType GetSkillType()
        {
            if (SkillData == null)
                return ESkillType.None;

            return SkillData.SkillType;
        }

        #region Skill Level
        public bool CheckCanLevelUp(int point)
        {
            int maxLevel = SkillData.MaxLevel;

            if (maxLevel < CurrentSkillLevel + point)
                return false;

            return true;
        }

        public virtual void UpdateSkillLevel(int level)
        {
            CurrentSkillLevel = level;
        }
        #endregion
    }
}
