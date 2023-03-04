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

    [SerializeField]
    [Tooltip("Whether an NPC is commuting to a destination or not")]
    private bool _isCommuting;

    private NavMeshAgent _agent;
    private NPC _npc;
    private Animator _animator;
    private GameManager _gameManager;

    private LinkedList<GameObject> _residentials;
    private LinkedList<GameObject> _commercials;
    private LinkedList<GameObject> _medicals;

    public delegate void NavigationEventHandler(GameObject obj);
    public event NavigationEventHandler OnReachedDestination;

    public Transform Destination { get => _destination; set => _destination = value; }
    public Transform Home { get => _home; set => _home = value; }
    public bool IsCommuting { get => _isCommuting; set => _isCommuting = value; }
    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _residentials = _gameManager.ResidentialDestinations;
        _commercials = _gameManager.CommercialDestinations;
        _medicals = _gameManager.MedicalDestinations;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _npc = GetComponent<NPC>();
        UpdateDestination();
    }

    private void Update()
    {
        UpdateDestination();
        UpdateAnimation();
        CheckDestinationReached();
    }

    private void NotifyReachedDestination(GameObject obj)
    {
        if (obj != null)
        {
            OnReachedDestination(obj);
        }
    }

    private void CheckDestinationReached()
    {
        if (Vector3.Distance(transform.position, _destination.position) <= 0.5f)
        {
            NotifyReachedDestination(gameObject);
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


    public void UpdateDestination()
    {
        if (_isCommuting)
        {
            _agent.destination = _destination.position;
            return;
        }

        if (_npc.Health <= _gameManager.HealthThreshold)
        {
            _destination = _medicals.ElementAt(Random.Range(0, _medicals.Count)).transform;
            _agent.destination = _destination.position;
            _isCommuting = true;
            return;
        }

        if (_npc.Stamina <= _gameManager.StaminaThreshold)
        {
            _destination = _home;
            _agent.destination = _destination.position;
            _isCommuting = true;
            return;
        }

        int randomIndex = Random.Range(0, _medicals.Count);
        _destination = _commercials.ElementAt(randomIndex).transform;
        _agent.destination = _destination.position;
        _isCommuting = true;
    }

    public void SetDestination(Vector3 position)
    {
        List<GameObject> destinations = _residentials.Concat(_commercials).Concat(_medicals).Concat(_residentials).ToList();
        foreach (GameObject dest in destinations)
        {
            if (dest.transform.position == position)
            {
                _destination = dest.transform;
                _agent.destination = _destination.position;
                _isCommuting = true;
                break;
            }
        }
    }

    public void SetHome(Vector3 position)
    {
        foreach (GameObject residential in _residentials)
        {
            if (residential.transform.position == position)
            {
                _home = residential.transform;
                _agent.destination = _destination.position;
                _isCommuting = true;
                break;
            }
        }
    }

}
