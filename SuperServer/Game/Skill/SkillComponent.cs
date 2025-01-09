using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Job;
using SuperServer.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Skill
{
    public class SkillComponent
    {
        Dictionary<int, BaseSkill> _skills = new Dictionary<int, BaseSkill>();
        bool _canUpdateSkillLevel = true;
        public Creature Owner { get; private set; }
        public int NormalSkillId { get; private set; }
        public BaseSkill LastSkill { get; set; }
        public IJob CurrentRegisterJob { get; set; }
        public int ActiveSkillPoint { get; private set; }
        public int PassiveSkillPoint { get; private set; }

        public SkillComponent(Creature owner)
        {
            Owner = owner;
        }

        public void RegisterSkill(Dictionary<int, int> skills)
        {
            foreach (KeyValuePair<int, int> s in skills)
            {
                if (DataManager.SkillDict.TryGetValue(s.Key, out SkillData skillData) == true)
                {
                    if (_skills.ContainsKey(s.Key))
                        continue;

                    BaseSkill skill = null;
                    switch (skillData.SkillProjectileType)
                    {
                        case ESkillProjectileType.None:
                            skill = new NonProjectileSkill(Owner, skillData, s.Key, s.Value);
                            break;
                    }
                    if (skill.SkillData.SkillSlotType == ESkillSlotType.Normal)
                        NormalSkillId = skill.TemplateId;
                    if (skill != null)
                        _skills.Add(s.Key, skill);
                }
            }
        }

        public void InitSkillPoint(int activePoint, int passivePoint)
        {
            ActiveSkillPoint = activePoint;
            PassiveSkillPoint = passivePoint;
        }

        public void UseSKill(SkillInfo skillInfo, PosInfo skillPivot)
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

            switch (skill.SkillData.SkillTargetingType)
            {
                case ESkillTargetingType.Target:
                    skill.UseSkill(skillInfo.SkillTargetId);
                    break;
                case ESkillTargetingType.NonTarget:
                    skill.UseSkill(skillPivot);
                    break;
                case ESkillTargetingType.SmartTarget:
                    skill.UseSkill(skillInfo.SkillTargetId, skillPivot);
                    break;
            }
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

        #region Skill Level & Point
        //Player 레벨 업
        public void LevelUp(int preveLevel, int currentLevel)
        {
            Hero hero = Owner as Hero;
            if (hero == null)
                return;

            int increasePoint = currentLevel - preveLevel;
            ESkillType active = ESkillType.Active;
            ESkillType passive = ESkillType.Passive;

            SetSkillPoint(active, ActiveSkillPoint + increasePoint);
            SetSkillPoint(passive, PassiveSkillPoint + increasePoint);

            DBCommander.Instance.Push(DBLogic.Instance.SaveSkillPoint, hero, active, GetSkillPointBySkillType(active));
            DBCommander.Instance.Push(DBLogic.Instance.SaveSkillPoint, hero, passive, GetSkillPointBySkillType(passive));
        }
        //스킬 레벨 업
        public void LevelUpSkill(int templateId, int usePoint = 1)
        {
            BaseSkill skill = GetSkillById(templateId);
            if (skill == null)
                return;

            ESkillType skillType = skill.GetSkillType();
            if (skillType == ESkillType.None)
                return;

            Hero hero = Owner as Hero;
            if (hero == null)
                return;

            //먼저 DB에 반영이 되었는지 체크 후
            if (_canUpdateSkillLevel == false)
                return;
            //해당 스킬이 만렙인지 확인
            if (skill.CheckCanLevelUp(usePoint) == false)
                return;
            //스킬 포인트가 유효한지
            if (GetSkillPointBySkillType(skillType) < usePoint)
                return;

            //스킬 포인트 차감
            SetSkillPoint(skillType, GetSkillPointBySkillType(skillType) - usePoint);
            //스킬 레벨 증가
            int updateLevel = skill.CurrentSkillLevel + usePoint;
            skill.UpdateSkillLevel(updateLevel);

            //Send Packet
            List<SkillLevelInfo> levelInfos = new List<SkillLevelInfo>();
            SkillLevelInfo levelInfo = new SkillLevelInfo()
            {
                SkillId = templateId,
                SkillLevel = updateLevel,
            };
            levelInfos.Add(levelInfo);
            SendUpdateLevelPacket(levelInfos);

            //DB 갱신
            DBCommander.Instance.Push(DBLogic.Instance.SaveSkillList, hero);
            DBCommander.Instance.Push(DBLogic.Instance.SaveSkillPoint, hero, skillType, GetSkillPointBySkillType(skillType));
        }

        //플레이어가 갖고 있는 모든 스킬들을 초기화
        public void CheckAndResetSkillPoint(ESkillType skillType)
        {
            Hero hero = Owner as Hero;
            if (hero == null)
                return;

            if (DataManager.CostDict.TryGetValue(hero.HeroData.SkillInitId, out CostData costData) == false)
                return;

            if (hero.CurrencyComponent.CanPay(costData.CurrencyType, costData.CostValue) == false)
                return;

            int level = hero.GrowthComponent.Level;
            //스킬 포인트 리셋
            SetSkillPoint(skillType, level - 1);
            //Send Packet
            List<SkillLevelInfo> levelInfos = new List<SkillLevelInfo>();
            foreach (BaseSkill skill in _skills.Values)
            {
                if (skill.SkillData.SkillType == skillType)
                {
                    skill.UpdateSkillLevel(skillType == ESkillType.Active ? 1 : 0);

                    levelInfos.Add(new SkillLevelInfo() { SkillId = skill.TemplateId, SkillLevel = skill.CurrentSkillLevel });
                }
            }
            SendUpdateLevelPacket(levelInfos, cost : costData.CostValue);

            //DB 갱신
            DBCommander.Instance.Push(DBLogic.Instance.SaveSkillList, hero);
            DBCommander.Instance.Push(DBLogic.Instance.SaveSkillPoint, hero, skillType, GetSkillPointBySkillType(skillType));
        }
        #endregion

        #region Get/Set
        public BaseSkill GetSkillById(int templateId)
        {
            BaseSkill skill;
            if (_skills.TryGetValue(templateId, out skill) == false)
                return null;

            return skill;
        }

        public int GetSkillPointBySkillType(ESkillType skillType)
        {
            return skillType == ESkillType.Active ? ActiveSkillPoint : PassiveSkillPoint;
        }

        public Dictionary<int, int> GetAllSkillLevels()
        {
            Dictionary<int, int> allSkillLevel = new Dictionary<int, int>();
            foreach (KeyValuePair<int, BaseSkill> kvp in _skills)
            {
                allSkillLevel.Add(kvp.Key, kvp.Value.CurrentSkillLevel);
            }
            return allSkillLevel;
        }

        public void SetSkillPoint(ESkillType skillType, int point)
        {
            if (skillType == ESkillType.Active)
                ActiveSkillPoint = point;
            if (skillType == ESkillType.Passive)
                PassiveSkillPoint = point;
        }
        #endregion

        #region Send
        private void SendUpdateLevelPacket(List<SkillLevelInfo> levelInfos, int cost = 0)
        {
            Hero hero = Owner as Hero;
            if (hero == null)
                return;

            UpdateSkillLevelToC levelUpPacket = new UpdateSkillLevelToC();
            levelUpPacket.SkillLevelInfos.AddRange(levelInfos);
            levelUpPacket.ActiveSkillPoint = ActiveSkillPoint;
            levelUpPacket.PassiveSkillPoint = PassiveSkillPoint;
            levelUpPacket.Cost = cost;
            hero?.Session.Send(levelUpPacket);
        }
        #endregion
    }
}
