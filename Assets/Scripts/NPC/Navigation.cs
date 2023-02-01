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
    private GameObject[] _destinations;
    private int _noOfDest;

    public void UpdateDestination(Transform newDest)
    {
        //_destination = newDest;
        if (_agent.velocity.magnitude == 0)
        {
            _destination = _destinations[Random.Range(0,_noOfDest)].transform;
            _agent.destination = _destination.position;
        }
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
        _destinations = GameObject.FindGameObjectsWithTag("Commercial");
        print(_destinations[0].transform);
        _noOfDest = _destinations.Length;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    { 
         
            UpdateDestination(_destination);
            UpdateAnimation();
        
    }
}
