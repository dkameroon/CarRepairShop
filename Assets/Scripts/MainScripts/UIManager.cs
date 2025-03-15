using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Button buyLiftButton;

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
    }

    public void Initialize(ILiftService liftService)
    {
        _liftService = liftService;
        buyLiftButton.onClick.AddListener(() => _liftService.BuyAndPlaceLift());
    }

    public void UpdateMoneyText(int amount)
    {
        moneyText.text = "" + amount;
    }
}
