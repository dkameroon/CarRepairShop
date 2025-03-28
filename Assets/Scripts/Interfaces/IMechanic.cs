using UnityEngine;

public interface IMechanic
{
    void MoveToLift(ILift lift);
    void MoveToSpawn();
    void StartRepair(ILift lift);
    bool IsBusy { get; }
    void CompleteRepair();
}
