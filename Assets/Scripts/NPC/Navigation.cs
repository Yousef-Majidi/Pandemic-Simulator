using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public void UpdateDestination()
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
            foreach (GameObject residential in _gameManager.ResidentialDestinations)
            {
                if (residential.transform == _destination)
                {
                    if (!residential.GetComponentInParent<Residential>().EnRoute.Contains(gameObject))
                    {
                        residential.GetComponentInParent<Residential>().EnRoute.AddFirst(gameObject);
                        Debug.Log("Added " + gameObject.name + " to EnRoute");
                    }

                    return;
                }
            }
            return;
        }

        if (!_isCommuting)
        {
            int randomIndex = Random.Range(0, _medicals.Count);
            _destination = _commercials.ElementAt(randomIndex).transform;
            _agent.destination = _destination.position;
            _isCommuting = true;
            return;
        }
    }

    public void UpdateDestination(Transform newDest)
    {
        _isCommuting = true;
        _destination = newDest;
        _agent.destination = _destination.position;
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
        _medicals = _gameManager.MedicalDestinations;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _npc = GetComponent<NPC>();
    }

    private void Update()
    {
        UpdateDestination();
        if (Vector3.Distance(transform.position, _destination.position) < 1f)
        {
            _isCommuting = false;
        }
        UpdateAnimation();
    }
}
