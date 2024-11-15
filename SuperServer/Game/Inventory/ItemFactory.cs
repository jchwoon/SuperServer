using Google.Protobuf.Enum;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.DB;
using SuperServer.Game.Object;
using SuperServer.Logic;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Inventory
{
    public class ItemFactory : Singleton<ItemFactory>
    {
        public Item MakeItem(DBItem dbItem)
        {
            int templateId = dbItem.ItemTemplateId;
            ItemData itemData;
            if (DataManager.ItemDict.TryGetValue(templateId, out itemData) == false)
                return null;

            Item item = null;

            switch (itemData.ItemType)
            {
                case EItemType.Consume:
                    item = new Consumable(dbItem.OwnerDbId, templateId);
                    break;
                case EItemType.Equip:
                    item = new Equipment(dbItem.OwnerDbId, templateId);
                    break;
                case EItemType.Etc:
                    item = new Etc(dbItem.OwnerDbId, templateId);
                    break;

            }

            if (item != null)
            {
                item.ItemDbId = dbItem.DBItemId;
                item.Count = dbItem.Count;
                item.SlotType = dbItem.SlotType;
            }

            return item;
        }

        public void AddStackOrAddNewItem(DropItem dropItem)
        {
            int dropItemCount = dropItem.Count;

            //스택커블로 채울 수 있다면 먼저 채워넣기
            if (dropItem.ItemData.Stackable == true)
            {
                List<Item> stackableItems = dropItem.Owner.Inventory.FindCanStackItems(dropItem.ItemData.ItemId);

                foreach (Item stackableItem in stackableItems)
                {
                    if (dropItemCount <= 0)
                        break;
                    
                    if ((dropItemCount - stackableItem.GetAvailableStackCount()) <= 0)
                    {
                        DBCommander.Instance.Push(DBLogic.Instance.ApplyChangedCountItem, stackableItem.ItemDbId, dropItemCount);
                        dropItem.Owner.Inventory.AddCount(stackableItem.ItemDbId, dropItemCount);
                    }
                    else
                    {
                        DBCommander.Instance.Push(DBLogic.Instance.ApplyChangedCountItem, stackableItem.ItemDbId, stackableItem.GetAvailableStackCount());
                        dropItem.Owner.Inventory.AddCount(stackableItem.ItemDbId, stackableItem.GetAvailableStackCount());
                    }
                    dropItemCount -= stackableItem.GetAvailableStackCount();
                }
            }

            //새로운 DBItem생성
            if (dropItemCount > 0 && dropItem.Owner.Inventory.CheckFull(dropItem.ItemData) == false)
            {
                ESlotType slotType = ESlotType.None;
                switch (dropItem.ItemData.ItemType)
                {
                    case EItemType.Equip:
                        slotType = ESlotType.Equip;
                        break;
                    case EItemType.Consume:
                        slotType = ESlotType.Consume;
                        break;
                    case EItemType.Etc:
                        slotType = ESlotType.Etc;
                        break;
                }

                if (slotType == ESlotType.None)
                    return;

                DBItem newItem = new DBItem()
                {
                    Count = dropItemCount,
                    OwnerDbId = dropItem.OwnerDBId,
                    ItemTemplateId = dropItem.ItemData.ItemId,
                    SlotType = slotType
                };

                DBCommander.Instance.Push(DBLogic.Instance.AddNewItem, dropItem.Owner, newItem);
            }
        }

        public void ApplyNewItemToInven(Hero hero, DBItem newDBItem)
        {
            if (newDBItem != null)
            {
                Item newItem = MakeItem(newDBItem);
                hero.Inventory.AddItem(newItem, true);
            }
        }
    }
}
