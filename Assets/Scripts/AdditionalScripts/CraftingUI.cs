using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private GameObject craftingItemTemplate;
    [SerializeField] private Transform craftingContent;
    [SerializeField] private Button toggleCraftingButton;
    [SerializeField] private Button closeCraftingButton;
    [SerializeField] private TextMeshProUGUI fragmentText;
    
    private ICurrencyService _currencyService;
    private ICraftingSystem _craftingSystem;
    private InventoryUI _inventoryUI;
    private CraftingItemsDatabase _craftingDatabase;
    private bool _isCraftingOpen = false;
    
    public void Initialize(ICurrencyService currencyService, ICraftingSystem craftingSystem, CraftingItemsDatabase craftingDatabase, InventoryUI inventoryUI)
    {
        _currencyService = currencyService;
        _craftingSystem = craftingSystem;
        _craftingDatabase = craftingDatabase;
        _inventoryUI = inventoryUI;

        toggleCraftingButton.onClick.AddListener(ToggleCrafting);
        closeCraftingButton.onClick.AddListener(CloseCrafting);

        _craftingSystem.OnCraftingSuccess += HandleCraftingSuccess;

        craftingPanel.SetActive(false);

        UpdateCraftingUI();
    }

    private void HandleCraftingSuccess()
    {
        _inventoryUI.UpdateInventoryUI();
    }

    private void ToggleCrafting()
    {
        _isCraftingOpen = !_isCraftingOpen;
        craftingPanel.SetActive(_isCraftingOpen);

        if (_isCraftingOpen)
        {
            UpdateCraftingUI();
        }
    }

    private void CloseCrafting()
    {
        _isCraftingOpen = false;
        craftingPanel.SetActive(false);
    }

    private void UpdateCraftingUI()
    {
        foreach (Transform child in craftingContent)
        {
            Destroy(child.gameObject);
        }
        foreach (CraftableItem craftableItem in _craftingDatabase.craftableItems)
        {
            CreateCraftingItemUI(craftableItem);
        }
        UpdateFragmentUI();
    }

    private void CreateCraftingItemUI(CraftableItem craftableItem)
    {
        var craftingItem = Instantiate(craftingItemTemplate, craftingContent);
        craftingItem.SetActive(true);

        TextMeshProUGUI[] textComponents = craftingItem.GetComponentsInChildren<TextMeshProUGUI>();
        Image iconImage = craftingItem.GetComponentInChildren<Image>();
        Button craftButton = craftingItem.GetComponentInChildren<Button>();

        if (textComponents.Length >= 2)
        {
            textComponents[0].text = craftableItem.itemName;
            textComponents[1].text = $" {craftableItem.requiredFragments} fragments";
        }
        else
        {
            Debug.LogError("Not enough TextMeshProUGUI components found in crafting item template.");
        }

        if (iconImage != null)
        {
            iconImage.sprite = craftableItem.itemIcon;
        }

        if (craftButton != null)
        {
            craftButton.onClick.AddListener(() => OnCraftButtonClick(craftableItem.partType));
        }
        else
        {
            Debug.LogError("Button component missing in the crafting item template.");
        }
    }

    private void OnCraftButtonClick(CarParts carPart)
    {
        TryCraftItem(carPart);
    }

    private void TryCraftItem(CarParts carPart)
    {
        if (_craftingSystem.TryCraft(carPart))
        {
            UpdateCraftingUI();
        }
    }

    private void UpdateFragmentUI()
    {
        if (fragmentText != null)
        {
            fragmentText.text = $"{_currencyService.GetFragments()}";
        }
    }
}
