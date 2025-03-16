using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeService : IUpgradeService
{
    public static IUpgradeService Instance;
    private IGameManager _gameManager;
    private UpgradesDatabase _upgradesDatabase;
    private UpgradeUI _upgradeUI;
    private ILiftService _liftService;
    

    public UpgradeService(IGameManager gameManager, UpgradesDatabase upgradesDatabase, ILiftService liftService)
    {
        Instance = this;
        _gameManager = gameManager;
        _upgradesDatabase = upgradesDatabase;
        _liftService = liftService;
        
    }


    public void SetUpgradeUI(UpgradeUI upgradeUI)
    {
        _upgradeUI = upgradeUI;
    }

    public void PurchaseUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogError("UpgradeData is null!");
            return;
        }

        int cost = upgrade.GetCurrentCost(GameData.Instance.GetUpgradeSaveData(upgrade.upgradeType).currentLevel);

        if (_gameManager.GetMoney() >= cost)
        {
            if (upgrade.CanUpgrade(GameData.Instance.GetUpgradeSaveData(upgrade.upgradeType).currentLevel))
            {
                Debug.Log($"Purchasing {upgrade.upgradeName} for {cost} money.");
                _gameManager.SpendMoney(cost);
                ApplyUpgrade(upgrade);
                LevelUp(upgrade);

                Debug.Log($"✅ {upgrade.upgradeName} purchased! New Level: {GameData.Instance.GetUpgradeSaveData(upgrade.upgradeType).currentLevel}");
            
                _upgradeUI.UpdateUpgradeUI();
            }
            else
            {
                Debug.Log("❌ This upgrade has reached the maximum level.");
            }
        }
        else
        {
            Debug.Log("❌ Not enough money to purchase this upgrade.");
        }
    }
    
    public void LevelUp(UpgradeData upgradeData)
    {
        var upgradeSaveData = GameData.Instance.GetUpgradeSaveData(upgradeData.upgradeType);
        if (upgradeSaveData.currentLevel < upgradeData.maxLevel)
        {
            Debug.Log("Current Level : " + upgradeSaveData.currentLevel);
            Debug.Log("Next Level : " + upgradeSaveData.currentLevel + 1);
            int newLevel = upgradeSaveData.currentLevel + 1;
            GameData.Instance.SaveUpgrade(upgradeSaveData.upgradeType, newLevel);
        }
        
    }

    private void ApplyUpgrade(UpgradeData upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeType.BuyLift:
                    _liftService.BuyAndPlaceLift(upgrade.GetCurrentCost(GameData.Instance.GetUpgradeSaveData(upgrade.upgradeType).currentLevel));
                
                break;

            case UpgradeType.IncreaseRepairSpeed:

                Debug.Log("Increasing repair speed.");
                break;

            case UpgradeType.IncreaseProfit:

                Debug.Log("Increasing profit.");
                break;
        }
        }

    public List<UpgradeData> GetUpgrades()
    {
        return _upgradesDatabase.upgrades;
    }
    
    }
