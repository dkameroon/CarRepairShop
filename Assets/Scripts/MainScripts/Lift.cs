using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Lift : MonoBehaviour, ILift
{
    public bool IsOccupied { get; private set; } = false;
    public bool IsReserved { get; private set; } = false;
    private GameObject _liftObject;
    public CarParts RepairedPart { get; private set; }

    [FormerlySerializedAs("liftCanvasPrefab")]
    [Header("Prefabs")]
    [SerializeField] private GameObject _liftCanvasPrefab;
    [SerializeField] private GameObject _moneyPopupPrefab;
    [SerializeField] private GameObject _fragmentsPopupPrefab;
    
    private GameObject liftCanvas;
    private Slider progressBar;
    private GameObject messageBox;
    private Image _iconImage;

    private GameManager _gameManager;
    private Car _currentCar;
    
    private IInventory _inventory;
    
    
    
    public Lift(GameObject liftObject, CarParts repairedPart)
    {
        _liftObject = liftObject;
        RepairedPart = repairedPart;
    }

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        
    }

    private void Start()
    {
        _inventory = GameBootstrapper.instance.GetInventory();
        liftCanvas = Instantiate(_liftCanvasPrefab, transform.position + Vector3.up * 10f, Quaternion.Euler(45f, -90f, 0f));
        progressBar = liftCanvas.transform.Find("ProgressBar").GetComponent<Slider>();
        messageBox = liftCanvas.transform.Find("MessageBox").gameObject;

        if (messageBox != null)
        {
            Transform iconTransform = messageBox.transform.Find("Icon");
            if (iconTransform != null)
            {
                _iconImage = iconTransform.GetComponent<Image>();
            }
        }

        liftCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") && !IsOccupied || other.CompareTag("Car") && IsReserved)
        {
            IsOccupied = true;
            _currentCar = other.GetComponent<Car>();

            if (_currentCar != null)
            {
                var requiredPart = _currentCar.GetRequiredPartData();
                if (_inventory.HasPart(requiredPart.partType))
                {
                    AssignMechanicToLift();
                }
                else
                {
                    Debug.Log("Не хватает нужной детали!");
                    RepairQueueManager.Instance.AddToQueue(requiredPart.partType, this);
                    ShowMessageBox(requiredPart);
                }
            }
        }
    }

    private Mechanic AssignMechanicToLift()
    {
        var mechanics = FindObjectsOfType<Mechanic>();
        foreach (var mechanic in mechanics)
        {
            if (!mechanic.IsBusy)
            {
                mechanic.MoveToLift(this);
                return mechanic;
            }
        }

        Debug.LogWarning("🚧 No!");
        return null;
    }


    
    private void ShowMoneyPopup(int amount, Vector3 liftPosition)
    {
        GameObject popup = Instantiate(_moneyPopupPrefab, liftPosition + Vector3.up * 3f, Quaternion.Euler(45f, -90f, 0f));

        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.position = liftPosition + Vector3.up * 3f;
        }
        
        Animator animator = popup.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Show");
        }

        popup.GetComponent<MoneyPopup>().ShowPopup(amount);

        float animationDuration = animator != null ? animator.GetCurrentAnimatorStateInfo(0).length : 2f;
        Destroy(popup, animationDuration);
    }

    public void ShowFragmentsPopup(int amount, Vector3 liftPosition)
    {
        GameObject popup = Instantiate(_fragmentsPopupPrefab, liftPosition + Vector3.up * 3f, Quaternion.Euler(45f, -90f, 0f));

        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.position = liftPosition + Vector3.up * 3f;
        }
        
        Animator animator = popup.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Show");
        }

        popup.GetComponent<MoneyPopup>().ShowPopup(amount);

        float animationDuration = animator != null ? animator.GetCurrentAnimatorStateInfo(0).length : 2f;
        Destroy(popup, animationDuration);
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public Vector3 GetForwardPosition()
    {
        return transform.forward;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation * Quaternion.Euler(0f, 180f, 0f);
    }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }

    public void SetReserved(bool reserved)
    {
        IsReserved = reserved;
    }
    
    public void StartRepair(CarPartData part, float repairTime)
    {
        var assignedMechanic = AssignMechanicToLift();
        if (assignedMechanic == null) 
        {
            Debug.LogWarning("Немає доступних механіків для ремонту!");
            return;
        }
    
        liftCanvas.SetActive(true);
        messageBox.SetActive(false);
        progressBar.gameObject.SetActive(true);
    
        StartCoroutine(RepairCoroutine(part, repairTime, assignedMechanic));
    }

    private IEnumerator RepairCoroutine(CarPartData part, float repairTime, Mechanic mechanic)
    {
        IUpgradeService upgradeService = UpgradeService.Instance;
        float adjustedRepairTime = part.repairTime / upgradeService.GetRepairSpeedMultiplier();
        progressBar.gameObject.SetActive(true);

        for (float timer = 0; timer < adjustedRepairTime; timer += Time.deltaTime)
        {
            float progress = timer / adjustedRepairTime;
            progressBar.value = progress;
            yield return null;
        }

        progressBar.gameObject.SetActive(false);
        IsOccupied = false;
        IsReserved = false;

        int adjustedReward = Mathf.RoundToInt(Random.Range(part.purchaseCost, part.repairReward) * upgradeService.GetProfitMultiplier());
        ShowMoneyPopup(adjustedReward, transform.position);
        FindObjectOfType<GameManager>().AddMoney(adjustedReward);
        RepairQueueManager.Instance.RemoveFromQueue(part.partType, this);
        mechanic.MoveToSpawn();
        SoundEffectsManager.Instance.PlaySound("Money");
        DropFragments();
        StartCoroutine(_currentCar.GoToFinish());
    }
    
    private void DropFragments()
    {
        float chance = Random.Range(0f, 1f);

        if (chance < 0.5f)
        {
            int fragmentsAmount = Random.Range(1, 5);
            FindObjectOfType<GameManager>().AddFragments(fragmentsAmount);
            Debug.Log($"Отримано {fragmentsAmount} фрагментів!");
            ShowFragmentsPopup(fragmentsAmount, GetPosition());
        }
    }

    public void ShowMessageBox(CarPartData part)
    {
        liftCanvas.SetActive(true);
        progressBar.gameObject.SetActive(false);
        messageBox.SetActive(true);

        if (_iconImage != null)
        {
            _iconImage.sprite = part.icon;
        }
        else
        {
            Debug.LogError("Icon is not found!");
        }
    }


    public Car GetCurrentCar()
    {
        return _currentCar;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void HideMessageBox()
    {
        messageBox.SetActive(false);
    }

    
}
