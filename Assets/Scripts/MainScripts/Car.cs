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

    private void HandlePartBought(CarParts partType)
    {
        if (isWaitingForPart && requiredPartType == partType.ToString())
        {
            StopCoroutine(RepairCoroutine());
            StartCoroutine(RepairCoroutine());
        }
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
            transform.SetPositionAndRotation(_targetLift.GetPosition(), _targetLift.GetRotation());
            StartCoroutine(RepairCoroutine());
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

    
    private IEnumerator RepairCoroutine()
    {
        IUpgradeService upgradeService = UpgradeService.Instance;
        CarPartData requiredPart = carPartsDatabase.carParts.Find(p => p.partType.ToString() == requiredPartType);
        
        if (requiredPart != null)
        {
            int itemCount = _inventory.GetItemCount(requiredPart.partType);

            if (itemCount <= 0)
            {
                isWaitingForPart = true;
                _targetLift.ShowMessageBox(requiredPart);

                RepairQueueManager.Instance.AddToQueue(requiredPart.partType, _targetLift);

                while (itemCount <= 0)
                {
                    itemCount = _inventory.GetItemCount(requiredPart.partType);

                    if (itemCount > 0)
                    {
                        bool wasRemoved = _inventory.RemoveItem(requiredPart.partType, 1);
                        if (wasRemoved)
                        {
                            FindObjectOfType<InventoryUI>().UpdateInventoryUI();
                            break;
                        }
                    }

                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                bool wasRemoved = _inventory.RemoveItem(requiredPart.partType, 1);
                if (wasRemoved)
                {
                    Debug.Log($"✅ Detail {requiredPart.partType} is in inventory, starting repairing!");
                }
                else
                {
                    yield break;
                }
            }
            float adjustedRepairTime = requiredPart.repairTime / upgradeService.GetRepairSpeedMultiplier();
            _targetLift.HideMessageBox();
            _targetLift.StartRepair(requiredPart, adjustedRepairTime);
            yield return new WaitForSeconds(adjustedRepairTime);
        }
        else
        {
            yield break;
        }

        int adjustedReward = Mathf.RoundToInt(Random.Range(requiredPart.purchaseCost, requiredPart.repairReward) * upgradeService.GetProfitMultiplier());
        FindObjectOfType<GameManager>().AddMoney(adjustedReward);
        SoundEffectsManager.Instance.PlaySound("Money");
        DropFragments();
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

    private void DropFragments()
    {
        float chance = Random.Range(0f, 1f);

        if (chance < 0.5f)
        {
            int fragmentsAmount = Random.Range(1, 5);
            FindObjectOfType<GameManager>().AddFragments(fragmentsAmount);
            Debug.Log($"Recieved {fragmentsAmount} fragments!");
            _targetLift.ShowFragmentsPopup(fragmentsAmount, _targetLift.GetPosition());
        }
    }

}
