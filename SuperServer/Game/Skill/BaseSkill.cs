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
        public long LastCoolTick { get; protected set; }
        public int SkillLevel { get; protected set; }

        public BaseSkill(Creature owner, SkillData skillData, int templateId, int skillLevel)
        {
            Owner = owner;
            SkillData = skillData;
            TemplateId = templateId;
            SkillLevel = skillLevel;    
        }

        protected BaseSkill(Creature owner, SkillData skillData, int skillId)
        {
            Owner = owner;
            SkillData = skillData;
            this.skillId = skillId;
        }

        //타겟을 구하고 타겟한테 Effect를 주고 브로드캐스트
        public abstract void UseSkill(int skillTargetId, PosInfo skillPivot);
        public abstract void UseSkill(PosInfo skillPivot);
        public abstract void UseSkill(int skillTargetId);

        protected void BroadcastSkill(int skillTargetId, bool sendPos = false)
        {
            Owner.BroadcastSkill(SkillData.TemplateId, skillTargetId, SkillData.AnimName, sendPos);
        }
        protected void BroadcastSkill( bool sendPos = false)
        {
            Owner.BroadcastSkill(SkillData.TemplateId, SkillData.AnimName, sendPos);
        }

        public Vector2 GetSkillCastDir(Creature target, Vector3 skillPivot)
        {
            Vector3 skillCastDir = (target.Position - skillPivot).Normalize();

            return new Vector2(skillCastDir.X, skillCastDir.Z);
        }

        public Vector2 GetSkillCastDir(float rotY)
        {
            Vector3 skillCastDir = Utils.Utils.GetDirFromRotY(rotY);

            return new Vector2(skillCastDir.X, skillCastDir.Z);
        }

        //타겟 스킬
        public List<Creature> GetSkillEffectedTargets(Creature target)
        {
            if (Owner == null)
                return null;

            if (Owner.Room == null)
                return null;

            List<Creature> effectedCreatures = new List<Creature>();


            switch (SkillData.SkillAreaType)
            {
                case ESkillAreaType.Single:
                    if (CheckSkillUsageType(target, SkillData.SkillUsageTargetType))
                        effectedCreatures.Add(target);
                    break;
                case ESkillAreaType.Area:
                    List<Creature> creatures = Owner.Room.FindCreatureInInterestRegion(Owner.Position);
                    foreach (Creature creature in creatures)
                    {
                        //피아식별 검사
                        if (CheckSkillUsageType(creature, SkillData.SkillUsageTargetType) == false) continue;
                        //거리 검사
                        float dist = Vector3.Distance(creature.Position, target.Position);
                        if (dist > SkillData.SkillRange) continue;
                            effectedCreatures.Add(creature);
                    }
                    break;
                default:
                    return effectedCreatures;
            }

            return effectedCreatures;
        }

        //논타겟 스킬
        public List<Creature> GetSkillEffectedTargets(Vector3 skillPos, Vector2 skillCastDir)
        {
            List<Creature> effectedCreatures = new List<Creature>();
            int maxEntityCount = SkillData.MaxEntityCount;
            int currentCount = 0;

            switch (SkillData.SkillAreaType)
            {
                case ESkillAreaType.Area:
                    List<Creature> creatures = Owner.Room.FindCreatureInInterestRegion(Owner.Position);
                    foreach (Creature creature in creatures)
                    {
                        //효과를 주는 최대 마릿 수 제한
                        if (maxEntityCount != 0 && currentCount >= maxEntityCount) break;
                        //피아식별 검사
                        if (CheckSkillUsageType(creature, SkillData.SkillUsageTargetType) == false) continue;
                        //거리 검사
                        float dist = Vector3.Distance(creature.Position, skillPos);
                        if (dist > SkillData.SkillRange) continue;
                        //Sector 검사
                        Vector3 dir = (creature.Position - skillPos).Normalize();
                        float dotValue = Vector2.Dot(
                            new Vector2(dir.X, dir.Z),
                            new Vector2(skillCastDir.X, skillCastDir.Y));

                        float skillSectorValue = MathF.Cos(SkillData.SectorAngle * Utils.Utils.DegreeToRadian);
                        if (skillSectorValue <= dotValue)
                            effectedCreatures.Add(creature);
                    }
                    break;
                default:
                    return effectedCreatures;
            }

            return effectedCreatures;
        }

        private bool CheckSkillUsageType(Creature target, ESkillUsageTargetType usageType)
        {
            switch (usageType)
            {
                case ESkillUsageTargetType.Self:
                    return target.ObjectId == Owner.ObjectId;
                    //나중에 파티시스템 만들면 수정
                case ESkillUsageTargetType.Ally:
                    return target.ObjectType == Owner.ObjectType;
                case ESkillUsageTargetType.Enemy:
                    return target.ObjectType != Owner.ObjectType;
                default:
                    return false;
            }
        }

        public void CalculateComboAndApply()
        {
            if (CheckSuccessCombo() == true)
            {
                if (DataManager.SkillDict.TryGetValue(SkillData.NextSkillTemplateId, out SkillData nextSkillData) == true)
                    SkillData = nextSkillData;
            }
            else
            {
                if (DataManager.SkillDict.TryGetValue(TemplateId, out SkillData originSkillData) == true)
                    SkillData = originSkillData;
            }
        }

        #region 스킬 검사 Or Tick계산
        //논타겟류 스킬 검사
        public bool CheckCanUseSkill()
        {
            if (Owner == null || Owner.Room == null)
                return false;

            if (CheckCoolTime() == false)
            {
                Console.WriteLine("CoolTime Fail");
                return false;
            }

            return true;
        }

        //타겟류 스킬 검사
        public bool CheckCanUseSkill(BaseObject target)
        {
            if (Owner == null || Owner.Room == null)
                return false;

            if (CheckTargetDie(target) == false)
            {
                Console.WriteLine("Target Die");
                return false;
            }

            if (CheckCoolTime() == false)
            {
                Console.WriteLine("CoolTime Fail");
                return false;
            }

            if (CheckRange(target) == false)
            {
                Console.WriteLine("Dist Fail");
                return false;
            }

            return true;
        }
        //스킬을 사용중인지 검사
        public bool CheckUsingSkill()
        {
            long elapsedTime = GetElapsedTimeAfterLastUseSkill();
            if (elapsedTime >= SkillData.AnimTime * 1000)
                return false;

            return true;
        }
        //타겟이 죽었는지 검사(타겟이 죽었으면 스킬을 못쓴다는 의미에서 false반환)
        protected bool CheckTargetDie(BaseObject target)
        {
            Creature t = target as Creature;
            if (t.CurrentState == ECreatureState.Die)
                return false;

            return true;
        }
        //쿨타임 검사
        protected bool CheckCoolTime()
        {
            long elapsedTime = GetElapsedTimeAfterLastUseSkill();
            if (elapsedTime >= SkillData.CoolTime * 1000)
                return true;

            return false;
        }
        //스킬 이펙트 타임
        protected int GetSkillEffectTick()
        {
            return (int)(SkillData.EffectDelayRatio * SkillData.AnimTime * 1000);
        }
        //스킬 범위 계산
        private bool CheckRange(BaseObject target)
        {
            if (target == null)
                return false;
            float dist = Vector3.Distance(Owner.Position, target.Position);

            if (dist > SkillData.SkillRange)
                return false;

            return true;
        }
        //가장최근에 해당 스킬을 쓰고 난 후 지난 시간
        protected long GetElapsedTimeAfterLastUseSkill()
        {
            return Environment.TickCount64 - LastCoolTick;
        }
       
        ////콤보 적용여부를 판단
        private bool CheckSuccessCombo()
        {
            long elapsedTime = GetElapsedTimeAfterLastUseSkill();
            if (elapsedTime - SkillData.AnimTime * 1000 < SkillData.ComboTime * 1000)
                return true;

            return false;
        }
        #endregion
        #region 스킬 사용 후 처리
        //스킬 사용후 Refresh할것들
        protected void Refresh()
        {
            RefreshCooldown();
        }
        //가장 마지막에 쓴 스킬 Tick을 갱신
        //현재 플레이어가 스킬을 사용중에 있는지 확인하기 위해 또는 콤보
        protected void RefreshCooldown()
        {
            LastCoolTick = Environment.TickCount64;
        }
        #endregion
    }
}
