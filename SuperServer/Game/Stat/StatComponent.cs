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
            StatInfo.Defence = statData.Defence;
        }
    }
}
