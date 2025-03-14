using System;
using System.Collections.Generic;

public class Inventory : IInventory
{
    private Dictionary<CarParts, int> _inventory = new Dictionary<CarParts, int>();

    public Inventory()
    {
        foreach (CarParts part in Enum.GetValues(typeof(CarParts)))
        {
            _inventory[part] = 0; 
        }
    }

    public int GetItemCount(CarParts part)
    {
        return _inventory.ContainsKey(part) ? _inventory[part] : 0;
    }

    public void AddItem(CarParts part, int count)
    {
        if (_inventory.ContainsKey(part))
        {
            _inventory[part] += count;
        }
    }
    
    

    public void RemoveItem(CarParts part, int count)
    {
        if (_inventory.ContainsKey(part) && _inventory[part] >= count)
        {
            _inventory[part] -= count;
        }
    }

    public bool HasItem(CarParts part, int count)
    {
        return _inventory.ContainsKey(part) && _inventory[part] >= count;
    }
}