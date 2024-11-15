using SuperServer.Game.Inventory;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Room
{
    public partial class GameRoom
    {
        public void PickupDropItem(Hero picker, int itemObjectId)
        {
            if (picker == null)
                return;

            DropItem dropItem;
            if (_dropItems.TryGetValue(itemObjectId, out dropItem) == false)
                return;

            dropItem.Pickup(picker);
        }
        public void HandleUseItem(Hero hero, int itemDbId)
        {
            Item item = hero.Inventory.GetItemByDbId(itemDbId);

            if (item == null)
                return;

            bool canUse = item.CheckCanUseItemAndUse(hero.Inventory);
            if (canUse == false)
                return;
        }

        public void HandleEquipItem(Hero hero, int itemDbId)
        {
            Item item = hero.Inventory.GetItemByDbId(itemDbId);

            if (item == null)
                return;

            Equipment equipItem = (Equipment)item;

            equipItem.EquipItem(hero);
        }

        public void HandleUnEquipItem(Hero hero, int itemDbId)
        {
            Item item = hero.Inventory.GetItemByDbId(itemDbId);

            if (item == null)
                return;

            Equipment equipItem = (Equipment)item;

            equipItem.UnEquipItem(hero);
        }
    }
}
