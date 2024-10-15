using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Skill;
using SuperServer.Game.Skill.Effect;
using SuperServer.Game.Stat;
using Google.Protobuf.Enum;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Object
{
    public class Creature : BaseObject
    {
        public CreatureInfo CreatureInfo { get; private set; }
        public StatComponent StatComponent { get; private set; }
        public SkillComponent SkillComponent { get; private set; }
        public EffectComponent EffectComponent { get; private set; }
        public PoolData PoolData { get; protected set; }
        private ResUseSkillToC _skillPacket = new ResUseSkillToC();
        public Creature()
        {
            StatComponent = new StatComponent();
            SkillComponent = new SkillComponent(this);
            CreatureInfo = new CreatureInfo();
            EffectComponent = new EffectComponent(this);
            CreatureInfo.ObjectInfo = ObjectInfo;
            CreatureInfo.StatInfo = StatComponent.StatInfo;
        }

        public virtual void OnDamage(Creature attacker, float damage)
        {
            if (Room == null)
                return;

            int retDamage = Math.Min(1, (int)MathF.Round(damage) - StatComponent.StatInfo.Defence);
            StatComponent.AddStat(EStatType.Hp, -retDamage);
        }

        public void BroadcastSkill(int skillId, int targetId)
        {
            if (Room == null)
                return;

            _skillPacket.SkillId = skillId;
            _skillPacket.ObjectId = this.ObjectId;
            _skillPacket.TargetId = targetId;

            GameCommander.Instance.Push(() =>
            {
                Room?.Broadcast(_skillPacket, Position);
            });
        }
    }
}
