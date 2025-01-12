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
using SuperServer.Game.StateMachine;
using System.Reflection.PortableExecutable;
using static System.Net.Mime.MediaTypeNames;

namespace SuperServer.Game.Object
{
    public class Creature : BaseObject
    {
        public CreatureInfo CreatureInfo { get; private set; }
        public StatComponent StatComponent { get; private set; }
        public SkillComponent SkillComponent { get; private set; }
        public EffectComponent EffectComponent { get; private set; }
        public ShieldComponent ShieldComponent { get; private set; }
        public AggroComponent AggroComponent { get; protected set; }

        public PoolData PoolData { get; protected set; }
        ResUseSkillToC _skillPacket = new ResUseSkillToC();
        ModifyStatToC _modifyStatPacket = new ModifyStatToC();
        ModifyOneStatToC _modifyOneStatPacket = new ModifyOneStatToC();
        DieToC _deadPacket = new DieToC();
        protected Random _rand = new Random();
        public Creature()
        {
            StatComponent = new StatComponent(this);
            SkillComponent = new SkillComponent(this);
            CreatureInfo = new CreatureInfo();
            EffectComponent = new EffectComponent(this);
            ShieldComponent = new ShieldComponent(this);
            CreatureInfo.ObjectInfo = ObjectInfo;
            CreatureInfo.StatInfo = StatComponent.AddedStatInfo;
        }

        public virtual void OnDamage(Creature attacker, float damage)
        {
            if (Room == null)
                return;

            if (CurrentState == ECreatureState.Die)
                return;

            int remainDamage = ShieldComponent.OnDamage((int)damage);
            int retDamage = GetCalculatedDamage(remainDamage);
            AddStat(EStatType.Hp, -retDamage, retDamage == 0 ? EFontType.Miss : EFontType.NormalHit);

            if (StatComponent.AddedStatInfo.Hp <= 0)
            {
                OnDie(attacker);
            }
        }

        protected int GetCalculatedDamage(float damage)
        {
            return Math.Max(0, (int)MathF.Round(damage) - StatComponent.AddedStatInfo.Defence);
        }

        public virtual void OnDie(Creature killer)
        {
            if (Room == null)
                return;

            if (CurrentState == ECreatureState.Die)
                return;

            CurrentState = ECreatureState.Die;
            _deadPacket.ObjectId = ObjectId;
            _deadPacket.KillerId = killer.ObjectId;

            Room.Broadcast(_deadPacket, Position);
        }

        public virtual void ReSpawn()
        {
            StatComponent.SetStat(EStatType.Hp, StatComponent.GetStat(EStatType.MaxHp));
        }


        public void AddStat(EStatType statType, float gapValue, EFontType fontType = EFontType.None, bool sendPacket = true)
        {
            //값의 변경이 일어나고
            float changeValue = StatComponent.GetStat(statType) + gapValue;
            StatComponent.SetStat(statType, changeValue);
            if (sendPacket == true)
                BroadcastOneStat(statType, StatComponent.GetStat(statType), gapValue, fontType);
        }

        public void BroadcastSkill(int skillId, int skillTargetId, bool sendPos = false)
        {
            if (Room == null)
                return;

            if (sendPos)
            {
                _skillPacket.PosInfo = this.PosInfo;
            }
            _skillPacket.ObjectId = this.ObjectId;
            _skillPacket.SkillInfo = new SkillInfo()
            {
                SkillId = skillId,
                SkillTargetId = skillTargetId,
            };

            Room.Broadcast(_skillPacket, Position);
        }

        public void BroadcastStat()
        {
            if (Room == null)
                return;

            _modifyStatPacket.ObjectId = ObjectId;
            _modifyStatPacket.StatInfo = StatComponent.AddedStatInfo;

            Room.Broadcast(_modifyStatPacket, Position);
        }
        public void BroadcastOneStat(EStatType statType, float changedValue, float gapValue, EFontType fontType)
        {
            if (Room == null)
                return;

            _modifyOneStatPacket.ObjectId = ObjectId;
            _modifyOneStatPacket.StatType = statType;
            _modifyOneStatPacket.ChangedValue = changedValue;
            _modifyOneStatPacket.GapValue = gapValue;
            _modifyOneStatPacket.FontType = fontType;

            Room.Broadcast(_modifyOneStatPacket, Position);
        }
    }
}
