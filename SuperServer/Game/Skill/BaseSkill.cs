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
    public class BaseSkill
    {
        public Creature Owner { get; protected set; }
        public int SkillId { get; protected set; }
        public SkillData SkillData { get; protected set; }
        public long LastCoolTick { get; protected set; }
        public BaseSkill(Creature owner, SkillData skillData, int skillId)
        {
            Owner = owner;
            SkillData = skillData;
            SkillId = skillId;
        }

        public bool UseSkill(int targetId)
        {
            if (Owner == null || Owner.Room == null)
                return false;

            Creature target = Owner.Room.FindCreatureById(targetId);
            if (target == null) 
                return false;

            bool canUse = CheckCanUseSkill(target);
            if (canUse == false)
                return false;

            EffectData effectData;
            if (DataManager.EffectDict.TryGetValue(SkillData.EffectId, out effectData) == false)
                return false;

            Owner.SkillComponent.LastSkill = this;
            GameCommander.Instance.PushAfter(GetSkillDelayTick(),
                () =>
                {
                    target.EffectComponent.ApplyEffect(Owner, effectData);
                });
            RefreshCooldown();
            BroadcastSkill(targetId);
            return true;
        }

        protected void BroadcastSkill(int targetId)
        {
            Owner.BroadcastSkill(SkillId, targetId);
        }

        public float GetSkillRange()
        {
            return SkillData.SkillRange;
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

        protected bool CheckTargetDie(BaseObject target)
        {
            Creature t = target as Creature;
            if (t.CurrentState == ECreatureState.Die)
                return false;

            return true;
        }

        protected virtual int GetSkillDelayTick()
        {
            return (int)(SkillData.EffectDelayRatio * 1 * 1000);
        }
        protected virtual bool CheckCoolTime()
        {
            long elapsedTime = GetElapsedTimeAfterLastUseSkill();
            if (elapsedTime >= SkillData.CoolTime * 1000)
                return true;

            return false;
        }
        //가장최근에 해당 스킬을 쓰고 난 후 지난 시간
        protected long GetElapsedTimeAfterLastUseSkill()
        {
            return Environment.TickCount64 - LastCoolTick;
        }
        protected void RefreshCooldown()
        {
            LastCoolTick = Environment.TickCount64;
        }
        private bool CheckRange(BaseObject target)
        {
            if (target == null)
                return false;

            float dist = Vector3.Distance(Owner.Position, target.Position);
            //검사할때만 스킬 범위 보정
            if (dist > GetSkillRange() +1)
                return false;

            return true;
        }
    }
}
