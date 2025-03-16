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
    
    private IMoneyService _moneyService;
    private ILiftService _liftService;

    private UIManager _uiManager;
    [SerializeField] private CarPartsDatabase carPartsDatabase;
    public CarPartsDatabase GetCarPartsDatabase() => carPartsDatabase; 

    private float _spawnInterval = 20f;
    private float _nextSpawnTime = 0f;

    private void Awake()
    {
        _moneyService = new MoneyService(SaveSystem.Load().Money);

    }

    private void Start()
    {
        _liftService = new LiftService(this, FindObjectOfType<GameBootstrapper>().LiftPrefab);
        _uiManager = FindObjectOfType<UIManager>();
        _carService = new CarService(
            carPrefabs, 
            new List<Vector3> { new Vector3(3, 0, -65), new Vector3(-3, 0, 25) },
            new List<Vector3> { new Vector3(-32, 0, -64) },
            new List<Quaternion> { Quaternion.identity, Quaternion.Euler(0, 180, 0) },
            _liftService);    
        
        _uiManager.Initialize(_liftService);
        _uiManager.UpdateMoneyText(GetMoney());
        _carService.SpawnCar();
        StartCoroutine(SpawnCarsRoutine());
    }

   

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