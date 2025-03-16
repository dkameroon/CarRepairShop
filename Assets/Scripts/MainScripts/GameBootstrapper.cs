using System.Collections.Generic;
using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject liftPrefab;
    [SerializeField] private UpgradesDatabase upgradesDatabase;
    [SerializeField] private GameManager gameManager;

    private UIManager uiManager;
    private UpgradeUI upgradeUI;
    private LiftService liftService;

    public GameObject LiftPrefab => liftPrefab;

    private void Awake()
    {
        new GameData(SaveSystem.Load().Money,SaveSystem.Load().LiftsPurchased,SaveSystem.Load().upgrades);
        uiManager = FindObjectOfType<UIManager>();
        upgradeUI = FindObjectOfType<UpgradeUI>();

        if (uiManager == null)
        {
            Debug.LogError("⚠️ UIManager не найден в сцене! Убедитесь, что он присутствует.");
        }
        else
        {
            Debug.Log("✅ UIManager найден.");
        }

        if (upgradeUI == null)
        {
            Debug.LogError("⚠️ UpgradeUI не найден в сцене! Убедитесь, что он присутствует.");
        }
        else
        {
            UpgradeService upgradeService = new UpgradeService(gameManager, upgradesDatabase, liftService);
            upgradeUI.Initialize(gameManager, upgradesDatabase, upgradeService);
            upgradeService.SetUpgradeUI(upgradeUI);

        }
    }
}
