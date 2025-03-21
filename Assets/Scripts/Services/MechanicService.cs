using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MechanicService : IMechanicService
{
    private IGameManager _gameManager;
    private GameObject _mechanicPrefab;
    private List<IMechanic> _mechanics = new List<IMechanic>();

    private List<Vector3> _mechanicPositions;
    private List<Quaternion> _mechanicRotations;
    private int _nextMechanicIndex = 0;

    public MechanicService(IGameManager gameManager, GameObject mechanicPrefab)
    {
        _gameManager = gameManager;
        _mechanicPrefab = mechanicPrefab;

        _mechanicPositions = new List<Vector3>
        {
            new Vector3(-23f, 2f,-12.5f),
            new Vector3(-20f, 2f,-10f),
            new Vector3(-20f, 2f,-15f),
            new Vector3(-15f, 2f,-10f),
            new Vector3(-15f, 2f,-15f),
            new Vector3(-12.5f, 2f,-12.5f)
        };

        _mechanicRotations = new List<Quaternion>
        {
            Quaternion.identity,
            Quaternion.identity,
            Quaternion.identity,
            Quaternion.identity,
            Quaternion.identity,
            Quaternion.identity
        };

        LoadMechanics();
    }

    private void LoadMechanics()
    {
        int purchasedMechanics = SaveSystem.Load().MechanicsHired;
        purchasedMechanics = Mathf.Min(purchasedMechanics, _mechanicPositions.Count);

        for (int i = 0; i < purchasedMechanics; i++)
        {
            GameObject mechanic = Object.Instantiate(_mechanicPrefab, _mechanicPositions[i], _mechanicRotations[i]);

            _mechanics.Add(mechanic.GetComponent<IMechanic>());
        }

        _nextMechanicIndex = purchasedMechanics;
    }

    
    public IMechanic GetAvailableMechanic()
    {
        foreach (var mechanic in _mechanics)
        {
            if (!mechanic.IsBusy)
            {
                return mechanic;
            }
        }
        return null;
    }


    public void BuyAndSpawnMechanic(int mechanicCost)
    {
        if (AllMechanicsHired())
        {
            Debug.Log("All mechanics have been hired!");
            return;
        }

        if (_gameManager.GetMoney() >= mechanicCost)
        {
            _gameManager.SpendMoney(mechanicCost);
            GameObject mechanic = Object.Instantiate(_mechanicPrefab, _mechanicPositions[_nextMechanicIndex], _mechanicRotations[_nextMechanicIndex]);
            _mechanics.Add(mechanic.GetComponent<IMechanic>());

            _nextMechanicIndex++;
            GameData.Instance.MechanicsHired = _nextMechanicIndex;
            GameData.Instance.Save();
        }
        else
        {
            Debug.Log("Not enough money to hire a mechanic!");
        }
    }

    public void AddMechanic(IMechanic mechanic)
    {
        _mechanics.Add(mechanic);
    }
    public bool AllMechanicsHired()
    {
        return _nextMechanicIndex >= _mechanicPositions.Count;
    }

    public int GetHiredMechanicsCount()
    {
        return _mechanics.Count;
    }

    public List<IMechanic> GetMechanics()
    {
        return _mechanics;
    }
}
