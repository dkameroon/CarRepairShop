using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField] private int initialMoney;
    [SerializeField] private List<GameObject> carPrefabs;

    [SerializeField] private UpgradesDatabase upgradesDatabase;
    
    private ICarService _carService;
    
    private ICurrencyService _currencyService;
    private ILiftService _liftService;
    private IMechanicService _mechanicService;

    private UIManager _uiManager;

    private void Awake()
    {
        _mechanicService = new MechanicService(this, FindObjectOfType<GameBootstrapper>().MechanicPrefab);
        _liftService = new LiftService(this, FindObjectOfType<GameBootstrapper>().LiftPrefab);
    }

    private void Start()
    {
        
        _uiManager = FindObjectOfType<UIManager>();
        _carService = new CarService(
            carPrefabs, 
            new List<Vector3> { new Vector3(3, 0, -65), new Vector3(-3, 0, 25) },
            new List<Vector3> { new Vector3(-32, 0, -64) },
            new List<Quaternion> { Quaternion.identity, Quaternion.Euler(0, 180, 0) },
            _liftService);    
        
        _uiManager.Initialize(_liftService);
        _uiManager.UpdateMoneyText(GetMoney());
        _uiManager.UpdateFragmentText(GetFragments());
        _carService.SpawnCar();
        StartCoroutine(SpawnCarsRoutine());
    }

    public void SetCurrencyService(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }
    
    public IInventory GetInventory()
    {
        return FindObjectOfType<InventoryUI>().GetInventory();
    }

    public IMechanicService GetMechanicService() => _mechanicService;
    public ILiftService GetLiftService() => _liftService;
    private IEnumerator SpawnCarsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            _carService.SpawnCar();
        }
    }

    public void AddMoney(int amount)
    {
        _currencyService.AddMoney(amount);
        _uiManager.UpdateMoneyText(GetMoney());
    }
    
    public void AddFragments(int amount)
    {
        _currencyService.AddFragments(amount);
        _uiManager.UpdateFragmentText(GetFragments());
    }

    public bool SpendMoney(int amount)
    {
        if (_currencyService.GetMoney() >= amount)
        {
            _currencyService.AddMoney(-amount);
            _uiManager.UpdateMoneyText(GetMoney());
            return true;
        }
        return false;
    }
    
    public bool SpendFragments(int amount)
    {
        if (_currencyService.GetFragments() >= amount)
        {
            _currencyService.AddFragments(-amount);
            _uiManager.UpdateFragmentText(GetFragments());
            return true;
        }
        return false;
    }

    public int GetMoney() => _currencyService.GetMoney();
    public int GetFragments() => _currencyService.GetFragments();
}