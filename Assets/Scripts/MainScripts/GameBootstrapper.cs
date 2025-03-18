using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    public static GameBootstrapper instance;
    
    [SerializeField] private GameObject _liftPrefab;
    [SerializeField] private UpgradesDatabase _upgradesDatabase;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private CraftingItemsDatabase _craftingDatabase;
    [SerializeField] private CraftingUI _craftingUI;
    [SerializeField] private CarPartsDatabase _carPartsDatabase;

    private UIManager _uiManager;
    private UpgradeUI _upgradeUI;
    private ILiftService _liftService;
    private IInventory _inventory;

    public GameObject LiftPrefab => _liftPrefab;

    private void Awake()
    {
        instance = this;
        new GameData(SaveSystem.Load().Money, SaveSystem.Load().Fragments, SaveSystem.Load().LiftsPurchased, SaveSystem.Load().upgrades, SaveSystem.Load().inventory);
    
        ICurrencyService currencyService = new CurrencyService(SaveSystem.Load().Money, SaveSystem.Load().Fragments);
        _inventory = new Inventory(_carPartsDatabase);
        ICraftingSystem craftingSystem = new CraftingSystem(currencyService, _inventory, _craftingDatabase);

        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>(); 

        if (inventoryUI == null)
        {
            Debug.LogError("InventoryUI не найден в сцене!");
        }
        else
        {
            _craftingUI.Initialize(currencyService, craftingSystem, _craftingDatabase, inventoryUI);
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
            UpgradeService upgradeService = new UpgradeService(_gameManager, _upgradesDatabase, _liftService);
            _upgradeUI.Initialize(_gameManager, _upgradesDatabase, upgradeService);
            upgradeService.SetUpgradeUI(_upgradeUI);
        }
    }

    public IInventory GetInventory()
    {
        return _inventory;
    }
}

