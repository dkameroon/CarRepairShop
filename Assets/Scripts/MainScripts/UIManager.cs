using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _fragmentsText;
    
    [SerializeField] private GameObject notificationPopupPrefab;
    private GameObject notificationPopup;

    private bool isNotificationActive = false;

    private ILiftService _liftService;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        notificationPopup = Instantiate(notificationPopupPrefab, FindObjectOfType<Canvas>().transform);
        notificationPopup.SetActive(false);
    }
    
    public void ShowNotification(string message)
    {
        if (isNotificationActive)
        {
            Debug.Log("Notification already active. Skipping new one.");
            return;
        }

        isNotificationActive = true;

        notificationPopup.SetActive(true); 

        Animator animator = notificationPopup.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Rebind(); // Сбрасываем анимацию
            animator.Play("NotificationPopup", 0, 0); // Воспроизводим анимацию
        }
        else
        {
            Debug.LogError("Animator component missing in the notification prefab.");
        }

        MoneyPopup moneyPopup = notificationPopup.GetComponent<MoneyPopup>();
        if (moneyPopup != null)
        {
            moneyPopup.ShowPopupText(message);
        }

        float animationDuration = animator != null ? animator.GetCurrentAnimatorStateInfo(0).length : 2f;

        StartCoroutine(WaitForNotification(animationDuration));
    }

    private IEnumerator WaitForNotification(float duration)
    {
        yield return new WaitForSeconds(duration); 

        notificationPopup.SetActive(false);

        isNotificationActive = false;
    }

    public void Initialize(ILiftService liftService)
    {
        _liftService = liftService;
    }

    public void UpdateMoneyText(int amount)
    {
        _moneyText.text = "" + amount;
    }
    
    public void UpdateFragmentText(int amount)
    {
        _fragmentsText.text = "" + amount;
    }
}
