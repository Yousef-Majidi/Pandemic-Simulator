using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("God Mode")]
    [SerializeField]
    [Tooltip("God mode status")]
    private bool _godMode;

    [Space]
    [Header("NPCs")]

    [SerializeField]
    [Tooltip("Sets the max number of NPCs allowed to be spawned")]
    private int _maxNPCs = 250;

    [SerializeField]
    [Tooltip("Currently spawned on the scene")]
    private int _npcCount = 0;

    [SerializeField]
    [Tooltip("Average Happiness of all NPCs")]
    private float _averageHappiness = 1f;

    [SerializeField]
    [Tooltip("Political Power")]
    private float _politicalPower = 1f;

    [SerializeField]
    [Tooltip("The political power multiplier")]
    private float _politicalPowerMultiplier = 0.001f;

    [Space]
    [Header("Prefabs")]

    [SerializeField]
    [Tooltip("Healthy NPC Prefab")]
    private GameObject _healthyPrefab;

    [SerializeField]
    [Tooltip("Infected NPC Prefab")]
    private GameObject _infectedPrefab;

    [SerializeField]
    private AssetChanger _assetChanger;

    private readonly LinkedList<GameObject> _commercialDestinations = new();
    private readonly LinkedList<GameObject> _residentialDestinations = new();
    private readonly LinkedList<GameObject> _medicalDestinations = new();
    private readonly LinkedList<GameObject> _npcs = new();

    public bool GodMode { get => _godMode; set => _godMode = value; }
    public int MaxNPCs { get => _maxNPCs; set => _maxNPCs = value; }
    public int NPCCount { get => _npcCount; set => _npcCount = value; }
    public GameObject HealthyPrefab { get => _healthyPrefab; }
    public GameObject InfectedPrefab { get => _infectedPrefab; }
    public float AverageHappiness { get => _averageHappiness; }
    public float PoliticalPower { get => _politicalPower; }
    public AssetChanger AssetChanger { get => _assetChanger; }
    public LinkedList<GameObject> CommercialDestinations { get => _commercialDestinations; }
    public LinkedList<GameObject> MedicalDestinations { get => _medicalDestinations; }
    public LinkedList<GameObject> ResidentialDestinations { get => _residentialDestinations; }

    private void ToggleGodMode()
    {
        if (!_godMode)
        {
            _godMode = true;
            Debug.Log("God mode enabled");
            return;
        }
        _godMode = false;
        Debug.Log("God mode disabled");
    }

    private void SpawnNPC(GameObject spawnPoint)
    {
        if (_npcCount < _maxNPCs)
        {
            GameObject newNPC = Instantiate(_healthyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            newNPC.transform.parent = GameObject.Find("NPCs").transform;
            int randomIndex = UnityEngine.Random.Range(0, _commercialDestinations.Count);
            newNPC.GetComponent<Navigation>().Home = spawnPoint.transform;
            Debug.Log("Spawn point: " + spawnPoint.transform);
            newNPC.GetComponent<Navigation>().UpdateDestination(_commercialDestinations.ElementAt(randomIndex).transform);
            newNPC.tag = "NPC";
            _npcs.AddFirst(newNPC);
            _npcCount++;
            Debug.Log($"Spawned NPC: {newNPC.name}" + $" - Going to: {_commercialDestinations.ElementAt(randomIndex).name}");
        }
    }

    private void DestroyNPC()
    {
        if (_npcs.Count == 0)
        {
            Debug.Log("No NPCs to destroy");
            return;
        }
        GameObject npc = _npcs.First();
        Debug.Log("Removed: " + npc.name);
        Destroy(npc);
        _npcs.Remove(npc);
        --_npcCount;
    }

    private void RefreshDestinations()
    {
        foreach (GameObject npc in _npcs.ToList())
        {
            if (npc != null)
            {
                int randomIndex = UnityEngine.Random.Range(0, _commercialDestinations.Count);
                npc.GetComponent<Navigation>().UpdateDestination(_commercialDestinations.ElementAt(randomIndex).transform);
                Debug.Log($"{npc.name} is now going to {_commercialDestinations.ElementAt(randomIndex)}");
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
                --_npcCount;
            }
        }
        _averageHappiness = totalHappiness / _npcs.Count;
    }

    public void UpdateAsset(GameObject npc)
    {
        Debug.Log("Updating asset for " + npc.name);
        GameObject newNPC;
        if (npc.GetComponent<NPC>().IsInfected)
        {
            newNPC = _assetChanger.UpdateAsset(_infectedPrefab, npc.transform.position, npc.transform.rotation);
            newNPC.GetComponent<NPC>().Asset = NPC.AssetType.Infected;
        }
        else
        {
            newNPC = _assetChanger.UpdateAsset(_healthyPrefab, npc.transform.position, npc.transform.rotation);
            newNPC.GetComponent<NPC>().Asset = NPC.AssetType.Healthy;
            newNPC.GetComponent<NPC>().Virus = null;
        }
        newNPC.transform.parent = npc.transform.parent;
        newNPC.GetComponent<NPC>().Copy(npc);
        _npcs.Remove(npc);
        _npcs.AddFirst(newNPC);
        Destroy(npc);
    }

    public void CalculatePoliticalPower()
    {
        if (!float.IsNaN(_averageHappiness))
            _politicalPower += _averageHappiness * Time.deltaTime * _politicalPowerMultiplier;
    }

    void Awake()
    {
        GameObject[] commercialWaypoints = GameObject.FindGameObjectsWithTag("Commercial");
        foreach (GameObject waypoint in commercialWaypoints)
        {
            _commercialDestinations.AddFirst(waypoint);
        }

        GameObject[] residentialWaypoints = GameObject.FindGameObjectsWithTag("Residential");
        foreach (GameObject waypoint in residentialWaypoints)
        {
            _residentialDestinations.AddFirst(waypoint);
        }

        GameObject[] medicalWaypoints = GameObject.FindGameObjectsWithTag("Medical");
        foreach (GameObject waypoint in medicalWaypoints)
        {
            _medicalDestinations.AddFirst(waypoint);
        }

        // DEBUG:
        InvokeRepeating(nameof(RefreshDestinations), 0f, 30f);
    }

    void Update()
    {
        #region GOD_MODE
        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGodMode();
        }

        if (Input.GetKeyDown(KeyCode.N) && _godMode)
        {
            int randomIndex = UnityEngine.Random.Range(0, _residentialDestinations.Count);
            SpawnNPC(_residentialDestinations.ElementAt(randomIndex));
        }

        if (Input.GetKeyDown(KeyCode.V) && _godMode)
        {
            DestroyNPC();
        }
        #endregion GOD_MODE

        CalculateAverageHappiness();
        CalculatePoliticalPower();
    }
}
