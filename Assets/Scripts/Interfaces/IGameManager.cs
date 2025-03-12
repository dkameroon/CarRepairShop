using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    void AddMoney(int amount);
    bool SpendMoney(int amount);
    int GetMoney();
}
