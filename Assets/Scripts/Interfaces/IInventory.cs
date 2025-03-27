public interface IInventory
{
    int GetItemCount(CarParts partType);
    int GetPartCost(CarParts part);
    bool RemoveItem(CarParts part, int count);
    void AddItem(CarParts part, int count);
    bool HasItem(CarParts part, int count);
    float GetRepairTime(CarParts part);

    bool HasPart(CarParts part);
}