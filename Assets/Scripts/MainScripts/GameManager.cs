using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField] private int initialMoney;
    
    private IMoneyService _moneyService;
    private ILiftService _liftService;
    private UIManager _uiManager;

    private void Awake()
    {
        _moneyService = new MoneyService(initialMoney);
    }

    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _liftService = new LiftService(this, FindObjectOfType<GameBootstrapper>().LiftPrefab);

        _uiManager.Initialize(_liftService);
        _uiManager.UpdateMoneyText(GetMoney());
    }

    public void AddMoney(int amount)
    {
        _moneyService.AddMoney(amount);
        _uiManager.UpdateMoneyText(GetMoney());
    }

    public bool SpendMoney(int amount)
    {
        if (_moneyService.GetMoney() >= amount)
        {
            _moneyService.AddMoney(-amount);
            _uiManager.UpdateMoneyText(GetMoney());
            return true;
        }
        return false;
    }

    public int GetMoney() => _moneyService.GetMoney();
}