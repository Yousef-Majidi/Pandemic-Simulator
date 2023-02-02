using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The destination that the NPC moves to")]
    private Transform _destination;

    [SerializeField]
    [Tooltip("The location that the NPC spawns at")]
    private Transform _home;

    private NavMeshAgent _agent;
    private NPC _npc;
    private Animator _animator;
    private GameManager _gameManager;
    private bool _isCommuting;

    private LinkedList<GameObject> _commercials = new();
    private LinkedList<GameObject> _medicals = new();

    public Transform Destination { get => _destination; set => _destination = value; }
    public Transform Home { get => _home; set => _home = value; }

    public void UpdateDestination(Transform newDest)
    {
        if (_npc.Health < 25)
        {
            _destination = _medicals.ElementAt(Random.Range(0, _medicals.Count)).transform;
            _agent.destination = _destination.position;
            return;
        }

        if (_npc.Stamina < 25)
        {
            _destination = _home;
            _agent.destination = _destination.position;
            return;
        }

        if (!_isCommuting)
        {
            _destination = _commercials.ElementAt(Random.Range(0, _commercials.Count)).transform;
            _agent.destination = _destination.position;
            _isCommuting = true;
            return;
        }
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
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _commercials = _gameManager.CommercialDestinations;
        GameObject[] commercialWaypoints = GameObject.FindGameObjectsWithTag("Commercial");
        foreach (GameObject waypoint in commercialWaypoints)
        {
            _commercials.AddFirst(waypoint);
        }

        _medicals = _gameManager.MedicalDestinations;
        GameObject[] medicalWaypoints = GameObject.FindGameObjectsWithTag("Medical");
        foreach (GameObject waypoint in medicalWaypoints)
        {
            _medicals.AddFirst(waypoint);
        }

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _npc = GetComponent<NPC>();
    }

    private void Update()
    {
        UpdateDestination(_destination);
        if (Vector3.Distance(transform.position, _destination.position) < 1f)
        {
            _isCommuting = false;
        }
        UpdateAnimation();
    }
}
