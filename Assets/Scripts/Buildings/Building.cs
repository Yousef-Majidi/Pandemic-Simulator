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

    [SerializeField]
    [Tooltip("List of visiting NPCs")]
    protected List<GameObject> _visiting = new();

    public GameObject SpawnPoint { get => _spawnPoint; }
    public List<GameObject> Visiting { get => _visiting; }
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
        Debug.Log($"{gameObject} <color=green>subscribed</color> to {nav.gameObject.name}");
    }

    public void Unsubscribe(Navigation nav)
    {
        nav.OnReachedDestination -= Nav_OnReachedDestination;
        Debug.Log($"{gameObject} <color=red>unsubscribed</color> from {nav.gameObject.name}");
    }

    protected void Nav_OnReachedDestination(GameObject obj)
    {
        if (obj.activeSelf)
        {
            if (this is Residential || _occupancy == 0 || _occupancy < _capacity)
            {
                obj.GetComponent<Navigation>().IsTravelling = false;
                obj.SetActive(false);
                _visiting.Add(obj);
                return;
            }
            if (_occupancy == _capacity)
            {
                obj.GetComponent<Navigation>().IsTravelling = false;
                return;
            }
        }
    }

    protected void SetSpawnPoint(List<GameObject> waypoints)
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

