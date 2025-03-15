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
    [SerializeField] private Button closeInventoryButton;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private CarPartsDatabase carPartsDatabase;
    
    [SerializeField] private GameObject progressBarPrefab;
    private GameObject progressBarInstance;

    private IInventory _inventory;
    private GameManager _gameManager;
    private bool _isInventoryOpen = false;

    public event Action<CarParts> OnPartBought;

    private void Start()
    {
        _inventory = new Inventory();
        _gameManager = FindObjectOfType<GameManager>();
        
        toggleInventoryButton.onClick.AddListener(ToggleInventory);
        closeInventoryButton.onClick.AddListener(CloseInventory);
        
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
            bool partIsNeeded = FindObjectOfType<Car>()?.isWaitingForPart == true;

            if (partIsNeeded)
            {
                
                OnPartBought?.Invoke(partData.partType);
                Debug.Log($"✅ Куплена нужная деталь {partData.partName}. Оставшиеся деньги: {_gameManager.GetMoney()}");
            }
            else
            {
                _inventory.AddItem(partData.partType, 1);
                Debug.Log($"✅ Куплена деталь {partData.partName}, добавлена в инвентарь. Оставшиеся деньги: {_gameManager.GetMoney()}");
            }

            UpdateInventoryUI();
        }
        else
        {
            Debug.Log("❌ Недостаточно денег для покупки этой детали.");
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"{_gameManager.GetMoney()}";
        }
    }

    
    public void StartRepairProgressBar(Vector3 liftPosition)
    {
        if (progressBarInstance != null)
        {
            Destroy(progressBarInstance);
        }

        progressBarInstance = Instantiate(progressBarPrefab);
        progressBarInstance.SetActive(true);


        Vector3 worldPos = liftPosition + new Vector3(0, 2, 0);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        
        progressBarInstance.transform.position = screenPos;
    }


    public void UpdateRepairProgressBar(float progress)
    {
        if (progressBarInstance != null)
        {
            Image progressBarFillImage = progressBarInstance.GetComponentInChildren<Image>();
            if (progressBarFillImage != null)
            {
                progressBarFillImage.fillAmount = progress;
            }
        }
    }


    public void HideRepairProgressBar()
    {
        if (progressBarInstance != null)
        {
            progressBarInstance.SetActive(false);
        }
    }
}
