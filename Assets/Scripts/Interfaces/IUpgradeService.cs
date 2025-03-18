using System.Collections.Generic;

public interface IUpgradeService
{
    void PurchaseUpgrade(UpgradeData upgrade);
    List<UpgradeData> GetUpgrades();
    void SetUpgradeUI(UpgradeUI upgradeUI);
    float GetRepairSpeedMultiplier();
    float GetProfitMultiplier();
}