using Google.Protobuf.Enum;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Object;
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
        public Creature Owner { get; protected set; }
        public int TemplateId { get; protected set; }
        public SkillData SkillData { get; protected set; }
        public long LastCoolTick { get; protected set; }
        public string PlayAnimName { get; protected set; }
        public BaseSkill(Creature owner, SkillData skillData, int templateId)
        {
            Owner = owner;
            SkillData = skillData;
            TemplateId = templateId;
        }

        //스킬에 영향을 받을 모든 타겟들을 얻어오는 함수
        protected abstract List<Creature> GetSkillTargets(int targetId);

        //타겟을 구하고 타겟한테 Effect를 주고 브로드캐스트
        public abstract void UseSkill(int targetId, float rotY);
        public abstract void UseSkill(int targetId);

        protected void BroadcastSkill(int targetId)
        {
            Owner.BroadcastSkill(TemplateId, targetId, GetPlayAnimName());
        }

        public float GetSkillRange()
        {
            return SkillData.SkillRange;
        }

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
        public bool CheckUsingSkill()
        {
            long elapsedTime = GetElapsedTimeAfterLastUseSkill();
            if (elapsedTime >= SkillData.AnimTime * 1000)
                return false;

            return true;
        }

        protected virtual int GetSkillDelayTick()
        {
            return (int)(SkillData.EffectDelayRatio * 1 * 1000);
        }
        protected bool CheckTargetDie(BaseObject target)
        {
            Creature t = target as Creature;
            if (t.CurrentState == ECreatureState.Die)
                return false;

            return true;
        }
        protected virtual bool CheckCoolTime()
        {
            long elapsedTime = GetElapsedTimeAfterLastUseSkill();
            if (elapsedTime >= SkillData.CoolTime * 1000)
                return true;

            return false;
        }
        private bool CheckRange(BaseObject target)
        {
            if (target == null)
                return false;
            float dist = Vector3.Distance(Owner.Position, target.Position);

            if (dist > GetSkillRange())
                return false;

            return true;
        }
        //가장최근에 해당 스킬을 쓰고 난 후 지난 시간
        protected long GetElapsedTimeAfterLastUseSkill()
        {
            return Environment.TickCount64 - LastCoolTick;
        }
        
        protected virtual void Refresh()
        {
            RefreshCooldown();
        }
        //가장 마지막에 쓴 스킬 Tick을 갱신
        //현재 플레이어가 스킬을 사용중에 있는지 확인하기 위해
        protected void RefreshCooldown()
        {
            LastCoolTick = Environment.TickCount64;
        }

        protected virtual string GetPlayAnimName()
        {
            return SkillData.AnimName;
        }
    }
}
