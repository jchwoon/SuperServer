using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
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
        public BaseSkill LastSkill { get; set; }

        public SkillComponent(Creature owner)
        {
            Owner = owner;
        }

        public void RegisterSkill(List<int> skillIds)
        {
            foreach (int id in skillIds)
            {
                if (DataManager.SkillDict.TryGetValue(id, out SkillData skillData) == true)
                {
                    if (_skills.ContainsKey(id))
                        continue;

                    BaseSkill skill = null;
                    switch (skillData.SkillProjectileType)
                    {
                        case ESkillProjectileType.None:
                            skill = new NonProjectileSkill(Owner, skillData, id);
                            
                            break;
                    }
                    if (skill.SkillData.IsNormalSkill == true)
                        NormalSkillId = skill.TemplateId;
                    if (skill != null)
                        _skills.Add(id, skill);
                }
            }
        }

        public void UseSKill(SkillInfo skillInfo)
        {
            BaseSkill skill;
            if (_skills.TryGetValue(skillInfo.SkillId, out skill) == false)
                return;

            skill.UseSkill(skillInfo.TargetId, skillInfo.RotY);
        }

        #region AI
        //등록된 skill들 중 사용 가능한 스킬을 뽑기 일반 스킬을 맨 마지막에
        public BaseSkill GetCanUseSkill(BaseObject target)
        {
            foreach (BaseSkill skill in _skills.Values)
            {
                if (skill.TemplateId == NormalSkillId) continue;
                if (skill.CheckCanUseSkill(target) == true)
                {
                    return skill;
                }
            }

            BaseSkill normalSkill = _skills[NormalSkillId];
            if (normalSkill.CheckCanUseSkill(target) == true)
                return normalSkill;

            return null;
        }
        #endregion

        public BaseSkill GetSkillById(int skillId)
        {
            BaseSkill skill;
            if (_skills.TryGetValue(skillId, out skill) == false)
                return null;

            return skill;
        }

        public bool CheckLastSkillIsUsing()
        {
            if (LastSkill == null)
                return false;
            return LastSkill.CheckUsingSkill();
        }

        public float GetSkillRange(BaseSkill skill)
        {
            return skill.GetSkillRange();
        }
    }
}
