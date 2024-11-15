using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using SuperServer.Data;
using SuperServer.DB;
using SuperServer.Game.Inventory;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Item
{
    int _ownerDBId;
    public ItemData ItemData { get; private set; }
    public ItemInfo Info { get; private set; } = new ItemInfo();

    public int ItemDbId
    {
        get { return Info.ItemDbId; }
        set { Info.ItemDbId = value; }
    }

    public int Count
    {
        get { return Info.Count; }
        set { Info.Count = value; }
    }
    public ESlotType SlotType
    {
        get { return Info.SlotType; }
        set { Info.SlotType = value; }
    }
    public Item(int ownerDBId, int itemId)
    {
        if (DataManager.ItemDict.TryGetValue(itemId, out ItemData itemData) == true)
        {
            ItemData = itemData;
            Info.TemplateId = itemId;
        }
        _ownerDBId = ownerDBId;
    }

    public int GetAvailableStackCount()
    {
        return ItemData.MaxStack - Count;
    }

    public void AddCount(int addCount)
    {
        Count = Math.Clamp(Count + addCount, 0, ItemData.MaxStack);
    }

    public virtual bool CheckCanUseItemAndUse(InventoryComponent inventory)
    {
        return false;
    }

    protected void SendChangeSlotPacket(Hero owner)
    {
        ChangeSlotTypeToC changeSlotPacket = new ChangeSlotTypeToC();
        changeSlotPacket.ItemDbId = ItemDbId;
        changeSlotPacket.SlotType = SlotType;

        owner.Session?.Send(changeSlotPacket);
    }
}