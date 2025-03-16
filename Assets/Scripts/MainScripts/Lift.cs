using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lift : MonoBehaviour, ILift
{
    public bool IsOccupied { get; private set; } = false;
    private GameObject _liftObject;
    public CarParts RepairedPart { get; private set; }

    [SerializeField] private GameObject progressBarCanvasPrefab;
    private GameObject progressBarCanvas;
    private Slider progressBar;
    
    [SerializeField] private GameObject _messageBoxCanvasPrefab;
    private GameObject _messageBoxCanvas;
    private Image _iconImage;

    public Lift(GameObject liftObject, CarParts repairedPart)
    {
        _liftObject = liftObject;
        RepairedPart = repairedPart;
    }

    private void Start()
    {
        progressBarCanvas = Instantiate(progressBarCanvasPrefab, transform.position + Vector3.up * 10f, Quaternion.Euler(45f, -90f, 0f));
        _messageBoxCanvas = Instantiate(_messageBoxCanvasPrefab, transform.position + Vector3.up * 10f, Quaternion.Euler(45f, -90f, 0f));
        _messageBoxCanvas.SetActive(false);
        progressBar = progressBarCanvas.GetComponentInChildren<Slider>();
        progressBarCanvas.SetActive(false);
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

    public void StartRepair(CarPartData part, float repairTime)
    {
        progressBarCanvas.SetActive(true);
        StartCoroutine(RepairCoroutine(part, repairTime));
    }
    
    public void ShowMessageBox(CarPartData part)
    {
        
        GameObject messageBox = _messageBoxCanvas.transform.Find("MessageBox").gameObject;
        _messageBoxCanvas.SetActive(true);

        
        Transform iconTransform = messageBox.transform.Find("Icon");
        if (iconTransform != null)
        {
           
            Image icon = iconTransform.GetComponent<Image>();
            if (icon != null)
            {
               
                if (part.icon != null)
                {
                    icon.sprite = part.icon;
                }
                else
                {
                    Debug.LogError("Иконка для детали не назначена!");
                }
            }
            else
            {
                Debug.LogError("Компонент Image не найден на Icon!");
            }
        }
        else
        {
            Debug.LogError("Icon не найден в MessageBox!");
        }
    }


    public void HideMessageBox()
    {
        _messageBoxCanvas.SetActive(false);
    }

    private IEnumerator RepairCoroutine(CarPartData part, float repairTime)
    {
        for (float timer = 0; timer < repairTime; timer += Time.deltaTime)
        {
            float progress = timer / repairTime;
            progressBar.value = progress;
            yield return null;
        }

        progressBarCanvas.SetActive(false);
    }
}
