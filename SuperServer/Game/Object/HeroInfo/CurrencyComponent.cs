using Google.Protobuf.Enum;
using Google.Protobuf.WellKnownTypes;
using SuperServer.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Object.HeroInfo
{
    public class CurrencyComponent
    {
        public int Gold
        {
            get { return Owner.MyHeroInfo.Gold; }
            set { Owner.MyHeroInfo.Gold = value; }
        }
        public Hero Owner { get; private set; }

        public CurrencyComponent(Hero owner)
        {
            Owner = owner;
        }

        public void AddGold(int gold)
        {
            Gold += gold;
        }

        public void ReduceGold(int gold)
        {
            Gold -= gold;
        }

        public bool CanPay(ECurrencyType currencyType, int value)
        {
            //Temp 나중에 Currency가 추가 되면 수정
            if (value > Gold)
            {
                return false;
            }
            else
            {
                ReduceGold(value);
                return true;
            }
        }
    }
}
