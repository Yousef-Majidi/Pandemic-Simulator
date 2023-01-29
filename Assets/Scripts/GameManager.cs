using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("God mode status")]
    private bool _godMode;

    [SerializeField]
    [Tooltip("Max NPCs")]
    private int _maxNPCs = 500;

    [SerializeField]
    [Tooltip("Currently spawned")]
    private int _npcCount = 0;

    [SerializeField]
    [Tooltip("Test Spawn Point")]
    private GameObject _testSpawnPoint;

    [SerializeField]
    [Tooltip("Healthy NPC Prefab")]
    private GameObject _healthyPrefab;

    [SerializeField]
    [Tooltip("Infected NPC Prefab")]
    private GameObject _infectedPrefab;

    [SerializeField]
    [Tooltip("Average Happiness of all NPCs")]
    private float _averageHappiness;


    // a linked list of all waypoints
    private readonly LinkedList<GameObject> _destinations = new();

    // a linked list of all NPCS
    private readonly LinkedList<GameObject> _npcs = new();

    public bool GodMode { get => _godMode; set => _godMode = value; }

    private void ToggleGodMode()
    {
        if (!_godMode)
        {
            _godMode = true;
            Debug.Log("God mode enabled");
        }
        else
        {
            _godMode = false;
            Debug.Log("God mode disabled");
        }
    }

    private void SpawnNPC()
    {
        if (_npcCount < _maxNPCs)
        {
            // instantiate a new healthy npc at testSpawnPoint
            GameObject newNPC = Instantiate(_healthyPrefab, _testSpawnPoint.transform.position, _testSpawnPoint.transform.rotation);
            // place the npc as a child of "NPCs" in the hierarchy
            newNPC.transform.parent = GameObject.Find("NPCs").transform;
            // find a random waypoint
            int randomIndex = Random.Range(0, _destinations.Count);
            // set the destination of newNPC to the random waypoint
            newNPC.GetComponent<Navigation>().UpdateDestination(_destinations.ElementAt(randomIndex).transform);
            // set the tag to NPC
            newNPC.tag = "NPC";
            _npcs.AddFirst(newNPC);
            // Debug.Log the name of the NPC
            Debug.Log(newNPC.name + " created - going to " + _destinations.ElementAt(randomIndex).name);
            _npcCount++;
        }
    }

    private void DestroyNPC()
    {
        GameObject npc = _npcs.First();
        Debug.Log("Removed: " + npc.name);
        Destroy(npc);
        _npcs.Remove(npc);
    }

    private void RefreshDestinations()
    {
        foreach (GameObject npc in _npcs.ToList())
        {
            if (npc != null)
            {
                int randomIndex = Random.Range(0, _destinations.Count);
                npc.GetComponent<Navigation>().UpdateDestination(_destinations.ElementAt(randomIndex).transform);
                Debug.Log(npc.name + " is going to " + _destinations.ElementAt(randomIndex));
            }
        }
    }

    private void CalculateAverageHappiness()
    {
        float totalHappiness = 0;
        foreach (GameObject npc in _npcs.ToList())
        {
            if (npc)
            {
                totalHappiness += npc.GetComponent<NPC>().Happiness;
            }
            else
            {
                _npcs.Remove(npc);
            }
        }
        _averageHappiness = totalHappiness / _npcs.Count;
    }

    void Awake()
    {
        // find all the waypoints on the scene
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Test");
        foreach (GameObject waypoint in waypoints)
        {
            _destinations.AddFirst(waypoint);
        }
        _npcs.AddFirst(GameObject.FindGameObjectWithTag("NPC"));
        InvokeRepeating(nameof(SpawnNPC), 0f, 1f);
        InvokeRepeating(nameof(RefreshDestinations), 0f, 10f);
    }

    void Update()
    {
        #region GOD MODE
        if (_npcCount == _maxNPCs)
        {
            CancelInvoke(nameof(SpawnNPC));
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGodMode();
        }

        if (Input.GetKeyDown(KeyCode.N) && _godMode)
        {
            SpawnNPC();
        }

        if (Input.GetKeyDown(KeyCode.V) && _godMode)
        {
            DestroyNPC();
        }
        #endregion

        CalculateAverageHappiness();

    }
}
