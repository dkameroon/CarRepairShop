public interface IInventory
{
    int GetItemCount(CarParts partType);
    int GetPartCost(CarParts part);
    void RemoveItem(CarParts partType, int amount);
    void AddItem(CarParts part, int count);
    bool HasItem(CarParts part, int count);
}