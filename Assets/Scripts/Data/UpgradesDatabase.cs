using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesDatabase", menuName = "Upgrades/Upgrades Database")]
public class UpgradesDatabase : ScriptableObject
{
    public List<UpgradeData> upgrades;
}

[System.Serializable]
public class UpgradeData
{
    
    public string upgradeName;
    public Sprite icon;
    public int baseCost;
    public float costIncreaseCoefficient = 1.2f;
    public int maxLevel;
    public UpgradeType upgradeType;

    public int GetCurrentCost(int currentLevel)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costIncreaseCoefficient, currentLevel));
    }
    
    public bool CanUpgrade(int currentLevel)
    {
        return currentLevel < maxLevel;
    }


}

public enum UpgradeType
{
    BuyLift,
    IncreaseRepairSpeed,
    IncreaseProfit
}

[System.Serializable]
public class SaveData
{
    public UpgradeType upgradeType;
    public int currentLevel;

    public SaveData(UpgradeType upgradeType, int currentLevel)
    {
        this.upgradeType = upgradeType;
        this.currentLevel = currentLevel;
    }
}