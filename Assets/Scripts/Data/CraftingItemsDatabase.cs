using UnityEngine;

[CreateAssetMenu(fileName = "CraftingItemsDatabase", menuName = "Crafting/Crafting Items Database")]
public class CraftingItemsDatabase : ScriptableObject
{
    public CraftableItem[] craftableItems;
}

[System.Serializable]
public class CraftableItem
{
    public CarParts partType;
    public string itemName;
    public int requiredFragments;
    public Sprite itemIcon;
}
