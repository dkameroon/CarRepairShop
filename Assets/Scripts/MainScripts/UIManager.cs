using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI moneyText;

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
    }

    public void UpdateMoneyText(int amount)
    {
        moneyText.text = "" + amount;
    }
}
