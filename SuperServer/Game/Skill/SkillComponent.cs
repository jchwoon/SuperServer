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

        public void UseNormalSkill(int targetId)
        {
            UseSKill(NormalSkillId, targetId);
        }

        //쿨타임 없이 쓸 수 있는 스킬들 일반 공격스킬 제외
        public BaseSkill GetCanUseSkill(BaseObject target)
        {
            foreach(BaseSkill skill in _skills.Values)
            {
                if (skill.SkillId == NormalSkillId) continue;
                if (skill.UseSkill(target.ObjectId) == true)
                {
                    return skill;
                }
            }

            return null;
        }

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
