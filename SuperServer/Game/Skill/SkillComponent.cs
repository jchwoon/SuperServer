using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Job;
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
        public IJob CurrentRegisterJob { get; set; }

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
                    if (skill.SkillData.SkillSlotType == ESkillSlotType.Normal)
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

            //들어온 스킬이 이전 스킬을 취소 할 수 있는지 확인
            if (skill.SkillData.CanCancel)
            {
                CancelCurrentRegisterSkill();
            }
            else if (CheckLastSkillIsUsing() == true)
                return;

            if (skill.CheckCanUseSkill() == false)
                return;

            //콤보스킬인지 체크
            if (skill.SkillData.IsComboSkill)
                skill.CalculateComboAndApply();

            //위의 스킬이 콤보스킬이면 스킬 인포에서 받은 콤보스킬아이디를 통해 skillData를
            //꺼내서 위의 skill의 SkillData를 교체
            skill.UseSkill(skillInfo.SkillTargetId, skillInfo.SkillLocationTargetId, skillInfo.RotY);
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

        public void CancelCurrentRegisterSkill()
        {
            if (CurrentRegisterJob == null)
                return;

            LastSkill = null;
            CurrentRegisterJob.IsCancel = true;
        }
    }
}
