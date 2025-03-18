using System.Collections.Generic;
using UnityEngine;

public class RepairQueueManager : MonoBehaviour
{
    private static RepairQueueManager _instance;
    public static RepairQueueManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RepairQueueManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("RepairQueueManager");
                    _instance = obj.AddComponent<RepairQueueManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }
    
    public void MarkRepairStarted(CarParts partType)
    {
        if (repairQueue.ContainsKey(partType) && repairQueue[partType].Count > 0)
        {
            repairQueue[partType].Dequeue();
        }
    }


    private Dictionary<CarParts, Queue<ILift>> repairQueue = new Dictionary<CarParts, Queue<ILift>>();

    public void AddToQueue(CarParts partType, ILift lift)
    {
        if (!repairQueue.ContainsKey(partType))
        {
            repairQueue[partType] = new Queue<ILift>();
        }
        repairQueue[partType].Enqueue(lift);
    }

    public void RemoveFromQueue(CarParts partType, ILift lift)
    {
        if (repairQueue.ContainsKey(partType))
        {
            if (repairQueue[partType].Contains(lift))
            {
                var updatedQueue = new Queue<ILift>();
                foreach (var item in repairQueue[partType])
                {
                    if (item != lift)
                        updatedQueue.Enqueue(item);
                }
                repairQueue[partType] = updatedQueue;
            }
        }
    }

    public bool HasPendingRepairs(CarParts partType)
    {
        return repairQueue.ContainsKey(partType) && repairQueue[partType].Count > 0;
    }

    public ILift GetNextLift(CarParts partType)
    {
        if (repairQueue.ContainsKey(partType) && repairQueue[partType].Count > 0)
        {
            return repairQueue[partType].Peek();
        }
        return null;
    }
}
