using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject upgradeItemTemplate;
    [SerializeField] private Transform upgradeContent;
    [SerializeField] private Button toggleUpgradePanelButton;
    [SerializeField] private Button closeUpgradePanelButton;
    [SerializeField] private TextMeshProUGUI moneyText;

    private GameManager _gameManager;
    private IUpgradeService _upgradeService;
    private bool _isUpgradePanelOpen = false;
    private ILiftService _liftService;

    public void Initialize(GameManager gameManager, UpgradesDatabase upgradesDatabase, IUpgradeService upgradeService)
    {
        _gameManager = gameManager;
        _upgradeService = upgradeService;
        _upgradeService.SetUpgradeUI(this); 
    }


    private void Start()
    {
        toggleUpgradePanelButton.onClick.AddListener(ToggleUpgradePanel);
        closeUpgradePanelButton.onClick.AddListener(CloseUpgradePanel);
        upgradePanel.SetActive(false);
        UpdateUpgradeUI();
    }

    public void ToggleUpgradePanel()
    {
        _isUpgradePanelOpen = !_isUpgradePanelOpen;
        upgradePanel.SetActive(_isUpgradePanelOpen);

        if (_isUpgradePanelOpen)
        {
            UpdateUpgradeUI();
        }
    }

    private void CloseUpgradePanel()
    {
        _isUpgradePanelOpen = false;
        upgradePanel.SetActive(false);
    }

    public void UpdateUpgradeUI()
    {

        foreach (Transform child in upgradeContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var upgrade in _upgradeService.GetUpgrades())
        {
            CreateUpgradeItemUI(upgrade);
        }

        UpdateMoneyUI();
    }

    private void CreateUpgradeItemUI(UpgradeData upgrade)
    {
        var upgradeItem = Instantiate(upgradeItemTemplate, upgradeContent);
        upgradeItem.SetActive(true);

        TextMeshProUGUI[] textComponents = upgradeItem.GetComponentsInChildren<TextMeshProUGUI>();
        Button purchaseButton = upgradeItem.GetComponentInChildren<Button>();
        Image iconImage = upgradeItem.GetComponentInChildren<Image>();

        if (textComponents.Length >= 2)
        {
            textComponents[0].text = upgrade.upgradeName;
            textComponents[1].text = $"Cost: {upgrade.GetCurrentCost(GameData.Instance.GetUpgradeSaveData(upgrade.upgradeType).currentLevel)} \n (Level: {GameData.Instance.GetUpgradeSaveData(upgrade.upgradeType).currentLevel})";
        }
        
        if (iconImage != null)
        {
            iconImage.sprite = upgrade.icon;
        }


        if (upgrade.upgradeName == "Lift" && _liftService.AllLiftsPurchased())
        {
            if (purchaseButton != null)
            {
                purchaseButton.interactable = false;
                purchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Max upgrade"; 
            }
        }
        else if (purchaseButton != null)
        {
            purchaseButton.onClick.AddListener(() => _upgradeService.PurchaseUpgrade(upgrade));
        }
    }


    private void UpdateMoneyUI()
    {
        if (_gameManager != null && moneyText != null)
        {
            moneyText.text = $"{_gameManager.GetMoney()}";
        }
    }
}






