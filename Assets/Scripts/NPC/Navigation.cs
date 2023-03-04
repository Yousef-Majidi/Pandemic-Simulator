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

    public Transform Destination { get => _destination; set => _destination = value; }
    public Transform Home { get => _home; set => _home = value; }
    public bool IsCommuting { get => _isCommuting; set => _isCommuting = value; }

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
            foreach (GameObject medical in _gameManager.MedicalDestinations)
            {
                Medical building = medical.GetComponentInParent<Medical>();
                if (medical.transform == _destination && !building.EnRoute.Contains(gameObject))
                {
                    building.EnRoute.AddFirst(gameObject);
                    break;
                }
            }
            return;
        }

        if (_npc.Stamina <= _gameManager.StaminaThreshold)
        {
            _destination = _home;
            _agent.destination = _destination.position;
            _isCommuting = true;
            foreach (GameObject residential in _gameManager.ResidentialDestinations)
            {
                Residential building = residential.GetComponentInParent<Residential>();
                if (residential.transform == _destination && !building.EnRoute.Contains(gameObject))
                {
                    building.EnRoute.AddFirst(gameObject);
                    break;
                }
            }
            return;
        }

        int randomIndex = Random.Range(0, _medicals.Count);
        _destination = _commercials.ElementAt(randomIndex).transform;
        _agent.destination = _destination.position;
        _isCommuting = true;
        foreach (GameObject commercial in _commercials)
        {
            Commercial building = commercial.GetComponentInParent<Commercial>();
            if (commercial.transform == _destination)
            {
                if (!building.EnRoute.Contains(gameObject))
                {
                    building.EnRoute.AddFirst(gameObject);
                    break;
                }
            }
        }
    }

    public void UpdateDestination(Transform newDest, Building.BuildingType type)
    {
        _isCommuting = true;
        _destination = newDest;
        _agent.destination = _destination.position;
        if (type == Building.BuildingType.Residential)
        {
            foreach (GameObject residential in _gameManager.ResidentialDestinations)
            {
                Residential building = residential.GetComponentInParent<Residential>();
                if (residential.transform == _destination && !building.EnRoute.Contains(gameObject))
                {
                    building.EnRoute.AddFirst(gameObject);
                    return;
                }
            }
        }

        if (type == Building.BuildingType.Commercial)
        {
            foreach (GameObject commercial in _commercials)
            {
                Commercial building = commercial.GetComponentInParent<Commercial>();
                if (commercial.transform == _destination && !building.EnRoute.Contains(gameObject))
                {
                    building.EnRoute.AddFirst(gameObject);
                    return;
                }
            }
        }

        if (type == Building.BuildingType.Medical)
        {
            foreach (GameObject medical in _medicals)
            {
                Medical building = medical.GetComponentInParent<Medical>();
                if (medical.transform == _destination && !building.EnRoute.Contains(gameObject))
                {
                    building.EnRoute.AddFirst(gameObject);
                    break;
                }
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
                break;
            }
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
        _residentials = _gameManager.ResidentialDestinations;
        _commercials = _gameManager.CommercialDestinations;
        _medicals = _gameManager.MedicalDestinations;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _npc = GetComponent<NPC>();
    }

    private void Update()
    {
        UpdateDestination();
        UpdateAnimation();
    }
}
