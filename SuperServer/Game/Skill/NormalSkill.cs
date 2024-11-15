using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Skill
{
    public class NormalSkill : BaseSkill
    {
        int _currentComboIdx = 0;
        Dictionary<int, string> _normalSkillComboName = new Dictionary<int, string>();
        public NormalSkill(Creature owner, SkillData skillData, int skillId) : base(owner, skillData, skillId)
        {
            for (int i = 0; i < skillData.ComboNames.Count; i++)
            {
                _normalSkillComboName.Add(i, skillData.ComboNames[i]);
            }
        }

        protected override bool CheckCoolTime()
        {
            long elapsedTime = GetElapsedTimeAfterLastUseSkill();
            if (elapsedTime >= SkillData.AnimTime * GetCalculateAtkSpeedTick())
                return true;

            return false;
        }

        protected override int GetSkillDelayTick()
        {
            return (int)(GetCalculateAtkSpeedTick() * SkillData.EffectDelayRatio);
        }

        protected override string GetPlayAnimName()
        {
            if (CheckSuccessCombo() == false)
                _currentComboIdx = 0;
            return _normalSkillComboName[_currentComboIdx];
        }
        protected override void Refresh()
        {
            base.Refresh();
            UpdateComboIdx();
        }

        //콤보 적용여부를 판단
        private bool CheckSuccessCombo()
        {
            long elapsedTime = GetElapsedTimeAfterLastUseSkill();
            if (elapsedTime - SkillData.AnimTime * GetCalculateAtkSpeedTick() < SkillData.ComboTime * 1000)
                return true;

            return false;
        }
        private void UpdateComboIdx()
        {
            if (_currentComboIdx >= SkillData.MaxComboIdx)
                _currentComboIdx = 0;
            else
                _currentComboIdx++;
        }
        //공격속도에 비례된 Tick값을 반환
        private long GetCalculateAtkSpeedTick()
        {
            return (long)(SkillData.AnimTime * (1.0f / Owner.StatComponent.StatInfo.AtkSpeed) * 1000);
        }
    }
}
