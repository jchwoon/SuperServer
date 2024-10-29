using SuperServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Item
{
    public ItemData ItemData { get; private set; }
    public Hero Owner { get; private set; }
    public Item(Hero owner, int itemId)
    {
        Owner = owner;
        if (DataManager.ItemDict.TryGetValue(itemId, out ItemData itemData) == true)
            ItemData = itemData;
    }
}