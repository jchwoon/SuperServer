using Google.Protobuf.Enum;
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


            Console.WriteLine("UseSkill");
            Owner.SkillComponent.LastSkill = this;
            target.EffectComponent.ApplyEffect(Owner, effectData);
            RefreshCooldown();
            BroadcastSkill(targetId);
            return true;
        }

        private void BroadcastSkill(int targetId)
        {
            Owner.BroadcastSkill(SkillId, targetId);
        }

        public float GetSkillRange()
        {
            return SkillData.SkillRange;
        }

        private bool CheckCanUseSkill(BaseObject target)
        {
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

        protected virtual bool CheckCoolTime()
        {
            long elapsedTime = GetElapsedTimeAfterLastUseSkill();
            if (elapsedTime >= SkillData.CoolTime * 1000)
                return true;

            return false;
        }
        public bool CheckUsingSkill()
        {
            long elapsedTime = GetElapsedTimeAfterLastUseSkill();
            if (elapsedTime >= SkillData.AnimTime * 1000)
                return false;

            return true;
        }

        //가장최근에 해당 스킬을 쓰고 난 후 지난 시간
        protected long GetElapsedTimeAfterLastUseSkill()
        {
            return Environment.TickCount64 - LastCoolTick;
        }
        private void RefreshCooldown()
        {
            LastCoolTick = Environment.TickCount64;
        }
        private bool CheckRange(BaseObject target)
        {
            if (target == null)
                return false;

            float dist = Vector3.Distance(Owner.Position, target.Position);
            if (dist > GetSkillRange() + 2)
                return false;

            return true;
        }
    }
}
