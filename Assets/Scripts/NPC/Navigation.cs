using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The destination that the NPC moves to")]
    private Transform _destination;

    private NavMeshAgent _agent;
    private Animator _animator;

    public void UpdateDestination(Transform newDest)
    {
        _destination = newDest;
        _agent.destination = _destination.position;
    }

    public Transform GetDestination()
    {
        return _destination;
    }

    private void UpdateAnimation()
    {
        if (_agent.velocity.magnitude > 0)
        {
            _animator.SetBool("isWalking", true);
            return;
        }
        _animator.SetBool("isWalking", false);
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateDestination(_destination);
        UpdateAnimation();
    }
}
