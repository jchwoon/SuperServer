using SuperServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Inventory
{
    public class Etc : Item
    {
        public EtcData EtcData { get; private set; }
        public Etc(int ownerDBId, int itemId) : base(ownerDBId, itemId)
        {
            EtcData = (EtcData)ItemData;
        }
    }
}
