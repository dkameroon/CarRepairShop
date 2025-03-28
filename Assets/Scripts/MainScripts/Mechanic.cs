using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Mechanic : MonoBehaviour, IMechanic
{
    [SerializeField] private NavMeshAgent _agent;
    private Vector3 _spawnPoint;
    private ILift _currentLift;
    private Animator _animator;
    public bool IsBusy { get; private set; }
    
    private IInventory _inventory;
    

    private static readonly int WalkHash = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _spawnPoint = transform.position;
        _animator = GetComponent<Animator>();

        _agent.stoppingDistance = 0.5f;
    }

    private void Start()
    {
        _inventory = GameBootstrapper.instance.GetInventory();
    }

    private void Update()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.isStopped = true;
            _animator.SetBool(WalkHash, false);
        }
    }

    public void MoveToLift(ILift lift)
    {
        if (_agent != null)
        {
            _animator.SetBool(WalkHash, true);
            _agent.isStopped = false;
            _agent.SetDestination(lift.GetForwardPosition() * 6 + lift.GetPosition());
            IsBusy = true;
            _currentLift = lift;
            var requiredPart = _currentLift.GetCurrentCar()?.GetRequiredPartData();
            if (requiredPart != null && _inventory.HasPart(requiredPart.partType))
            {
                _currentLift.StartRepair(requiredPart, requiredPart.repairTime, this);
            }
        }
    }

    public void MoveToSpawn()
    {
        if (_spawnPoint != null)
        {
            IsBusy = false;
            _animator.SetBool(WalkHash, true);
            _agent.isStopped = false;
            _agent.SetDestination(_spawnPoint);
        }
        else
        {
            Debug.LogError("Spawn point not set for mechanic!");
        }
        }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentLift != null && other.gameObject == _currentLift.GetGameObject() || other.CompareTag("Car"))
        {
            Debug.Log("🔧 Механик прибыл к подъемнику, начинаем ремонт.");
            _animator.SetBool(WalkHash, false);
        }
    }

    public void CompleteRepair()
    {
        _currentLift.SetIsRepairInProgress(false);
        IsBusy = false; 
        MoveToSpawn();
    }


    public void StartRepair(ILift lift)
    {
        if (lift == null) return;

        IsBusy = true;
        Car car = lift.GetCurrentCar();

        if (car != null)
        {
            var requiredPart = car.GetRequiredPartData();

            if (_inventory.HasPart(requiredPart.partType))
            {
                _inventory.RemoveItem(requiredPart.partType, 1);
                _animator.SetBool(WalkHash, false);
                StartCoroutine(RepairCoroutine(lift, requiredPart.repairTime));
            }
            else
            {
                Debug.Log("❌ Деталь отсутствует, механик уходит!");
                MoveToSpawn();
            }
        }
    }

    private IEnumerator RepairCoroutine(ILift lift, float repairTime)
    {
        yield return new WaitForSeconds(repairTime);

        _inventory.RemoveItem(lift.GetCurrentCar().GetRequiredPartData().partType, 1);
        lift.StartRepair(lift.GetCurrentCar().GetRequiredPartData(), repairTime);
        
        _animator.SetBool(WalkHash, true);
        MoveToSpawn();
    }
}
