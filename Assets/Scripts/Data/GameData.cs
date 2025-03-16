using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static GameData Instance;
    public int Money;
    public int LiftsPurchased;
    public List<SaveData> upgrades = new List<SaveData>();

    public GameData(int money, int liftsPurchased, List<SaveData> upgrades)
    {
        Instance = this;
        Money = money;
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

    public void SaveMoney(int money)
    {
        Money = money;
        SaveSystem.Save(this);
    }

    public void SaveLiftsPurchased(int liftsPurchased)
    {
        LiftsPurchased = liftsPurchased;
        SaveSystem.Save(this);
    }

    public List<SaveData> GetAllUpgradesSaveData()
    {
        Debug.Log("Upgrades: " + upgrades.Count);
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