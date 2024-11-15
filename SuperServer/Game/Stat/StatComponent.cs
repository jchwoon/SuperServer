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
        public StatInfo StatInfo { get; set; } = new StatInfo();
        public Creature Owner { get; private set; }

        public static readonly Dictionary<EStatType, Action<StatInfo, float>> SetStatDict = new Dictionary<EStatType, Action<StatInfo, float>>()
        {
            {EStatType.Hp, (info, value) => { info.Hp = (int)value; } },
            {EStatType.MaxHp, (info, value) => { info.MaxHp =(int) value; } },
            {EStatType.Mp, (info, value) => { info.Mp =(int) value; } },
            {EStatType.MaxMp, (info, value) => { info.MaxMp =(int) value; } },
            {EStatType.Atk, (info, value) => { info.AtkDamage =(int) value; } },
            {EStatType.Defence, (info, value) => { info.Defence =(int) value; } },
            {EStatType.AtkSpeed, (info, value) => { info.AtkSpeed = value; } },
            {EStatType.AddAtkSpeedMultiplier, (info, value) => { info.AddAtkSpeedMultiplier = (int)value; } },
            {EStatType.BaseAtkSpeed, (info, value) => { info.BaseAtkSpeed = value; } },
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
            {EStatType.AtkSpeed, (info) => { return info.AtkSpeed; } },
            {EStatType.AddAtkSpeedMultiplier, (info) => { return info.AddAtkSpeedMultiplier; } },
            {EStatType.BaseAtkSpeed, (info) => { return info.BaseAtkSpeed; } },
            {EStatType.MoveSpeed, (info) => { return info.MoveSpeed; } },
        };

        public StatComponent(Creature owner)
        {
            Owner = owner;
        }

        public float GetStat(EStatType statType)
        {
            return GetStatDict[statType].Invoke(StatInfo);
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
                case EStatType.AddAtkSpeedMultiplier:
                    UpdateAtkSpeed((int)value);
                    break;
            }

            SetStatDict[statType].Invoke(StatInfo, value);
            if (gapValue != 0 && addStatType != EStatType.None)
                Owner.AddStat(addStatType, gapValue);
        }

        private void UpdateAtkSpeed(int addSpeedMultiplier)
        {
            float value = GetStat(EStatType.BaseAtkSpeed) + (addSpeedMultiplier * 0.01f);
            SetStatDict[EStatType.AtkSpeed].Invoke(StatInfo, value);
        }

        public void InitHeroStat(int level)
        {
            HeroStatData statData;
            if (DataManager.HeroStatDict.TryGetValue(level, out statData) == false)
                return;

            StatInfo.MaxHp = statData.MaxHp;
            StatInfo.MaxMp = statData.MaxMp;
            StatInfo.Hp = statData.MaxHp;
            StatInfo.Mp = statData.MaxMp;
            StatInfo.AtkDamage = statData.AtkDamage;
            StatInfo.AtkSpeed = statData.BaseAtkSpeed + (statData.AddAtkSpeedMultiplier * 0.01f);
            StatInfo.MoveSpeed = statData.MoveSpeed;
            StatInfo.Defence = statData.Defence;
            StatInfo.AddAtkSpeedMultiplier = statData.AddAtkSpeedMultiplier;
            StatInfo.BaseAtkSpeed = statData.BaseAtkSpeed;
        }
        public void InitSetStat(MonsterData statData)
        {
            StatInfo.MaxHp = statData.MaxHp;
            StatInfo.MaxMp = statData.MaxMp;
            StatInfo.Hp = statData.MaxHp;
            StatInfo.Mp = statData.MaxMp;
            StatInfo.AtkDamage = statData.AtkDamage;
            StatInfo.AtkSpeed = statData.AtkSpeed;
            StatInfo.MoveSpeed = statData.MoveSpeed;
            StatInfo.ChaseSpeed = statData.ChaseSpeed;
            StatInfo.Defence = statData.Defence;
        }
    }
}
