using System;
using UnityEngine;
using UnityEngine.AI;

public class Mechanic : MonoBehaviour, IMechanic
{
    [SerializeField]private NavMeshAgent _agent;
    private Vector3 _spawnPoint;
    private ILift _currentLift;
    private Animator animator;
    public bool IsBusy { get; private set; } 
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _spawnPoint = transform.position;
        animator = GetComponent<Animator>();
        
        _agent.stoppingDistance = 0.5f;
    }
    
    private void Update()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.isStopped = true;
            animator.SetBool("IsWalking", false);
        }
    }


    public void MoveToLift(ILift lift)
    {
        if (_agent != null)
        {
            animator.SetBool("IsWalking", true);
            _agent.isStopped = false;
            _agent.SetDestination(lift.GetForwardPosition()*6 + lift.GetPosition());
            IsBusy = true;
            _currentLift = lift;
        }
    }


    public void MoveToSpawn()
    {
        IsBusy = false;
        animator.SetBool("IsWalking", true);
        _agent.isStopped = false;
        _agent.SetDestination(_spawnPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentLift != null && other.gameObject == _currentLift.GetGameObject())
        {
            animator.SetBool("IsWalking", false);
            StartRepair(_currentLift);
        }
        else if (!IsBusy && Vector3.Distance(transform.position, _spawnPoint) < 1.5f) 
        {
            animator.SetBool("IsWalking", false);
        }
    }

    public void StartRepair(ILift lift)
    {
        if (lift == null) return;

        IsBusy = true;
        Car car = lift.GetCurrentCar();

        if (car != null)
        {
            var requiredPart = car.GetRequiredPartData();

            if (Inventory.Instance.HasPart(requiredPart.partType))
            {
                lift.StartRepair(requiredPart, requiredPart.repairTime);
                Inventory.Instance.RemoveItem(requiredPart.partType, 1);
                MoveToSpawn();
            }
            else
            {
                Debug.Log("Detail is not found !");
                MoveToSpawn();
            }
        }
    }

}