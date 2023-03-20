using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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
    private bool _isTravelling;

    [SerializeField]
    [Tooltip("The building the NPC is currently travelling to")]
    private Building _travellingTo;

    private NavMeshAgent _agent;
    private NPC _npc;
    private Animator _animator;
    private GameManager _gameManager;

    private List<GameObject> _residentials;
    private List<GameObject> _commercials;
    private List<GameObject> _medicals;
    private List<GameObject> _destinations;

    public delegate void NavigationEventHandler(GameObject obj);
    public event NavigationEventHandler OnReachedDestination;

    public Transform Destination { get => _destination; set => _destination = value; }
    public Transform Home { get => _home; set => _home = value; }
    public bool IsTravelling { get => _isTravelling; set => _isTravelling = value; }
    public Building TravelingTo { get => _travellingTo; set => _travellingTo = value; }
    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _residentials = _gameManager.ResidentialDestinations;
        _commercials = _gameManager.CommercialDestinations;
        _medicals = _gameManager.MedicalDestinations;
        _destinations = _residentials.Concat(_commercials).Concat(_medicals).Concat(_residentials).ToList();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _npc = GetComponent<NPC>();
        _isTravelling = false;
        _agent.speed = Random.Range(1.0f, 3.0f);
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
        if (_isTravelling)
        {
            _agent.destination = _destination.position;
            return;
        }

        _travellingTo.Unsubscribe(this);
        Building building;
        GameObject waypoint;
        int randomIndex;

        if (_npc.Health <= _gameManager.HealthThreshold)
        {
            randomIndex = UnityEngine.Random.Range(0, _medicals.Count);
            waypoint = _medicals.ElementAt(randomIndex);
            building = waypoint.GetComponentInParent<Medical>();

            _travellingTo = building;
            _travellingTo.Subscribe(this);
            _destination = waypoint.transform;
            _agent.destination = _destination.position;
            _isTravelling = true;
            return;
        }

        if (_npc.Stamina <= _gameManager.StaminaThreshold)
        {
            _destination = _home;
            foreach (GameObject residential in _residentials)
            {
                if (residential.transform == _destination)
                {
                    building = residential.GetComponentInParent<Residential>();
                    building.Subscribe(this);
                }
            }
            _agent.destination = _destination.position;
            _isTravelling = true;
            return;
        }

        randomIndex = UnityEngine.Random.Range(0, _commercials.Count);
        waypoint = _commercials.ElementAt(randomIndex);
        building = waypoint.GetComponentInParent<Commercial>();
        _travellingTo = building;
        _travellingTo.Subscribe(this);
        _destination = waypoint.transform;
        _agent.destination = _destination.position;
        _isTravelling = true;
    }

    public void SetDestination(Vector3 position)
    {
        foreach (GameObject dest in _destinations)
        {
            if (dest.transform.position == position)
            {
                _travellingTo = dest.GetComponentInParent<Building>();
                _travellingTo.Subscribe(this);
                _destination = dest.transform;
                _agent.destination = _destination.position;
                _isTravelling = true;
                break;
            }
        }
    }

    public void SetHome(Vector3 position)
    {
        foreach (GameObject residential in _residentials)
        {
            if (Vector3.Distance(residential.transform.position, position) < 10.1f)
            {
                _home = residential.transform;
                _travellingTo = residential.GetComponentInParent<Building>();
                _travellingTo.Subscribe(this);
                _destination = _home;
                _agent.destination = _destination.position;
                break;
            }
        }
    }

}
