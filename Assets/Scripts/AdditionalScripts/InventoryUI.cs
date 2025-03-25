using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IInventoryUI
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryItemTemplate;
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private Button toggleInventoryButton;
    [SerializeField] private Button closeInventoryButton;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private CarPartsDatabase carPartsDatabase;
    
    [SerializeField] private GameObject progressBarPrefab;
    private GameObject progressBarInstance;

    private IInventory _inventory;
    private GameManager _gameManager;
    private bool _isInventoryOpen = false;

    private void Awake()
    {
        _inventory = GameBootstrapper.instance.GetInventory();
        _gameManager = FindObjectOfType<GameManager>();
        
    }

    private void Start()
    {
        toggleInventoryButton.onClick.AddListener(ToggleInventory);
        closeInventoryButton.onClick.AddListener(CloseInventory);
        
        inventoryPanel.SetActive(false);
        UpdateInventoryUI();
    }

    private void ToggleInventory()
    {
        _isInventoryOpen = !_isInventoryOpen;
        inventoryPanel.SetActive(_isInventoryOpen);
        if (_isInventoryOpen) { UpdateInventoryUI(); }
    }


    private void CloseInventory()
    {
        _isInventoryOpen = false;
        inventoryPanel.SetActive(false);
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
            textComponents[1].text = $"You have: {_inventory.GetItemCount(partData.partType)} \n Cost : {partData.purchaseCost}";
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
            if (RepairQueueManager.Instance.HasPendingRepairs(partData.partType))
            {
                ILift nextLift = RepairQueueManager.Instance.GetNextLift(partData.partType);
                if (nextLift != null)
                {
                    Car car = nextLift.GetCurrentCar();
                    if (car != null)
                    {
                        car.StartRepairWithPart(partData);
                    }
                }
            }

            UpdateInventoryUI();
        }
        else
        {
            Debug.Log("No money.");
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"{_gameManager.GetMoney()}";
        }
    }

    
   }
