using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory :  IInventory
{
    public static Inventory Instance;

    
    private Dictionary<CarParts, int> _inventory = new Dictionary<CarParts, int>();
    private CarPartsDatabase _carPartsDatabase;

    public bool HasPart(CarParts part) => _inventory.ContainsKey(part) && _inventory[part] > 0;

    public Inventory(CarPartsDatabase carPartsDatabase)
    {
        _carPartsDatabase = carPartsDatabase;
        foreach (CarParts part in Enum.GetValues(typeof(CarParts)))
        {
            _inventory[part] = GameData.Instance.GetInventoryPart(part).count; 
        }
    }

    public int GetItemCount(CarParts part)
    {
        return _inventory.ContainsKey(part) ? _inventory[part] : 0;
    }
    
    public int GetPartCost(CarParts part)
    {
        CarPartData partData = _carPartsDatabase.GetCarPartData(part);
        
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
            GameData.Instance.SaveInventoryPart(part, _inventory[part]);

            InventoryUI inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
            {
                inventoryUI.UpdateInventoryUI();
            }
            if (RepairQueueManager.Instance.HasPendingRepairs(part))
            {
                ILift lift = RepairQueueManager.Instance.GetNextLift(part);
                if (lift != null && !lift.IsOccupied)
                {
                    lift.SetOccupied(true);
                    RepairQueueManager.Instance.RemoveFromQueue(part, lift);
                    lift.StartRepair(new CarPartData { partType = part }, GetRepairTime(part));
                }
            }
        }
    }
    
    public float GetRepairTime(CarParts part)
    {
        CarPartData partData = _carPartsDatabase.GetCarPartData(part);
        return partData != null ? partData.repairTime : 0f;
    }

    public bool RemoveItem(CarParts part, int count)
    {
        if (_inventory.ContainsKey(part) && _inventory[part] >= count)
        {
            _inventory[part] -= count;
            GameData.Instance.SaveInventoryPart(part, _inventory[part]);
            return true;
        }
        return false;
    }
    
    public bool HasItem(CarParts part, int count)
    {
        return _inventory.ContainsKey(part) && _inventory[part] >= count;
    }
}