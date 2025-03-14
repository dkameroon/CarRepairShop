using UnityEngine;

public interface ILift 
{
    Vector3 GetPosition();
    Quaternion GetRotation();
    bool IsOccupied { get; }
    void SetOccupied(bool state);
    GameObject GetGameObject();
    CarParts RepairedPart { get; }
}
