using System;
using System.Collections.Generic;

public interface ICraftingSystem
{
    event Action OnCraftingSuccess;
    event Action OnCraftingFailed;
    bool TryCraft(CarParts part);
}