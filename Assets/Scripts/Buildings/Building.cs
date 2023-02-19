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

    public enum BuildingType
    {
        Commercial,
        Medical,
        Residential
    }

    protected abstract bool UpdateStamina(NPC npc);
    protected abstract bool UpdateHealth(NPC npc);
    protected abstract bool UpdateHappiness(NPC npc);
    protected abstract void ReleaseNPC(GameObject npc);

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
                npc.GetComponent<Navigation>().IsCommuting = false;
                npc.SetActive(false);
                _enRoute.Remove(npc);
                _visiting.AddFirst(npc);
                break;
            }
        }
    }

    protected void CalculateAttributes()
    {
        foreach (GameObject obj in _visiting.ToList())
        {
            NPC npc = obj.GetComponent<NPC>();
            if (UpdateStamina(npc))
            {
                ReleaseNPC(obj);
            }
            if (UpdateHealth(npc))
            {
                ReleaseNPC(obj);
            }
            UpdateHappiness(npc);
        }
    }

    protected void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
}

