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
using SuperServer.Job;

namespace SuperServer.Game.Skill
{
    public class NonProjectileSkill : BaseSkill
    {
        public NonProjectileSkill(Creature owner, SkillData skillData, int skillId) : base(owner, skillData, skillId)
        {
            
        }

        //public override void UseSkill(int lookTargetId)
        //{
        //    if (Owner == null || Owner.Room == null)
        //        return;

        //    Creature target = Owner.Room.FindCreatureById(lookTargetId);
        //    if (target == null)
        //        return;

        //    EffectData effectData;
        //    if (DataManager.EffectDict.TryGetValue(SkillData.EffectId, out effectData) == false)
        //        return;

        //    GameCommander.Instance.PushAfter(GetSkillEffectTick(),
        //    () =>
        //    {
        //        target.EffectComponent.ApplyEffect(Owner, effectData);
        //    });

        //    Owner.SkillComponent.LastSkill = this;
        //    BroadcastSkill(lookTargetId);
        //    Refresh();
        //    return;
        //}

        public override void UseSkill(int skillTargetId, int locationTargetId, float? rotY)
        {
            if (Owner == null || Owner.Room == null)
                return;

            //스킬이 발생되는 타겟 위치를 얻기위해
            Creature locationTarget = Owner.Room.FindCreatureById(locationTargetId);
            if (locationTarget == null)
                return;
            //어떤 타겟을 향해 스킬을 쓸지 판단하기 위해
            Creature skillTarget = Owner.Room.FindCreatureById(skillTargetId);
            if (skillTarget == null)
                return;
            Vector2 skillCastDir = GetSkillCastDir(skillTarget, rotY);

            DataManager.EffectDict.TryGetValue(SkillData.EffectId, out EffectData effectData);

            if (effectData != null)
            {
                IJob job = GameCommander.Instance.PushAfter(GetSkillEffectTick(),
                () =>
                {
                    List<Creature> effectedCreatures = GetSkillEffectedTargets(locationTarget, skillCastDir);
                    foreach (Creature creature in effectedCreatures)
                    {
                        if (creature == null)
                            return;
                        creature.EffectComponent.ApplyEffect(Owner, effectData);
                    }
                });
                Owner.SkillComponent.CurrentRegisterJob = job;
            }

            if (SkillData.IsMoveSkill)
            {
                MoveFromSkill(new Vector3(skillCastDir.X, 0, skillCastDir.Y));
                BroadcastSkill(skillTargetId, locationTargetId, sendPos:true);
            }
            else
            {
                BroadcastSkill(skillTargetId, locationTargetId);
            }
            Owner.SkillComponent.LastSkill = this;
            Refresh();
        }

        private void MoveFromSkill(Vector3 dir, int depth = 20)
        {
            float dist = SkillData.Dist;
            float checkUnit = 0.5f;
            Vector3 currentPos = Owner.Position;
            Vector3 destPos = currentPos + (dir * dist);
            float startToDest = (currentPos - destPos).MagnitudeSqr();
            int count = 0;

            while (count < depth)
            {
                Vector3 nextPos = currentPos + (dir * checkUnit);
                if (!Owner.Room.Map.CanGo(nextPos.Z, nextPos.X))
                    break;

                currentPos = nextPos;

                float startToCurrent = (currentPos - Owner.Position).MagnitudeSqr();
                if (startToCurrent >= startToDest)
                {
                    currentPos = destPos;
                    break;
                }

                count++;
            }

            Console.WriteLine(count);

            Owner.PosInfo.PosX = currentPos.X;
            Owner.PosInfo.PosY = currentPos.Y;
            Owner.PosInfo.PosZ = currentPos.Z;
        }
    }
}
