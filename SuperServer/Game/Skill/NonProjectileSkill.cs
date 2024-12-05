using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Utils;
using Google.Protobuf.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Skill
{
    public class NonProjectileSkill : BaseSkill
    {
        int _currentComboIdx = 0;
        Dictionary<int, string> _normalSkillComboName = new Dictionary<int, string>();
        public NonProjectileSkill(Creature owner, SkillData skillData, int skillId) : base(owner, skillData, skillId)
        {
            for (int i = 0; i < skillData.ComboNames.Count; i++)
            {
                _normalSkillComboName.Add(i, skillData.ComboNames[i]);
            }
        }

        protected override List<Creature> GetSkillTargets(int targetId)
        {
            return null;
        }

        public override void UseSkill(int targetId, float rotY)
        {
            if (Owner == null || Owner.Room == null)
                return;

            switch (SkillData.SkillAreaType)
            {
                case ESkillAreaType.None:
                    break;
                case ESkillAreaType.Sector:
                    UseSectorSkill(targetId, rotY);
                    break;
                case ESkillAreaType.Circle:
                    break;
            }
        }

        public override void UseSkill(int targetId)
        {
            if (Owner == null || Owner.Room == null)
                return;

            bool canUse = CheckCanUseSkill();
            if (canUse == false)
                return;

            Creature target = Owner.Room.FindCreatureById(targetId);
            if (target == null)
                return;

            EffectData effectData;
            if (DataManager.EffectDict.TryGetValue(SkillData.EffectId, out effectData) == false)
                return;

            GameCommander.Instance.PushAfter(GetSkillDelayTick(),
            () =>
            {
                target.EffectComponent.ApplyEffect(Owner, effectData);
            });

            Owner.SkillComponent.LastSkill = this;
            BroadcastSkill(targetId);
            Refresh();
            return;
        }

        private void UseSectorSkill(int targetId, float rotY)
        {
            Creature target = Owner.Room.FindCreatureById(targetId);
            Vector3 skillCastDir;

            if (target != null)
                skillCastDir = (target.Position - Owner.Position).Normalize();
            else
                skillCastDir = Utils.Utils.GetDirFromRotY(rotY);

            List<Creature> effectedCreatures = new List<Creature>();
            EffectData effectData;
            if (DataManager.EffectDict.TryGetValue(SkillData.EffectId, out effectData) == false)
                return;

            List<Monster> monsters = Owner.Room.FindMonsterInInterestRegion(Owner.Position);
            foreach (Monster monster in monsters)
            {
                //거리 Check
                float dist = Vector3.Distance(monster.Position, Owner.Position);
                if (dist > SkillData.SkillRange) continue;
                //Sector Check
                Vector3 ownerToMonsterDir = (monster.Position - Owner.Position).Normalize();
                float dotValue = Vector2.Dot(
                    new Vector2(ownerToMonsterDir.X, ownerToMonsterDir.Z),
                    new Vector2(skillCastDir.X, skillCastDir.Z));

                float skillSectorValue = MathF.Cos(SkillData.SectorAngle * Utils.Utils.DegreeToRadian);
                if (skillSectorValue <= dotValue)
                    effectedCreatures.Add(monster);
            }

            foreach (Creature creature in effectedCreatures)
            {
                GameCommander.Instance.PushAfter(GetSkillDelayTick(),
                () =>
                {
                    creature.EffectComponent.ApplyEffect(Owner, effectData);
                });
            }

            Owner.SkillComponent.LastSkill = this;
            BroadcastSkill(targetId);
            Refresh();
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
            //UpdateComboIdx();
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
