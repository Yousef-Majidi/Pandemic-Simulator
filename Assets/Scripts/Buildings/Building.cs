using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Building capacity")]
    protected int _capacity;

    [SerializeField]
    [Tooltip("Currently inside the building")]
    protected int _occupancy;

    [SerializeField]
    [Tooltip("The location that the NPC spawns at")]
    protected GameObject _spawnPoint;

    [SerializeField]
    [Tooltip("The Game Manager")]
    protected GameManager _gameManager;

    protected LinkedList<GameObject> _visiting = new();

    public GameObject SpawnPoint { get => _spawnPoint; }
    public LinkedList<GameObject> Visiting { get => _visiting; }
    public int Capacity { get => _capacity; }
    public int Occupancy { get => _occupancy; set => _occupancy = value; }

    protected abstract bool UpdateStamina(NPC npc);
    protected abstract bool UpdateHealth(NPC npc);
    protected abstract bool UpdateHappiness(NPC npc);
    protected abstract void ReleaseNPC(GameObject npc);

    protected void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Subscribe(Navigation nav)
    {
        nav.OnReachedDestination += Nav_OnReachedDestination;
    }

    public void Unsubscribe(Navigation nav)
    {
        nav.OnReachedDestination -= Nav_OnReachedDestination;
    }

    protected void Nav_OnReachedDestination(GameObject obj)
    {
        if (obj.activeSelf)
        {
            if (this is Residential || _occupancy == 0)
            {
                obj.GetComponent<Navigation>().IsCommuting = false;
                obj.SetActive(false);
                _visiting.AddFirst(obj);
                return;
            }
            if (_occupancy == _capacity)
            {
                obj.GetComponent<Navigation>().IsCommuting = false;
            }
        }
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

    protected void CalculateAttributes()
    {
        foreach (GameObject obj in _visiting.ToList())
        {
            if (obj != null)
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
            else
            {
                _visiting.Remove(obj);
            }
        }
    }
}

