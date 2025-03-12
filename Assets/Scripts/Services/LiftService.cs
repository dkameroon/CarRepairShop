using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftService : ILiftService
{
    private IGameManager _gameManager;
    private GameObject _liftPrefab;
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
        
        Object.Instantiate(_liftPrefab, _liftPositions[0], _liftRotations[0]);
    }

    public void BuyAndPlaceLift()
    {
        int liftCost = 100;

        if (_nextLiftIndex >= _liftPositions.Count)
        {
            Debug.Log("All lifts have been purchased!");
            return;
        }

        if (_gameManager.GetMoney() >= liftCost)
        {
            _gameManager.SpendMoney(liftCost);
            Object.Instantiate(_liftPrefab, _liftPositions[_nextLiftIndex], _liftRotations[_nextLiftIndex]);
            _nextLiftIndex++;
        }
        else
        {
            Debug.Log("Not enough money to buy this lift!");
        }
    }
}
