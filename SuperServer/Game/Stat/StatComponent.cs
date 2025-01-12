using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json.Linq;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.DB;
using SuperServer.Game.Object;
using SuperServer.Game.Skill.Effect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Stat
{
    public class StatComponent
    {
        public StatInfo BaseStatInfo { get; set; } = new StatInfo();
        public StatInfo AddedStatInfo { get; set; } = new StatInfo();
        public Creature Owner { get; private set; }

        public static readonly Dictionary<EStatType, Action<StatInfo, float>> SetStatDict = new Dictionary<EStatType, Action<StatInfo, float>>()
        {
            {EStatType.Hp, (info, value) => { info.Hp = (int)value; } },
            {EStatType.MaxHp, (info, value) => { info.MaxHp =(int) value; } },
            {EStatType.Mp, (info, value) => { info.Mp =(int) value; } },
            {EStatType.MaxMp, (info, value) => { info.MaxMp =(int) value; } },
            {EStatType.Atk, (info, value) => { info.AtkDamage =(int) value; } },
            {EStatType.Defence, (info, value) => { info.Defence =(int) value; } },
            {EStatType.MoveSpeed, (info, value) => { info.MoveSpeed = value; } },
        };
        public static readonly Dictionary<EStatType, Func<StatInfo, float>> GetStatDict = new Dictionary<EStatType, Func<StatInfo, float>>()
        {
            {EStatType.Hp, (info) => { return info.Hp; } },
            {EStatType.MaxHp, (info) => { return info.MaxHp; } },
            {EStatType.Mp, (info) => { return info.Mp; } },
            {EStatType.MaxMp, (info) => { return info.MaxMp; } },
            {EStatType.Atk, (info) => { return info.AtkDamage; } },
            {EStatType.Defence, (info) => { return info.Defence; } },
            {EStatType.MoveSpeed, (info) => { return info.MoveSpeed; } },
        };

        public StatComponent(Creature owner)
        {
            Owner = owner;
        }

        public float GetStat(EStatType statType)
        {
            return GetStatDict[statType].Invoke(AddedStatInfo);
        }
        public void SetStat(EStatType statType, float value)
        {
            float preValue = 0;
            float gapValue = 0;
            EStatType addStatType = EStatType.None;

            switch (statType)
            {
                case EStatType.MaxHp:
                    preValue = GetStat(EStatType.MaxHp);
                    gapValue = value - preValue;
                    addStatType = EStatType.Hp;
                    break;
                case EStatType.MaxMp:
                    preValue = GetStat(EStatType.MaxMp);
                    gapValue = value - preValue;
                    addStatType = EStatType.Mp;
                    break;
                case EStatType.Hp:
                    value = Math.Clamp(value, 0, GetStat(EStatType.MaxHp));
                    break;
                case EStatType.Mp:
                    value = Math.Clamp(value, 0, GetStat(EStatType.MaxMp));
                    break;
            }

            SetStatDict[statType].Invoke(AddedStatInfo, value);
            if (gapValue != 0 && addStatType != EStatType.None)
                Owner.AddStat(addStatType, gapValue);
        }

        public void InitHeroStat(int level)
        {
            HeroStatData statData;
            if (DataManager.HeroStatDict.TryGetValue(level, out statData) == false)
                return;

            BaseStatInfo.MaxHp = statData.MaxHp;
            BaseStatInfo.MaxMp = statData.MaxMp;
            BaseStatInfo.Hp = statData.MaxHp;
            BaseStatInfo.Mp = statData.MaxMp;
            BaseStatInfo.AtkDamage = statData.AtkDamage;
            BaseStatInfo.MoveSpeed = statData.MoveSpeed;
            BaseStatInfo.Defence = statData.Defence;
        }
        public void InitSetStat(MonsterData statData)
        {
            AddedStatInfo.MaxHp = statData.MaxHp;
            AddedStatInfo.MaxMp = statData.MaxMp;
            AddedStatInfo.Hp = statData.MaxHp;
            AddedStatInfo.Mp = statData.MaxMp;
            AddedStatInfo.AtkDamage = statData.AtkDamage;
            AddedStatInfo.MoveSpeed = statData.MoveSpeed;
            AddedStatInfo.ChaseSpeed = statData.ChaseSpeed;
            AddedStatInfo.Defence = statData.Defence;
        }
    }
}
