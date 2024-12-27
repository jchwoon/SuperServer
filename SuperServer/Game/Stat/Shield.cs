using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Stat
{
    public class Shield
    {
        public int ShieldValue { get; private set; }
        //쉴드량이 0가 되었을 때 true
        public bool IsBroken { get; private set; }
        public Shield(int value)
        {
            ShieldValue = value;
        }

        public void OnDamage(int value)
        {
            ShieldValue = (int)MathF.Max(0, ShieldValue - value);
            if (ShieldValue == 0)
            {
                IsBroken = true;
            }
        }
    }
}
