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
        public NonProjectileSkill(Creature owner, SkillData skillData, int skillId) : base(owner, skillData, skillId)
        {
            
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

            Creature target = Owner.Room.FindCreatureById(targetId);
            if (target == null)
                return;

            EffectData effectData;
            if (DataManager.EffectDict.TryGetValue(SkillData.EffectId, out effectData) == false)
                return;

            GameCommander.Instance.PushAfter(GetSkillEffectTick(),
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
            //스킬 쓸 방향 계산
            Vector3 skillCastDir;
            if (target != null)
                skillCastDir = (target.Position - Owner.Position).Normalize();
            else
                skillCastDir = Utils.Utils.GetDirFromRotY(rotY);

            //스킬 Effect를 받을 몬스터 계산
            List<Creature> effectedCreatures = new List<Creature>();
            EffectData effectData;
            if (DataManager.EffectDict.TryGetValue(SkillData.EffectId, out effectData) == false)
                return;

            List<Monster> monsters = Owner.Room.FindMonsterInInterestRegion(Owner.Position);
            foreach (Monster monster in monsters)
            {
                //거리 검사
                float dist = Vector3.Distance(monster.Position, Owner.Position);
                if (dist > SkillData.SkillRange) continue;
                //Sector 검사
                Vector3 ownerToMonsterDir = (monster.Position - Owner.Position).Normalize();
                float dotValue = Vector2.Dot(
                    new Vector2(ownerToMonsterDir.X, ownerToMonsterDir.Z),
                    new Vector2(skillCastDir.X, skillCastDir.Z));

                float skillSectorValue = MathF.Cos(SkillData.SectorAngle * Utils.Utils.DegreeToRadian);
                if (skillSectorValue <= dotValue)
                    effectedCreatures.Add(monster);
            }

            //SkillEffect주기
            foreach (Creature creature in effectedCreatures)
            {
                GameCommander.Instance.PushAfter(GetSkillEffectTick(),
                () =>
                {
                    creature.EffectComponent.ApplyEffect(Owner, effectData);
                });
            }

            //브로드케스트 및 Refresh
            Owner.SkillComponent.LastSkill = this;
            BroadcastSkill(targetId);
            Refresh();
        }
    }
}
