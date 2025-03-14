using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarService : ICarService
{
    private List<GameObject> _carPrefabs;
    private List<Vector3> _spawnPoints;
    private List<Vector3> _finishPoints;
    private ILiftService _liftService;

    public CarService(List<GameObject> carPrefabs, List<Vector3> spawnPoints, List<Vector3> finishPoints, List<Quaternion> spawnRotations, ILiftService liftService)
    {
        _carPrefabs = carPrefabs;
        _spawnPoints = spawnPoints;
        _finishPoints = finishPoints;
        _liftService = liftService;
    }

    public void SpawnCar()
    {
        if (_spawnPoints.Count == 0)
        {
            return;
        }

        GameObject selectedCarPrefab = _carPrefabs[UnityEngine.Random.Range(0, _carPrefabs.Count)];
        Vector3 spawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)];
        Vector3 finishPoint = _finishPoints[UnityEngine.Random.Range(0, _finishPoints.Count)];
        Quaternion spawnRotation = spawnPoint == new Vector3(-3, 0, 25) ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

        GameObject newCar = UnityEngine.Object.Instantiate(selectedCarPrefab, spawnPoint, spawnRotation);
        NavMeshAgent agent = newCar.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            ILift availableLift = FindAvailableLift();
            if (availableLift != null)
            {
                agent.SetDestination(availableLift.GetPosition());
                newCar.GetComponent<Car>().SetDestination(availableLift);
                newCar.GetComponent<Car>().SetDestination(finishPoint);
            }
            else
            {
                agent.SetDestination(spawnPoint + new Vector3(0, 0, 100));
                newCar.GetComponent<Car>().SetDestination(null);
            }
        }
    }

    private ILift FindAvailableLift()
    {
        foreach (ILift lift in _liftService.GetLifts())
        {
            if (!lift.IsOccupied)
                return lift;
        }
        return null;
    }
}
