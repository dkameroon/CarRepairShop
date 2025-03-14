using UnityEngine;

public class Lift : MonoBehaviour, ILift
{
    public bool IsOccupied { get; private set; } = false;
    private GameObject _liftObject;
    public CarParts RepairedPart { get; private set; } 

    public Lift(GameObject liftObject, CarParts repairedPart)
    {
        _liftObject = liftObject;
        RepairedPart = repairedPart;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            IsOccupied = true;
        }
    }


    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public Quaternion GetRotation()
    {
        return transform.rotation * Quaternion.Euler(0f, 180f, 0f);
    }


    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}