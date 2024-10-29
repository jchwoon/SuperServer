using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InventoryComponent
{
    public Hero Owner { get; private set; }
    public InventoryComponent(Hero hero)
    {
        Owner = hero;
    }

    public bool CheckFull()
    {
        return false;
    }
}