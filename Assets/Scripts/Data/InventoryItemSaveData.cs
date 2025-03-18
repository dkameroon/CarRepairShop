using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemSaveData
{
    public CarParts partType;
    public int count;

    public InventoryItemSaveData(CarParts partType, int count)
    {
        this.partType = partType;
        this.count = count;
    }
}
