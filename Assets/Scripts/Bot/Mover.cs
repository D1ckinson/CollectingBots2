using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;

    private NavMeshAgent _agent;
    private float _arrivalThreshold = 1f;
    private Vector3 _target;
    private Transform _transformTarget;

    public event Action TargetReached;

    private void Update()
    {
        if (_agent == null)
            return;

        if (_agent.remainingDistance < _arrivalThreshold)
            Stop();
    }

    public void Init()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.acceleration = float.MaxValue;
        _agent.speed = _speed;
    }

    public void Move(Vector3 target)
    {
        _target = target;
        _agent.SetDestination(target);
        _agent.isStopped = false;
    }

    private void Stop()
    {
        _agent.isStopped = true;
        TargetReached?.Invoke();
    }
}
