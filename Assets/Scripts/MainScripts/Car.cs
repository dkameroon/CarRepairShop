using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Car : MonoBehaviour
{
    private NavMeshAgent _agent;
    private ILift _targetLift;
    private ICarService _carService;
    private Vector3 finishPosition;
    private bool _isRepaired = false;
    private GameManager _gameManager;
    private CarPartsDatabase carPartsDatabase;
    private IInventory _inventory;

    private bool isWaitingForPart = false;
    
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
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.OnPartBought += HandlePartBought;
        }

        if (carPartsDatabase == null)
        {
            Debug.LogError("❌ CarPartsDatabase не загружен! Проверьте, находится ли он в Resources.");
        }

        
        SetRandomBreakdown();
    }

    private void HandlePartBought(CarParts partType)
    {
        if (isWaitingForPart && requiredPartType == partType.ToString())
        {
            StopCoroutine("RepairCoroutine");
            StartCoroutine(RepairCoroutine());
        }
    }

    private void SetRandomBreakdown()
    {
        if (carPartsDatabase != null && carPartsDatabase.carParts.Count > 0)
        {
            requiredPartType = carPartsDatabase.carParts[Random.Range(0, carPartsDatabase.carParts.Count)].partType.ToString();
            Debug.Log($"❌ Машина нуждается в починке: {requiredPartType}");
        }
        else
        {
            Debug.LogError("❌ Не удалось случайно выбрать поломку! carPartsDatabase пуст.");
        }
    }

    public void SetDestination(ILift targetLift)
    {
        if (targetLift == null || targetLift.IsOccupied)
        {
            DrivePast();
            return;
        }

        _targetLift = targetLift;
        _agent.SetDestination(_targetLift.GetPosition());
        _targetLift.SetOccupied(true);
    }

    public void SetDestination(Vector3 targetPosition)
    {
        finishPosition = targetPosition;
    }

    private void DrivePast()
    {
        Vector3 drivePastDestination = transform.position + transform.forward * 100;
        _agent.SetDestination(drivePastDestination);
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isRepaired)
        {
            return;
        }
        if (other.gameObject == _targetLift?.GetGameObject())
        {
            _agent.enabled = false;
            Debug.Log("Car arrived at lift!");
            transform.SetPositionAndRotation(_targetLift.GetPosition(), _targetLift.GetRotation());
            
            StartCoroutine(RepairCoroutine());
        }
    }

    private IEnumerator RepairCoroutine()
    {
        CarPartData requiredPart = carPartsDatabase.carParts.Find(p => p.partType.ToString() == requiredPartType);

        if (requiredPart != null)
        {
            int itemCount = _inventory.GetItemCount(requiredPart.partType);

            if (itemCount <= 0)
            {
                isWaitingForPart = true;
                Debug.Log($"❌ Нет детали {requiredPart.partType}, ожидаем покупки.");

                while (itemCount <= 0)
                {
                    itemCount = _inventory.GetItemCount(requiredPart.partType);
                    yield return new WaitForSeconds(1f);
                }

                
                _inventory.RemoveItem(requiredPart.partType, 1);
                Debug.Log($"✅ Деталь {requiredPart.partType} куплена, начинаем починку!");
            }
            else
            {
                _inventory.RemoveItem(requiredPart.partType, 1);
                Debug.Log($"✅ Деталь {requiredPart.partType} уже есть, начинаем починку!");
            }
        }
        else
        {
            yield break; 
        }

        
        yield return new WaitForSeconds(20f);

     
        _agent.enabled = true;
        transform.Rotate(0f, 180f, 0f);
        _agent.SetDestination(finishPosition);
        _targetLift.SetOccupied(false);
        _isRepaired = true;
        
        FindObjectOfType<GameManager>().AddMoney(requiredPart.repairReward);

        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }



    private void OnDestroy()
    {
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.OnPartBought -= HandlePartBought;
        }
    }
}
