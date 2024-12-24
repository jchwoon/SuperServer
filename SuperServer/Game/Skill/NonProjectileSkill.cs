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

        //NonTarget UseSkill
        public override void UseSkill(float rotY)
        {
            if (!IsValidOwnerState())
                return;

            Vector2 skillCastDir = GetSkillCastDir(rotY);

            DataManager.EffectDict.TryGetValue(SkillData.EffectId, out EffectData effectData);

            if (effectData != null)
            {
                //Temp Owner 나중에 스킬 위치를 받아와야함
                //ApplySkillEffect(effectData, Owner, skillCastDir);
            }

            if (SkillData.IsMoveSkill)
            {
                MoveFromSkill(new Vector3(skillCastDir.X, 0, skillCastDir.Y));
                BroadcastSkill(sendPos:true);
            }
            else
            {
                BroadcastSkill();
            }
            Owner.SkillComponent.LastSkill = this;
            Refresh();
        }

        //SmartTarget
        public override void UseSkill(int skillTargetId, float rotY)
        {
            if (!IsValidOwnerState())
                return;

            Creature skillTarget = Owner.Room.FindCreatureById(skillTargetId);
            Vector2 skillCastDir = (skillTarget != null) ? GetSkillCastDir(skillTarget) : GetSkillCastDir(rotY);
            

            DataManager.EffectDict.TryGetValue(SkillData.EffectId, out EffectData effectData);

            if (effectData != null)
            {
                //Temp Owner 나중에 스킬 위치를 받아와야함
                ApplySkillEffect(effectData, Owner, skillCastDir);
            }

            if (SkillData.IsMoveSkill)
            {
                MoveFromSkill(new Vector3(skillCastDir.X, 0, skillCastDir.Y));
                BroadcastSkill(skillTargetId, sendPos: true);
            }
            else
            {
                BroadcastSkill(skillTargetId);
            }
            Owner.SkillComponent.LastSkill = this;
            Refresh();
        }

        //Targeting
        public override void UseSkill(int skillTargetId)
        {
            if (!IsValidOwnerState())
                return;

            Creature skillTarget = Owner.Room.FindCreatureById(skillTargetId);
            if (skillTarget == null)
                return;
            Vector2 skillCastDir = GetSkillCastDir(skillTarget);


            DataManager.EffectDict.TryGetValue(SkillData.EffectId, out EffectData effectData);

            if (effectData != null)
            {
                ApplySkillEffect(effectData, skillTarget, skillCastDir);
            }

            if (SkillData.IsMoveSkill)
            {
                MoveFromSkill(new Vector3(skillCastDir.X, 0, skillCastDir.Y));
                BroadcastSkill(skillTargetId, sendPos: true);
            }
            else
            {
                BroadcastSkill(skillTargetId);
            }
            Owner.SkillComponent.LastSkill = this;
            Refresh();
        }

        //논타겟
        private void ApplySkillEffect(EffectData effectData, Vector3 skillPos, Vector2 castDir)
        {
            IJob job = GameCommander.Instance.PushAfter(GetSkillEffectTick(),
            () =>
            {
                if (!IsValidOwnerState()) return;

                List<Creature> effectedCreatures = GetSkillEffectedTargets(skillPos, castDir);
                foreach (Creature creature in effectedCreatures)
                {
                    if (creature == null) continue;
                    creature.EffectComponent.ApplyEffect(Owner, effectData);
                }
            });
            Owner.SkillComponent.CurrentRegisterJob = job;
        }

        //타겟
        private void ApplySkillEffect(EffectData effectData, Creature target, Vector2 castDir)
        {
            IJob job = GameCommander.Instance.PushAfter(GetSkillEffectTick(),
            () =>
            {
                if (!IsValidOwnerState()) return;

                List<Creature> effectedCreatures = GetSkillEffectedTargets(target, castDir);
                foreach (Creature creature in effectedCreatures)
                {
                    if (creature == null) continue;
                    creature.EffectComponent.ApplyEffect(Owner, effectData);
                }
            });
            Owner.SkillComponent.CurrentRegisterJob = job;
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

            Owner.PosInfo.PosX = currentPos.X;
            Owner.PosInfo.PosY = currentPos.Y;
            Owner.PosInfo.PosZ = currentPos.Z;
        }

        private bool IsValidOwnerState()
        {
            return Owner != null && Owner.Room != null && Owner.CurrentState != ECreatureState.Die;
        }

    }
}
