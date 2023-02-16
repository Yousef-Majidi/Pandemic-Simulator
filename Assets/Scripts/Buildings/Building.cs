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
            if (npc.activeSelf && npc.GetComponent<Navigation>().Destination.transform.position == _spawnPoint.transform.position)
            {
                if (Vector3.Distance(npc.transform.position, _spawnPoint.transform.position) < 1f)
                {
                    npc.SetActive(false);
                    _enRoute.Remove(npc);
                    _visiting.AddFirst(npc);
                    break;
                }
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

    abstract protected void ReleaseNPC(GameObject npc);


}

