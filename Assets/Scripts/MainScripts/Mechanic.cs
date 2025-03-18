using System;
using UnityEngine;
using UnityEngine.AI;

public class Mechanic : MonoBehaviour, IMechanic
{
    [SerializeField]private NavMeshAgent _agent;
    private Vector3 _spawnPoint;
    private ILift _currentLift;
    public bool IsBusy { get; private set; } 
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _spawnPoint = transform.position;
    }


    public void MoveToLift(ILift lift)
    {
        if (_agent != null)
        {
            _agent.SetDestination(lift.GetForwardPosition()*6 + lift.GetPosition());
            IsBusy = true;
            _currentLift = lift;
        }
    }


    public void MoveToSpawn()
    {
        IsBusy = false;
        _agent.SetDestination(_spawnPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentLift != null && other.gameObject == _currentLift.GetGameObject())
        {
            StartRepair(_currentLift);
        }
    }

    public void StartRepair(ILift lift)
    {
        if (lift == null) return;

        IsBusy = true;
        Car car = lift.GetCurrentCar();

        if (car != null)
        {
            var requiredPart = car.GetRequiredPartData();

            if (Inventory.Instance.HasPart(requiredPart.partType))
            {
                lift.StartRepair(requiredPart, requiredPart.repairTime);
                Inventory.Instance.RemoveItem(requiredPart.partType, 1);
                MoveToSpawn();
            }
            else
            {
                Debug.Log("Detail is not found !");
                MoveToSpawn();
            }
        }
    }

}