using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    private IMoneyService _moneyService;
    private UIManager _uiManager;
    
    void Awake()
    {
        _moneyService = new MoneyService(100);
    }

    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _uiManager.Initialize(this);
    }


    public void AddMoney(int amount)
    {
        _moneyService.AddMoney(amount);
    }

    public bool SpendMoney(int amount)
    {
        throw new System.NotImplementedException();
    }

    public int GetMoney() => _moneyService.GetMoney();

}
