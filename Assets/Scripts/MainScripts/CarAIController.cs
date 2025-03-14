using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAIController : MonoBehaviour
{
    private NavMeshAgent _agent;
    private bool _hasTarget = false;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Vector3 target)
    {
        _hasTarget = true;
        _agent.SetDestination(target);
    }

    private void Update()
    {
        if (_hasTarget && !_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _hasTarget = false;
            _agent.isStopped = true;
            
            Destroy(gameObject, 3f);
        }
    }
}
