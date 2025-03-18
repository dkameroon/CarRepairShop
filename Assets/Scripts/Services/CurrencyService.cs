using UnityEngine;

public class CurrencyService : ICurrencyService
{
    private int _money;
    private int _fragments;

    public CurrencyService(int Money, int Fragments)
    {
        _money = Money;
        _fragments = Fragments;
    }
    public int GetMoney() => _money;

    public bool TrySpendMoney(int amount)
    {
        if (_money >= amount)
        {
            AddMoney(-amount);
            return true;
        }
        return false;
    }
    
    public bool TrySpendFragments(int amount)
    {
        if (_fragments >= amount)
        {
            AddFragments(-amount);
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        _money += amount;
        GameData.Instance.SaveMoney(_money);
    }

    public int GetFragments() => _fragments;

    public void AddFragments(int amount)
    {
        _fragments += amount;
        Debug.Log($"Fragments updated : " +_fragments);
        GameData.Instance.SaveFragments(_fragments);
    }
}