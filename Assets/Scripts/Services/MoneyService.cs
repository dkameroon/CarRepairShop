using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyService : IMoneyService
{

    private int _money;
    
    public MoneyService(int initialMoney)
    {
        _money = initialMoney;
    }
    
    public int GetMoney() => _money;
    
    public bool TrySpend(int amount)
    {
        if (_money >= amount)
        {
            _money -= amount;
            return true;
        }
        return false;
    }
    
    public void AddMoney(int amount)
    {
        _money += amount;
    }
}
