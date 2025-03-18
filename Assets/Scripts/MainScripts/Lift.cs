using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Lift : MonoBehaviour, ILift
{
    public bool IsOccupied { get; private set; } = false;
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
        if (other.CompareTag("Car"))
        {
            IsOccupied = true;
        }
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

    public Quaternion GetRotation()
    {
        return transform.rotation * Quaternion.Euler(0f, 180f, 0f);
    }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }

    public void StartRepair(CarPartData part, float repairTime)
    {
        liftCanvas.SetActive(true);
        messageBox.SetActive(false);
        progressBar.gameObject.SetActive(true);
        StartCoroutine(RepairCoroutine(part, repairTime));
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

    private IEnumerator RepairCoroutine(CarPartData part, float repairTime)
    {
        IUpgradeService upgradeService = UpgradeService.Instance;
        progressBar.gameObject.SetActive(true);

        for (float timer = 0; timer < repairTime; timer += Time.deltaTime)
        {
            float progress = timer / repairTime;
            progressBar.value = progress;
            yield return null;
        }

        progressBar.gameObject.SetActive(false);
        IsOccupied = false;

        int adjustedReward = Mathf.RoundToInt(Random.Range(part.purchaseCost, part.repairReward) * upgradeService.GetProfitMultiplier());
        ShowMoneyPopup(adjustedReward, transform.position);

        RepairQueueManager.Instance.RemoveFromQueue(part.partType, this);
    }
}
