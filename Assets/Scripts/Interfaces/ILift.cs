using UnityEngine;

public interface ILift 
{
    Vector3 GetPosition();
    Quaternion GetRotation();
    bool IsOccupied { get; }
    void SetOccupied(bool state);
    GameObject GetGameObject();
    CarParts RepairedPart { get; }
    void StartRepair(CarPartData part, float repairTime);
    void ShowMessageBox(CarPartData part);
    void HideMessageBox();
    Car GetCurrentCar();
    void ShowFragmentsPopup(int amount, Vector3 liftPosition);

}
