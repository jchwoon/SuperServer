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
using Google.Protobuf.Struct;
using SuperServer.Game.Skill.Effect;

namespace SuperServer.Game.Skill
{
    public class NonProjectileSkill : BaseSkill
    {
        public NonProjectileSkill(Creature owner, SkillData skillData, int skillId, int skillLevel) : base(owner, skillData, skillId, skillLevel)
        {
            
        }

        //NonTarget HandleUseSkill
        public override void UseSkill(PosInfo skillPivot)
        {
            if (!IsValidOwnerState())
                return;

            Vector2 skillCastDir = GetSkillCastDir(skillPivot.RotY);

            foreach (int id in SkillData.EffectIds)
            {
                DataManager.EffectDict.TryGetValue(id, out EffectData effectData);
                if (effectData != null)
                {
                    ApplySkillEffect(effectData,
                        new Vector3(skillPivot.PosX, skillPivot.PosY, skillPivot.PosZ),
                        skillCastDir);
                }
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
        public override void UseSkill(int skillTargetId, PosInfo skillPivot)
        {
            if (!IsValidOwnerState())
                return;

            Vector3 skillPos = new Vector3(skillPivot.PosX, skillPivot.PosY, skillPivot.PosZ);

            Creature skillTarget = Owner.Room.FindCreatureById(skillTargetId);
            Vector2 skillCastDir = (skillTarget != null) ? GetSkillCastDir(skillTarget, skillPos) : GetSkillCastDir(skillPivot.RotY);


            foreach (int id in SkillData.EffectIds)
            {
                DataManager.EffectDict.TryGetValue(id, out EffectData effectData);
                if (effectData != null)
                {
                    ApplySkillEffect(effectData,
                        skillPos,
                        skillCastDir);
                }
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

            foreach (int id in SkillData.EffectIds)
            {
                DataManager.EffectDict.TryGetValue(id, out EffectData effectData);
                if (effectData != null)
                {
                    ApplySkillEffect(effectData, skillTarget);
                }
            }

            //시전자와 타겟의 방향
            if (SkillData.IsMoveSkill)
            {
                MoveFromSkill((skillTarget.Position - Owner.Position).Normalize());
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
                EffectDataEx effectEx = new EffectDataEx()
                {
                    effectData = effectData,
                    level = CurrentSkillLevel - 1,
                    entityCount = effectedCreatures.Count
                };
                if (effectData.FeedbackEffect)
                {
                    Owner.EffectComponent.ApplyEffect(Owner, effectEx);
                }
                else
                {
                    foreach (Creature creature in effectedCreatures)
                    {
                        if (creature == null) continue;
                        creature.EffectComponent.ApplyEffect(Owner, effectEx);
                    }
                }
            });
            Owner.SkillComponent.CurrentRegisterJob = job;
        }



        //타겟
        private void ApplySkillEffect(EffectData effectData, Creature target)
        {
            IJob job = GameCommander.Instance.PushAfter(GetSkillEffectTick(),
            () =>
            {
                if (!IsValidOwnerState()) return;

                List<Creature> effectedCreatures = GetSkillEffectedTargets(target);
                foreach (Creature creature in effectedCreatures)
                {
                    if (creature == null) continue;
                    EffectDataEx effectEx = new EffectDataEx() { effectData = effectData, level = CurrentSkillLevel-1 };
                    creature.EffectComponent.ApplyEffect(Owner, effectEx);
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
