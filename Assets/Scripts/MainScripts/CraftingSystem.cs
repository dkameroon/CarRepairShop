using System;
using UnityEngine;

public class CraftingSystem : ICraftingSystem
{
    private readonly ICurrencyService _currencyService;
    private readonly IInventory _inventory;
    private readonly CraftingItemsDatabase _craftingItemsDatabase;

    public event Action OnCraftingSuccess;
    public event Action OnCraftingFailed;

    public CraftingSystem(ICurrencyService currencyService, IInventory inventory, CraftingItemsDatabase craftingItemsDatabase)
    {
        _currencyService = currencyService;
        _inventory = inventory;
        _craftingItemsDatabase = craftingItemsDatabase;
    }

    public bool TryCraft(CarParts part)
    {
        CraftableItem craftableItem = Array.Find(_craftingItemsDatabase.craftableItems, item => item.partType == part);

        if (craftableItem == null)
        {
            OnCraftingFailed?.Invoke();
            return false;
        }

        if (!_currencyService.TrySpendFragments(craftableItem.requiredFragments))
        {
            OnCraftingFailed?.Invoke();
            return false;
        }

        _inventory.AddItem(part, 1);
        OnCraftingSuccess?.Invoke();
        return true;
    }



}