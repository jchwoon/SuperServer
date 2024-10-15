using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using SuperServer.Data;
using SuperServer.DB;
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
        public static readonly Dictionary<EStatType, Action<StatInfo, float>> SetStat = new Dictionary<EStatType, Action<StatInfo, float>>()
        {
            {EStatType.Hp, (info, value) => { info.Hp = (int)value; } },
            {EStatType.MaxHp, (info, value) => { info.MaxHp =(int) value; } },
            {EStatType.Mp, (info, value) => { info.Mp =(int) value; } },
            {EStatType.MaxMp, (info, value) => { info.MaxMp =(int) value; } },
            {EStatType.Atk, (info, value) => { info.AtkDamage =(int) value; } },
            {EStatType.Defence, (info, value) => { info.Defence =(int) value; } },
            {EStatType.AtkSpeed, (info, value) => { info.AtkSpeed = value; } },
            {EStatType.MoveSpeed, (info, value) => { info.MoveSpeed = value; } },
        };
        public static readonly Dictionary<EStatType, Func<StatInfo, float>> GetStat = new Dictionary<EStatType, Func<StatInfo, float>>()
        {
            {EStatType.Hp, (info) => { return info.Hp; } },
            {EStatType.MaxHp, (info) => { return info.MaxHp; } },
            {EStatType.Mp, (info) => { return info.Mp; } },
            {EStatType.MaxMp, (info) => { return info.MaxMp; } },
            {EStatType.Atk, (info) => { return info.AtkDamage; } },
            {EStatType.Defence, (info) => { return info.Defence; } },
            {EStatType.AtkSpeed, (info) => { return info.AtkSpeed; } },
            {EStatType.MoveSpeed, (info) => { return info.MoveSpeed; } },
        };

        //히어로 레벨별 스텟 적용
        public void SetHeroStat(int level)
        {
            HeroStatData heroStatData;
            if (DataManager.HeroStatDict.TryGetValue(level, out heroStatData) == false)
                return;

            StatInfo.MaxHp = heroStatData.MaxHp;
            StatInfo.MaxMp = heroStatData.MaxMp;
            StatInfo.Hp = heroStatData.MaxHp;
            StatInfo.Mp = heroStatData.MaxMp;
            StatInfo.AtkDamage = heroStatData.AtkDamage;
            StatInfo.AtkSpeed = heroStatData.AtkSpeed;
            StatInfo.MoveSpeed = heroStatData.MoveSpeed;
            StatInfo.Defence = heroStatData.Defence;
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

        public void AddStat(EStatType statType, float value)
        {
            float changeValue = GetStat[statType].Invoke(StatInfo) + value;
            SetStat[statType].Invoke(StatInfo, changeValue);
        }
    }
}
