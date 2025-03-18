using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LiftService : ILiftService
{
    private IGameManager _gameManager;
    private GameObject _liftPrefab;
    private List<ILift> _lifts = new List<ILift>();

    private List<Vector3> _liftPositions;
    private List<Quaternion> _liftRotations;
    private int _nextLiftIndex = 1;

    public LiftService(IGameManager gameManager, GameObject liftPrefab)
    {
        _gameManager = gameManager;
        _liftPrefab = liftPrefab;

        _liftPositions = new List<Vector3>
        {
            new Vector3(-40, 0, -30),
            new Vector3(-40, 0, -23),
            new Vector3(-40, 0, -16),
            new Vector3(-40, 0, -9),
            new Vector3(-25, 0, -33),
            new Vector3(-18, 0, -33)
        };

        _liftRotations = new List<Quaternion>
        {
            Quaternion.Euler(0, 90, 0),
            Quaternion.Euler(0, 90, 0),
            Quaternion.Euler(0, 90, 0),
            Quaternion.Euler(0, 90, 0),
            Quaternion.identity,
            Quaternion.identity
        };

        LoadLifts();
    }
    
    
    private void LoadLifts()
    {
        int purchasedLifts = SaveSystem.Load().LiftsPurchased;

        purchasedLifts = Mathf.Min(purchasedLifts, _liftPositions.Count); 

        for (int i = 0; i < purchasedLifts; i++)
        {
            GameObject lift = Object.Instantiate(_liftPrefab, _liftPositions[i], _liftRotations[i]);
            _lifts.Add(lift.GetComponent<ILift>());
        }

        _nextLiftIndex = purchasedLifts;
    }
    public void BuyAndPlaceLift(int liftCost)
    {
        if (AllLiftsPurchased())
        {
            Debug.Log("All lifts have been purchased!");
            return;
        }

        if (_gameManager.GetMoney() >= liftCost)
        {
            _gameManager.SpendMoney(liftCost);
            GameObject newLift = Object.Instantiate(_liftPrefab, _liftPositions[_nextLiftIndex], _liftRotations[_nextLiftIndex]);
            _lifts.Add(newLift.GetComponent<ILift>());

            _nextLiftIndex++;
            GameData.Instance.LiftsPurchased = _nextLiftIndex;
            GameData.Instance.Save();
        }
        else
        {
            Debug.Log("Not enough money to buy this lift!");
        }
    }


    public bool AllLiftsPurchased()
    {
        return _nextLiftIndex >= _liftPositions.Count;
    }

    public int GetPurchasedLiftsCount()
    {
        return _lifts.Count;
    }

    public List<ILift> GetLifts()
    {
        return _lifts;
    }
}
