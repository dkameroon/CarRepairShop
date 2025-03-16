using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : IInventory
{
    private Dictionary<CarParts, int> _inventory = new Dictionary<CarParts, int>();
    private CarPartsDatabase _carPartsDatabase;


    public Inventory(CarPartsDatabase carPartsDatabase)
    {
        _carPartsDatabase = carPartsDatabase;

        foreach (CarParts part in Enum.GetValues(typeof(CarParts)))
        {
            _inventory[part] = 0; 
        }
    }

    public int GetItemCount(CarParts part)
    {
        return _inventory.ContainsKey(part) ? _inventory[part] : 0;
    }
    
    public int GetPartCost(CarParts part)
    {
        CarPartData partData = _carPartsDatabase.carParts.Find(p => p.partType == part);
        
        if (partData != null)
        {
            return partData.purchaseCost;
        }
        else
        {
            return 0;
        }
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