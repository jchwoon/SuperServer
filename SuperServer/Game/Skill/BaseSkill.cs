using Google.Protobuf.Enum;
using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Utils;
using System;
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

        public void UseSkill(int targetId)
        {
            if (Owner == null || Owner.Room == null)
                return;

            Creature target = Owner.Room.FindCreatureById(targetId);
            if (target == null) 
                return;

            bool canUse = CheckCanUseSkill(target);
            if (canUse == false)
                return;
            Console.WriteLine("UseSkill");
            RefreshCooldown();
            BroadcastSkill(targetId);
        }

        private void BroadcastSkill(int targetId)
        {
            Owner.BroadcastSkill(SkillId, targetId);
        }

        private bool CheckCanUseSkill(Creature target)
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

        protected long GetElapsedTimeAfterLastUseSkill()
        {
            return Environment.TickCount64 - LastCoolTick;
        }
        private void RefreshCooldown()
        {
            LastCoolTick = Environment.TickCount64;
        }
        private bool CheckRange(Creature target)
        {
            if (target == null)
                return false;

            float dist = Vector3.Distance(Owner.Position, target.Position);
            if (dist > GetSkillRange() + 2)
                return false;

            return true;
        }
        private float GetSkillRange()
        {
            return SkillData.SkillRange;
        }
    }
}
