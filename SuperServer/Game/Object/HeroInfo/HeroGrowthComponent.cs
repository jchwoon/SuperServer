using Google.Protobuf.WellKnownTypes;
using SuperServer.Data;
using SuperServer.Game.Stat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HeroGrowthComponent
{
    public int Level 
    { 
        get { return Owner.HeroInfo.LobbyHeroInfo.Level; } 
        set { Owner.HeroInfo.LobbyHeroInfo.Level = value; }
    }
    public int Exp
    {
        get { return Owner.MyHeroInfo.Exp; }
        set { Owner.MyHeroInfo.Exp = value; }
    }
    public Hero Owner { get; private set; }

    public HeroGrowthComponent(Hero owner)
    {
        Owner = owner;
    }

    public void AddExp(int exp)
    {
        if (IsMaxLevel())
            return;

        Exp += exp;

        bool isUp = CheckLevelUp();
        if (isUp == true)
        {
            Owner.StatComponent.SetHeroStat(Level);
            Owner.BroadcastStat();
            //Owner.Session.Send();
        }
    }

    private bool CheckLevelUp()
    {
        bool isUp = false;

        while (true)
        {
            if (IsMaxLevel())
                break;

            if (Exp < GetExpAmountForNextLevel(Level))
                break;

            Exp = Math.Max(0, Exp - GetExpAmountForNextLevel(Level));
            Level++;
            isUp = true;
        }

        return isUp;
    }

    public int GetExpAmountForNextLevel(int level)
    {
        HeroStatData statData;
        if (DataManager.HeroStatDict.TryGetValue(level, out statData) == false)
            return 0;

        return statData.Exp;
    }

    public bool IsMaxLevel()
    {
        return Level == DataManager.HeroStatDict.Count;
    }
}
