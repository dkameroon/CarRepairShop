/*using System.Collections;
using UnityEngine;

public class Mechanic : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Lift _targetLift;
    private bool isWaitingForPart = false;
    private string requiredPartType;

    public void StartRepairProcess(string partType, Lift lift)
    {
        _targetLift = lift;
        requiredPartType = partType;
        StartCoroutine(RepairCoroutine());
    }

    private IEnumerator RepairCoroutine()
    {
        CarPartData requiredPart = _inventory.GetPartData(requiredPartType);

        if (requiredPart != null)
        {
            int itemCount = _inventory.GetItemCount(requiredPart.partType);

            if (itemCount <= 0)
            {
                isWaitingForPart = true;
                _targetLift.ShowMessageBox(requiredPart);

                // Добавляем подъёмник в очередь на деталь
                if (!RepairQueueManager.Instance.IsLiftInQueue(requiredPart.partType, _targetLift))
                {
                    RepairQueueManager.Instance.AddToQueue(requiredPart.partType, _targetLift);
                }

                while (itemCount <= 0)
                {
                    itemCount = _inventory.GetItemCount(requiredPart.partType);
                    yield return new WaitForSeconds(1f);
                }

                _inventory.RemoveItem(requiredPart.partType, 1);
                FindObjectOfType<InventoryUI>().UpdateInventoryUI();
                Debug.Log($"✅ Деталь {requiredPart.partType} куплена, начинаем починку!");
            }

            _targetLift.StartRepair(requiredPart.partType);
        }
    }
}*/