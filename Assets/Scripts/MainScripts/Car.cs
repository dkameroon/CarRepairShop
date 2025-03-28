using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Car : MonoBehaviour
{
    private NavMeshAgent _agent;
    private ILift _targetLift;
    private Vector3 finishPosition;
    private bool _isRepaired = false;
    private GameManager _gameManager;
    private CarPartsDatabase carPartsDatabase;
    private IInventory _inventory;

    public bool isWaitingForPart = false;
    private string requiredPartType;

    public void SetPartsDatabase(CarPartsDatabase database)
    {
        carPartsDatabase = database;
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _gameManager = FindObjectOfType<GameManager>();
        carPartsDatabase = Resources.Load<CarPartsDatabase>("CarPartsDatabase");
        _inventory = FindObjectOfType<InventoryUI>().GetInventory();

        SetRandomBreakdown();
    }

    private void SetRandomBreakdown()
    {
        if (carPartsDatabase != null && carPartsDatabase.carParts.Count > 0)
        {
            requiredPartType = carPartsDatabase.carParts[Random.Range(0, carPartsDatabase.carParts.Count)].partType.ToString();
        }
    }

    public void SetDestination(ILift targetLift)
    {
        if (targetLift == null || targetLift.IsReserved)
        {
            DrivePast();
            return;
        }
        _targetLift = targetLift;
        _agent.SetDestination(_targetLift.GetPosition());
        _targetLift.SetReserved(true);
    }

    public void SetDestination(Vector3 targetPosition)
    {
        finishPosition = targetPosition;
    }

    private void DrivePast()
    {
        Vector3 drivePastDestination = transform.position + transform.forward * 100;
        _agent.SetDestination(drivePastDestination);
        Destroy(gameObject, 15f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isRepaired) return;

        if (other.gameObject == _targetLift?.GetGameObject())
        {
            _agent.enabled = false;
            transform.SetPositionAndRotation(_targetLift.GetPosition(), _targetLift.GetRotation());
        }
    }

    public void StartRepairWithPart(CarPartData partData)
    {
        _inventory.RemoveItem(partData.partType, 1);
        _targetLift.StartRepair(partData, partData.repairTime);
        RepairQueueManager.Instance.MarkRepairStarted(partData.partType);
    }

    public CarPartData GetRequiredPartData()
    {
        return carPartsDatabase.carParts.Find(p => p.partType.ToString() == requiredPartType);
    }
    
    public IEnumerator GoToFinish()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        _agent.enabled = true;
        _isRepaired = true;
        transform.Rotate(0f, 180f, 0f);
        _agent.SetDestination(finishPosition);
        if (_isRepaired)
        {
            _targetLift.SetOccupied(false);
        }

        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
    
}

