using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    public static GameBootstrapper instance;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject _liftPrefab;
    [SerializeField] private GameObject _mechanicPrefab;

    [Header("Scripts and Databases")]
    [SerializeField] private UpgradesDatabase _upgradesDatabase;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private CraftingItemsDatabase _craftingDatabase;
    [SerializeField] private CraftingUI _craftingUI;
    [SerializeField] private CarPartsDatabase _carPartsDatabase;

    private UIManager _uiManager;
    private UpgradeUI _upgradeUI;
    private ILiftService _liftService;
    private IMechanicService _mechanicService;
    private IInventory _inventory;
    public ICurrencyService CurrencyService;

    public GameObject LiftPrefab => _liftPrefab;
    public GameObject MechanicPrefab => _mechanicPrefab;

    private void Awake()
    {
        instance = this;
        CurrencyService = new CurrencyService(SaveSystem.Load().Money, SaveSystem.Load().Fragments);
        _gameManager.SetCurrencyService(CurrencyService);
        new GameData(SaveSystem.Load().Money, SaveSystem.Load().Fragments, SaveSystem.Load().LiftsPurchased, SaveSystem.Load().MechanicsHired,SaveSystem.Load().upgrades, SaveSystem.Load().inventory);
    
        
        _inventory = new Inventory(_carPartsDatabase);
        ICraftingSystem craftingSystem = new CraftingSystem(CurrencyService, _inventory, _craftingDatabase);

        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>(); 

        if (inventoryUI == null)
        {
            Debug.LogError("InventoryUI не найден в сцене!");
        }
        else
        {
            _craftingUI.Initialize(CurrencyService, craftingSystem, _craftingDatabase, inventoryUI);
        }

        _uiManager = FindObjectOfType<UIManager>();
        _upgradeUI = FindObjectOfType<UpgradeUI>();

        if (_upgradeUI == null)
        {
            Debug.LogError("UpgradeUI не найден в сцене!");
        }
        else
        {
            _liftService = _gameManager.GetLiftService();
            _mechanicService = _gameManager.GetMechanicService();
            UpgradeService upgradeService = new UpgradeService(_gameManager, _upgradesDatabase, _liftService, _mechanicService);
            _upgradeUI.Initialize(_gameManager, _upgradesDatabase, upgradeService);
            upgradeService.SetUpgradeUI(_upgradeUI);
        }
    }

    public IInventory GetInventory()
    {
        return _inventory;
    }
    
}

