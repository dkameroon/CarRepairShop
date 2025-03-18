using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _fragmentsText;
    

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
        _moneyText.text = "" + amount;
    }
    
    public void UpdateFragmentText(int amount)
    {
        _fragmentsText.text = "" + amount;
    }
}
