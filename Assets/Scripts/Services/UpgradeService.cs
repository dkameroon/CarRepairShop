using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeService : IUpgradeService
{
    public static IUpgradeService Instance;
    private IGameManager _gameManager;
    private UpgradesDatabase _upgradesDatabase;
    private UpgradeUI _upgradeUI;
    private ILiftService _liftService;
    private IMechanicService _mechanicService;

    private float repairSpeedMultiplier;
    private float profitMultiplier;

    public UpgradeService(IGameManager gameManager, UpgradesDatabase upgradesDatabase, ILiftService liftService, IMechanicService mechanicService)
    {
        Instance = this;
        _gameManager = gameManager;
        _upgradesDatabase = upgradesDatabase;
        _liftService = liftService;
        _mechanicService = mechanicService;

        int repairSpeedLevel = GameData.Instance.GetUpgradeSaveData(UpgradeType.IncreaseRepairSpeed).currentLevel;
        int profitLevel = GameData.Instance.GetUpgradeSaveData(UpgradeType.IncreaseProfit).currentLevel;

        repairSpeedMultiplier = CalculateRepairSpeedMultiplier(repairSpeedLevel);
        profitMultiplier = CalculateProfitMultiplier(profitLevel);
    }
    
    private float CalculateRepairSpeedMultiplier(int level)
    {
        return Mathf.Pow(1.3f, level);
    }

    private float CalculateProfitMultiplier(int level)
    {
        return Mathf.Pow(1.1f, level);
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
                _gameManager.SpendMoney(cost);
                ApplyUpgrade(upgrade);
                LevelUp(upgrade);
                SoundEffectsManager.Instance.PlaySound("UpgradeSound");
                UIManager.Instance.ShowNotification($"Upgrade {upgrade.upgradeName} \n successfully purchased");
                _upgradeUI.UpdateUpgradeUI();
            }
            else
            {
                Debug.Log("This upgrade has reached the maximum level.");
            }
        }
        else
        {
            UIManager.Instance.ShowNotification("Not enough money!");
        }
    }

    public void LevelUp(UpgradeData upgradeData)
    {
        var upgradeSaveData = GameData.Instance.GetUpgradeSaveData(upgradeData.upgradeType);
        if (upgradeSaveData.currentLevel < upgradeData.maxLevel)
        {
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
            case UpgradeType.HireMechanic:
                _mechanicService.BuyAndSpawnMechanic(upgrade.GetCurrentCost(GameData.Instance.GetUpgradeSaveData(upgrade.upgradeType).currentLevel));
                break;
            case UpgradeType.IncreaseRepairSpeed:
                repairSpeedMultiplier *= 1.3f;
                break;

            case UpgradeType.IncreaseProfit:
                profitMultiplier *= 1.1f;
                break;
        }
    }

    public float GetRepairSpeedMultiplier() => repairSpeedMultiplier;
    public float GetProfitMultiplier() => profitMultiplier;

    public List<UpgradeData> GetUpgrades()
    {
        return _upgradesDatabase.upgrades;
    }
}
