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
            if (waypoint.name == gameObject.name)
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

    protected void CalculateHealth()
    {
        foreach (GameObject obj in _visiting.ToList())
        {
            NPC npc = obj.GetComponent<NPC>();
            if (npc.IsInfected)
            {
                npc.Health -= npc.Virus.HealthDecayRate * Time.deltaTime;
            }
            if (npc.Health <= _gameManager.HealthThreshold)
            {
                ReleaseNPC(obj);
            }
        }
    }

    protected abstract void ReleaseNPC(GameObject npc);
}

