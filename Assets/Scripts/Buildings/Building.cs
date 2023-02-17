using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The location that the NPC spawns at")]
    protected GameObject _spawnPoint;

    [SerializeField]
    [Tooltip("The Game Manager")]
    protected GameManager _gameManager;

    [SerializeField]
    [Tooltip("Ellapsed time")]
    protected float _elapsedTime;

    [Space]
    [Header("Recovery Rates")]

    [SerializeField]
    [Tooltip("The rate at which the NPC's stamin recovers")]
    protected float _staminaRecoveryRate = 1f;

    protected LinkedList<GameObject> _enRoute = new();
    protected LinkedList<GameObject> _visiting = new();

    public GameObject SpawnPoint { get => _spawnPoint; }
    public LinkedList<GameObject> EnRoute { get => _enRoute; }
    public LinkedList<GameObject> Visiting { get => _visiting; }


    protected void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    protected void SetSpawnPoint(LinkedList<GameObject> waypoints)
    {
        foreach (GameObject waypoint in waypoints.ToList())
        {
            if (Vector3.Distance(waypoint.transform.position, transform.position) < 10f)
            {
                _spawnPoint = waypoint;
                break;
            }
        }
    }

    protected void DetectNPC()
    {
        foreach (GameObject npc in _enRoute.ToList())
        {
            if (npc == null)
            {
                _enRoute.Remove(npc);
                continue;
            }
            if (!npc.activeSelf || npc.GetComponent<Navigation>().Destination.transform.position != _spawnPoint.transform.position)
            {
                continue;
            }
            if (Vector3.Distance(npc.transform.position, _spawnPoint.transform.position) < 1f)
            {
                npc.SetActive(false);
                _enRoute.Remove(npc);
                _visiting.AddFirst(npc);
                break;
            }
        }
    }

    protected void RecoverStamina()
    {
        foreach (GameObject npc in _visiting.ToList())
        {
            NPC component = npc.GetComponent<NPC>();
            if (component.Stamina < 100f)
            {
                component.Stamina += _staminaRecoveryRate * Time.deltaTime;
            }
            if (component.Stamina > 100f)
            {
                component.Stamina = 100f;
                npc.SetActive(true);
                _visiting.Remove(npc);
                ReleaseNPC(npc);
            }
        }
    }

    protected void CalculateHealth()
    {
        foreach (GameObject npc in _visiting.ToList())
        {
            NPC component = npc.GetComponent<NPC>();
            if (component.IsInfected)
            {
                component.Health -= component.Virus.HealthDecayRate * Time.deltaTime;
            }
        }
    }

    protected void ElapsedTime()
    {
        if (_elapsedTime >= 1)
        {
            _elapsedTime = 0;
        }
        else
        {
            _elapsedTime += Time.deltaTime;
        }
    }

    virtual protected void TransmitVirus()
    {
        if (_elapsedTime >= 1)
        {
            foreach (GameObject obj in _visiting.ToList())
            {
                NPC npc = obj.GetComponent<NPC>();
                if (npc.IsInfected)
                {
                    foreach (GameObject obj2 in _visiting.ToList())
                    {
                        NPC otherNPC = obj2.GetComponent<NPC>();
                        if (!otherNPC.IsInfected)
                        {
                            npc.Virus.TransmitVirus(otherNPC);
                        }
                    }
                }
            }
        }
    }

    abstract protected void ReleaseNPC(GameObject npc);


}

