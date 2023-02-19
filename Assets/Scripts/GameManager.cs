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
    private int _maxNPC = 250;

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

    [Space]
    [Header("Thresholds")]

    [SerializeField]
    [Tooltip("The health threshold to visit the hospital")]
    private int _healthThreshold = 25;

    [SerializeField]
    [Tooltip("The stamina threshold to go back home")]
    private int _staminaThreshold = 25;

    [SerializeField]
    private TimeManager _timeManager;

    [SerializeField]
    private Decisions _decisions;

    private readonly LinkedList<GameObject> _commercialDestinations = new();
    private readonly LinkedList<GameObject> _residentialDestinations = new();
    private readonly LinkedList<GameObject> _medicalDestinations = new();
    private readonly LinkedList<GameObject> _npcs = new();

    private LinkedList<Decisions> _decisionList = new LinkedList<Decisions>();

    public bool GodMode { get => _godMode; set => _godMode = value; }
    public int MaxNPCs { get => _maxNPC; set => _maxNPC = value; }
    public int NPCCount { get => _npcCount; set => _npcCount = value; }
    public GameObject HealthyPrefab { get => _healthyPrefab; }
    public GameObject InfectedPrefab { get => _infectedPrefab; }
    public float AverageHappiness { get => _averageHappiness; }
    public float PoliticalPower { get => _politicalPower; }
    public AssetChanger AssetChanger { get => _assetChanger; }
    public int HealthThreshold { get => _healthThreshold; set => _healthThreshold = value; }
    public int StaminaThreshold { get => _staminaThreshold; set => _staminaThreshold = value; }
    public TimeManager TimeManager { get => _timeManager; }
    public LinkedList<GameObject> CommercialDestinations { get => _commercialDestinations; }
    public LinkedList<GameObject> MedicalDestinations { get => _medicalDestinations; }
    public LinkedList<GameObject> ResidentialDestinations { get => _residentialDestinations; }
    public LinkedList<GameObject> NPCs { get => _npcs; }
    public Decisions Decisions { get => _decisions; }
    public LinkedList<Decisions> DecisionList { get => _decisionList; }


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
        if (_npcCount < _maxNPC)
        {
            GameObject newNPC = Instantiate(_healthyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            newNPC.transform.parent = GameObject.Find("NPCs").transform;
            int randomIndex = UnityEngine.Random.Range(0, _commercialDestinations.Count);
            newNPC.GetComponent<Navigation>().Home = spawnPoint.transform;
            newNPC.GetComponent<Navigation>().UpdateDestination(_commercialDestinations.ElementAt(randomIndex).transform, Building.BuildingType.Commercial);
            newNPC.tag = "NPC";
            _npcs.AddFirst(newNPC);
            _npcCount++;
        }
    }

    private void DestroyNPC()
    {
        if (_npcs.Count == 0)
        {
            return;
        }
        GameObject npc = _npcs.First();
        Destroy(npc);
        _npcs.Remove(npc);
        --_npcCount;
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
        }
        newNPC.transform.parent = npc.transform.parent;
        newNPC.GetComponent<NPC>().Copy(npc);
        newNPC.GetComponent<NPC>().Virus = null;
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

        #region DEBUG
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npc in npcs)
        {
            _npcs.AddFirst(npc);
            _npcCount++;
        }

        Decisions decision1 = Decisions.CreateInstance<Decisions>();
        decision1.description = "This is a decision";
        decision1.title = "Decision";
        decision1.healthEffect = 0;
        decision1.virusEffect = 0;
        decision1.happyEffect = 0;
        decision1.isActive = false;

        Decisions decision2 = Decisions.CreateInstance<Decisions>();
        decision2.description = "This is a decision";
        decision2.title = "Poop1";

        Decisions decision3 = Decisions.CreateInstance<Decisions>();
        decision3.description = "This is a decision";
        decision3.title = "Poop2";
        decision3.isActive = true;

        Decisions decision4 = Decisions.CreateInstance<Decisions>();
        decision4.description = "This is a decision";
        decision4.title = "Poop3";

        Decisions decision5 = Decisions.CreateInstance<Decisions>();
        decision5.description = "This is a decision";
        decision5.title = "Poop4";

        Decisions decision6 = Decisions.CreateInstance<Decisions>();
        decision6.description = "This is a decision";
        decision6.title = "Poop5";

        DecisionList.AddLast(decision1);
        DecisionList.AddLast(decision2);
        DecisionList.AddLast(decision3);
        DecisionList.AddLast(decision4);
        DecisionList.AddLast(decision5);
        DecisionList.AddLast(decision6);
        #endregion DEBUG
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

        if (Input.GetKeyDown(KeyCode.S) && _godMode)
        {
            _timeManager.SetTimeScale(8);
        }
        #endregion GOD_MODE

        CalculateAverageHappiness();
        CalculatePoliticalPower();
        _timeManager.OnKeyDown();
        _timeManager.Clock();
    }

}
