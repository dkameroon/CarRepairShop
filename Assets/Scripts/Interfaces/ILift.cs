using UnityEngine;

public interface ILift 
{
    Vector3 GetPosition();
    Vector3 GetForwardPosition();
    Quaternion GetRotation();
    bool IsOccupied { get; }
    bool IsReserved { get; }
    bool IsRepairInProgress { get; }
    void SetIsRepairInProgress(bool isRepairInProgress);
    void SetReserved(bool reserved);
    void SetOccupied(bool state);
    GameObject GetGameObject();
    CarParts RepairedPart { get; }
    void StartRepair(CarPartData part, float repairTime);
    void StartRepair(CarPartData part, float repairTime, IMechanic mechanic);
    void ShowMessageBox(CarPartData part);
    void HideMessageBox();
    Car GetCurrentCar();
    void ShowFragmentsPopup(int amount, Vector3 liftPosition);
    bool NeedsRepair();
    IMechanic AssignMechanicToLift();
}
