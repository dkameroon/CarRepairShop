using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static GameData Instance;
    public int Money;
    public int Fragments;
    public int LiftsPurchased;
    public List<SaveData> upgrades = new List<SaveData>();
    public List<InventoryItemSaveData> inventory = new List<InventoryItemSaveData>();
    public float MusicVolume;
    public float SoundVolume;
   

    public GameData(int money, int Fragments, int liftsPurchased,  List<SaveData> upgrades, List<InventoryItemSaveData> inventory)
    {
        Instance = this;
        Money = money;
        Fragments = Fragments;
        LiftsPurchased = liftsPurchased;
        if (upgrades == null || upgrades.Count == 0)
        {
            this.upgrades = new List<SaveData>
            {
                new SaveData(UpgradeType.BuyLift, 0),
                new SaveData(UpgradeType.IncreaseRepairSpeed, 0),
                new SaveData(UpgradeType.IncreaseProfit, 0)
            };
        }
        else
        {
            this.upgrades = new List<SaveData>(upgrades);
        }
        if (inventory == null || inventory.Count == 0)
        {
            this.inventory = new List<InventoryItemSaveData>
            {
                new InventoryItemSaveData(CarParts.Battery, 0),
                new InventoryItemSaveData(CarParts.Brakes, 0),
                new InventoryItemSaveData(CarParts.Engine, 0),
                new InventoryItemSaveData(CarParts.Exhaust, 0),
                new InventoryItemSaveData(CarParts.Transmission, 0),
                new InventoryItemSaveData(CarParts.Radiator, 0),
                new InventoryItemSaveData(CarParts.EngineOil, 0)
            };
        }
        else
        {
            this.inventory = new List<InventoryItemSaveData>(inventory);
        }
        MusicVolume = 0.5f;
        SoundVolume = 0.5f;
    }

    public void SaveUpgrade(UpgradeType upgradeType, int level)
    {
        if (upgrades.Contains(GetUpgradeSaveData(upgradeType)))
        {
            upgrades.Remove(GetUpgradeSaveData(upgradeType));
        }
        upgrades.Add(new SaveData(upgradeType, level));
        SaveSystem.Save(this);
    }

    public void SaveMusicVolume(float volume)
    {
        MusicVolume = volume;
        SaveSystem.Save(this);
    }
    
    public void SaveSoundsVolume(float volume)
    {
        SoundVolume = volume;
        SaveSystem.Save(this);
    }

    public InventoryItemSaveData GetInventoryPart(CarParts part)
    {
        foreach (var inventoryPart in inventory)
        {
            if (inventoryPart.partType == part)
            {
                return inventoryPart;
            }
        }
        return null;
    }

    public void SaveInventoryPart(CarParts partType, int count)
    {
        if (inventory.Contains(GetInventoryPart(partType)))
        {
            inventory.Remove(GetInventoryPart(partType));
        }
        inventory.Add(new InventoryItemSaveData(partType, count));
        SaveSystem.Save(this);
    }

    
    public void SaveMoney(int money)
    {
        Money = money;
        SaveSystem.Save(this);
    }

    public void SaveFragments(int fragments)
    {
        Fragments = fragments;
        SaveSystem.Save(this);
    }

    public void SaveLiftsPurchased(int liftsPurchased)
    {
        LiftsPurchased = liftsPurchased;
        SaveSystem.Save(this);
    }

    public List<SaveData> GetAllUpgradesSaveData()
    {
        return new List<SaveData>(upgrades);
    }

    public SaveData GetUpgradeSaveData(UpgradeType upgradeType)
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.upgradeType == upgradeType)
            {
                return upgrade;
            }
        }
        return null;
    }

    public void Save()
    {
        SaveSystem.Save(this);
    }

    public void Load()
    {
        GameData data = SaveSystem.Load();
        if (data != null)
        {
            Money = data.Money;
            LiftsPurchased = data.LiftsPurchased;
            upgrades = new List<SaveData>(data.upgrades);
        }
    }
}