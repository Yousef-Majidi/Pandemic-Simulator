using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The destination that the NPC moves to")]
    private Transform _destination;
    private Transform _home;

    private NavMeshAgent _agent;
    private NPC _npc;
    private Animator _animator;

    private LinkedList<GameObject> _commercials = new();
    private LinkedList<GameObject> _medicals = new();

    //private GameObject[] _commercials;
    //private GameObject[] _residential;
    //private GameObject[] _medical;

    private int _noOfComm;
    private int _noOfResi;
    private int _noOfMedi;

    //getters and setter

    public Transform Destination { get => _destination; set => _destination = value; }
    public Transform Home { get => _home; set => _destination = _home; }
    //public LinkedList<GameObject> Commercials { get => _commercials; set => _commercials = value; }
    //public GameObject[] Residential{ get => _residential; set => _residential= value; }
    //public GameObject[] Medical { get => _medical; set => _medical = value; }
    //private GameManager _gameManager;

    // add getter and setter for all private members

    public void UpdateDestination(Transform newDest)
    {
        //_destination = newDest;
        if (_agent.velocity.magnitude == 0)
        {
            if (_npc.Health < 25)
            {
                //_destination = _medicals[Random.Range(0, _noOfMedi)].transform;
                _destination = _medicals.ElementAt(Random.Range(0, _medicals.Count)).transform;
                _agent.destination = _destination.position;
            }

            else if (_npc.Stamina < 25)
            {
                _destination = _home;
                _agent.destination = _destination.position;
            }

            else
            {
                _destination = _commercials.ElementAt(Random.Range(0, _commercials.Count)).transform;
                _agent.destination = _destination.position;
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
        _commercials = _gameManager.CommercialDestinations;
        GameObject[] commercialWaypoints = GameObject.FindGameObjectsWithTag("Commercial");

        foreach (GameObject waypoint in commercialWaypoints)
        {
            _commercials.AddFirst(waypoint);
        }

        //_residentials = GameObject.FindGameObjectsWithTag("Residential");
        //_noOfResi = _residential.Length;
        //_home = _residential[0].transform;

        GameObject[] medicalWaypoints = GameObject.FindGameObjectsWithTag("Medical");
        _medicals = _gameManager.MedicalDestination;
        foreach (GameObject waypoint in medicalWaypoints)
        {
            _medicals.AddFirst(waypoint);
        }

        //_noOfMedi = _medical.Length;

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _npc = GetComponent<NPC>();
    }

    private void Update()
    { 
         
            UpdateDestination(_destination);
            UpdateAnimation();
        
    }
}
