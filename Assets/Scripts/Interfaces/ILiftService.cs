using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiftService
{
    void BuyAndPlaceLift();
    List<ILift> GetLifts();
}
