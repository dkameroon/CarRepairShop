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
        upgradeItem.SetActive(false);

        TextMeshProUGUI[] textComponents = upgradeItem.GetComponentsInChildren<TextMeshProUGUI>();
        Button purchaseButton = upgradeItem.GetComponentInChildren<Button>();
        Image iconImage = upgradeItem.GetComponentInChildren<Image>();
        TextMeshProUGUI buttonText = purchaseButton.GetComponentInChildren<TextMeshProUGUI>();

        int currentLevel = GameData.Instance.GetUpgradeSaveData(upgrade.upgradeType).currentLevel;
    
        if (textComponents.Length >= 2)
        {
            textComponents[0].text = upgrade.upgradeName;
            textComponents[1].text = $"Cost: {upgrade.GetCurrentCost(currentLevel)} \n (Level: {currentLevel})";
        }

        if (iconImage != null)
        {
            iconImage.sprite = upgrade.icon;
        }

        if (purchaseButton != null)
        {
            purchaseButton.onClick.RemoveAllListeners();

            if (currentLevel >= upgrade.maxLevel) 
            {
                purchaseButton.interactable = false;
                buttonText.text = "Max"; 
            }
            else
            {
                purchaseButton.interactable = true;
                buttonText.text = "Buy"; 
                purchaseButton.onClick.AddListener(() => _upgradeService.PurchaseUpgrade(upgrade));
            }
        }

        upgradeItem.SetActive(true);
    }



    private void UpdateMoneyUI()
    {
        if (_gameManager != null && moneyText != null)
        {
            moneyText.text = $"{_gameManager.GetMoney()}";
        }
    }
}






