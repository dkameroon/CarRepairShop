using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiftService
{
    void BuyAndPlaceLift(int liftCost);
    List<ILift> GetLifts();
    bool AllLiftsPurchased();
    int GetPurchasedLiftsCount();
}
