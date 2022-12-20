using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The destination that the NPC moves to")]
    private Transform _destination;

    private NavMeshAgent _agent;

    public void SetDestination()
    {
        _agent.destination = _destination.position;
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        SetDestination();
    }
}
