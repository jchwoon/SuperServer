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

public class InventoryComponent
{
    public Hero Owner { get; private set; }

    Dictionary<int, Item> _ownerAllItems = new Dictionary<int, Item>();
    //Equipped
    Dictionary<ESlotType, Equipment> _equippedItems = new Dictionary<ESlotType, Equipment>();
    //Inventory
    Dictionary<int, Item> _equipItems = new Dictionary<int, Item>();
    Dictionary<int, Item> _consumeItems = new Dictionary<int, Item>();
    Dictionary<int, Item> _etcItems = new Dictionary<int, Item>();

    Dictionary<EConsumableType, long> _lastUseTicks = new Dictionary<EConsumableType, long>();
    int _consumableSlotCount;
    int _equipmentSlotCount;
    int _etcSlotCount;
    public const int DefaultSlotCount = 30;
    public InventoryComponent(Hero hero)
    {
        Owner = hero;
    }

    public void Init(DBHero dbHero)
    {
        _consumableSlotCount = dbHero.ConsumableSlotCount;
        _equipmentSlotCount = dbHero.EquipmentSlotCount;
        _etcSlotCount = dbHero.ETCSlotCount;

        foreach (DBItem dbItem in dbHero.Items)
        {
            Item item = ItemFactory.Instance.MakeItem(dbItem);
            if (item == null)
                continue;

            AddItem(item);
            if (item.ItemData.ItemType == EItemType.Equip)
            {
                Equipment equip = (Equipment)item;
                if ((int)equip.SlotType < (int)ESlotType.Equip)
                    equip.EquipItem(Owner);
            }
        }
    }

    public bool CheckFull(ItemData itemData)
    {
        bool isFull = false;
        switch (itemData.ItemType)
        {
            case EItemType.Consume:
                if (_consumeItems.Count >= _consumableSlotCount)
                    isFull = true;
                break;
            case EItemType.Equip:
                if (_equipItems.Count >= _equipmentSlotCount)
                    isFull = true;
                break;
            case EItemType.Etc:
                if (_etcItems.Count >= _etcSlotCount)
                    isFull = true;
                break;
            default:
                break;
        }

        return isFull;
    }

    //dropItem을 pickup후 dbItem과 Item으로 만들어주는 작업
    public void AddItem(DropItem item)
    {
        ItemFactory.Instance.AddStackOrAddNewItem(item);
    }
    //실제 메모리 상에 Item이 올라감 
    public void AddItem(Item item, bool sendPacket = false)
    {
        EItemType type = item.ItemData.ItemType;
        _ownerAllItems.Add(item.ItemDbId, item);
        switch (type)
        {
            case EItemType.Equip:
                _equipItems.Add(item.ItemDbId, item);
                break;
            case EItemType.Consume:
                _consumeItems.Add(item.ItemDbId, item);
                break;
            case EItemType.Etc:
                _etcItems.Add(item.ItemDbId, item);
                break;
        }

        if (sendPacket == true)
            SendAddPacket(item, EAddItemType.New);
    }

    //Stackable인 아이템
    public void AddCount(int itemDbId, int count)
    {
        Item item = GetItemByDbId(itemDbId);
        if (item == null)
            return;

        item.AddCount(count);
        SendAddPacket(item, EAddItemType.Old);
    }

    //아직 MaxStack에 도달하지 않은 Item들을 뽑기
    public List<Item> FindCanStackItems(int templateId)
    {
        List<Item> stackableItems = new List<Item> ();
        foreach (Item item in _ownerAllItems.Values)
        {
            if (item.ItemData.ItemId != templateId)
                continue;

            stackableItems.Add(item);
        }

        return stackableItems;
    }

    public List<ItemInfo> GetAllItemInfos()
    {
        List<ItemInfo> itemInfos = new List<ItemInfo>();
        foreach (Item item in _ownerAllItems.Values)
        {
            itemInfos.Add(item.Info);
        }

        return itemInfos;
    }

    public Item GetItemByDbId(int itemDbId)
    {
        Item item;
        if (_ownerAllItems.TryGetValue(itemDbId, out item) == false)
            return null;
        
        return item;
    }

    public bool CheckCanUseItem(EConsumableType consumeType, float coolTime)
    {
        if (_lastUseTicks.ContainsKey(consumeType) == false)
        {
            _lastUseTicks.Add(consumeType, Environment.TickCount64);
            return true;
        }

        long elapsedTime = Environment.TickCount64 - _lastUseTicks[consumeType];

        if (elapsedTime < coolTime * 1000)
            return false;

        return true;
    }

    public void UpdateCoolTick(EConsumableType consumeType)
    {
        if (_lastUseTicks.ContainsKey(consumeType) == true)
            _lastUseTicks[consumeType] = Environment.TickCount64;
    }

    public Equipment GetCurrentEquippedItem(ESlotType slotType)
    {
        Equipment equipItem;
        if (_equippedItems.TryGetValue(slotType, out equipItem) == false)
            return null;

        return equipItem;
    }

    public void RemoveItem(int itemDbId)
    {
        _ownerAllItems.Remove(itemDbId);
        _equipItems.Remove(itemDbId);
        _consumeItems.Remove(itemDbId);
        _etcItems.Remove(itemDbId);
    }

    #region Add/Remove 
    public void AddEquippedItem(ESlotType slotType, Equipment equipItem)
    {
        _equippedItems.Add(slotType, equipItem);
    }
    public void RemoveEquippedItem(ESlotType slotType)
    {
        _equippedItems.Remove(slotType);
    }
    #endregion

    public void SendAddPacket(Item item, EAddItemType addType)
    {
        AddItemToC addItemPacket = new AddItemToC();
        addItemPacket.ItemInfo = item.Info;
        addItemPacket.AddType = addType;
        Owner?.Session.Send(addItemPacket);
    }

    public void SendUseItemPacket(Item item)
    {
        UseItemToC useItemPacket = new UseItemToC();
        useItemPacket.ItemDbId = item.ItemDbId;

        Owner?.Session.Send(useItemPacket);
    }
}