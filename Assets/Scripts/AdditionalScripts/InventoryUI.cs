using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IInventoryUI
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryItemTemplate;
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private Button toggleInventoryButton;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private CarPartsDatabase carPartsDatabase;

    private IInventory _inventory;
    private GameManager _gameManager;
    private bool _isInventoryOpen = false;
    
    public event Action<CarParts> OnPartBought;
    

    private void Start()
    {
        _inventory = new Inventory();
        _gameManager = FindObjectOfType<GameManager>();

        toggleInventoryButton.onClick.AddListener(ToggleInventory);
        inventoryPanel.SetActive(false);
        UpdateInventoryUI();
    }

    private void ToggleInventory()
    {
        _isInventoryOpen = !_isInventoryOpen;
        inventoryPanel.SetActive(_isInventoryOpen);

        if (_isInventoryOpen)
        {
            UpdateInventoryUI();
        }
    }

    public void UpdateInventoryUI()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        foreach (CarPartData partData in carPartsDatabase.carParts)
        {
            CreateInventoryItemUI(partData);
        }

        UpdateMoneyUI();
    }

    public IInventory GetInventory()
    {
        return _inventory;
    }
    private void CreateInventoryItemUI(CarPartData partData)
    {
        var inventoryItem = Instantiate(inventoryItemTemplate, inventoryContent);
        inventoryItem.SetActive(true);
        
        TextMeshProUGUI[] textComponents = inventoryItem.GetComponentsInChildren<TextMeshProUGUI>();
        Image iconImage = inventoryItem.GetComponentInChildren<Image>();
        Button purchaseButton = inventoryItem.GetComponentInChildren<Button>();

        if (textComponents.Length >= 2)
        {
            textComponents[0].text = partData.partName;
            textComponents[1].text = $"You have: {_inventory.GetItemCount(partData.partType)}";
        }
        else
        {
            Debug.LogError("Not enough TextMeshProUGUI components found in inventory item template.");
        }

        if (iconImage != null)
        {
            iconImage.sprite = partData.icon;
        }

        if (purchaseButton != null)
        {
            purchaseButton.onClick.AddListener(() => PurchaseItem(partData));
        }
        else
        {
            Debug.LogError("Button component missing in the inventory item template.");
        }
    }

    private void PurchaseItem(CarPartData partData)
    {
        if (_gameManager.SpendMoney(partData.purchaseCost))
        {
            _inventory.AddItem(partData.partType, 1);
            OnPartBought?.Invoke(partData.partType); 
            Debug.Log($"Purchased {partData.partName}. Remaining money: {_gameManager.GetMoney()}");
            UpdateInventoryUI();
        }
        else
        {
            Debug.Log("Not enough money to purchase this item.");
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"Money: ${_gameManager.GetMoney()}";
        }
    }
}
