using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    private IGameManager _gameManager;

    public void Initialize(IGameManager gameManager)
    {
        _gameManager = gameManager;
        UpdateMoneyUI();
    }

    public void UpdateMoneyUI()
    {
        moneyText.text = "Money: " + _gameManager.GetMoney();
    }

    
}
