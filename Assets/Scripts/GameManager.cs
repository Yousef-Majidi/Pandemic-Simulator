using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("God Mode")]
    [SerializeField]
    [Tooltip("God mode status")]
    private bool _godMode;

    [SerializeField]
    [Tooltip("Number of healthy npcs to spawn on start")]
    private int _spawnHealthyAtStart;

    [SerializeField]
    [Tooltip("Number of infected npcs to spawn on start")]
    private int _spawnInfectedAtStart;

    [Space]
    [Header("NPCs")]

    [SerializeField]
    [Tooltip("Sets the max number of NPCs allowed to be spawned")]
    private int _maxNPC = 250;

    [SerializeField]
    [Tooltip("Number of healthy NPCs")]
    private int _healthyCount = 0;

    [SerializeField]
    [Tooltip("Number of infected NPCs")]
    private int _infectedCount = 0;

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

    [Space]
    [Header("Time Manager")]

    [SerializeField]
    private TimeManager _timeManager;

    [Space]
    [Header("Decisions")]

    [SerializeField]
    [Tooltip("The list of all decisions")]
    private List<Decision> _decisionList = new();

    readonly SaveManager _saveManager = new();

    private List<GameObject> _commercialDestinations = new();
    private List<GameObject> _residentialDestinations = new();
    private List<GameObject> _medicalDestinations = new();
    private LinkedList<GameObject> _npcs = new();

    public bool GodMode { get => _godMode; set => _godMode = value; }
    public int MaxNPCs { get => _maxNPC; set => _maxNPC = value; }
    public int HealthyCount { get => _healthyCount; }
    public int InfectedCount { get => _infectedCount; }
    public int NPCCount { get => _npcs.Count; }
    public GameObject HealthyPrefab { get => _healthyPrefab; }
    public GameObject InfectedPrefab { get => _infectedPrefab; }
    public float AverageHappiness { get => _averageHappiness; }
    public float PoliticalPower { get => _politicalPower; set => _politicalPower = value; }
    public float PoliticalPowerMultiplier { get => _politicalPowerMultiplier; set => _politicalPowerMultiplier = value; }
    public AssetChanger AssetChanger { get => _assetChanger; }
    public int HealthThreshold { get => _healthThreshold; set => _healthThreshold = value; }
    public int StaminaThreshold { get => _staminaThreshold; set => _staminaThreshold = value; }
    public TimeManager TimeManager { get => _timeManager; }
    public List<GameObject> CommercialDestinations { get => _commercialDestinations; }
    public List<GameObject> MedicalDestinations { get => _medicalDestinations; }
    public List<GameObject> ResidentialDestinations { get => _residentialDestinations; }
    public List<Decision> DecisionList { get => _decisionList; }
    public LinkedList<GameObject> NPCs { get => _npcs; }


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

    public void DestroyNPC()
    {
        if (_npcs.Count != 0)
        {
            GameObject obj = _npcs.First();
            _npcs.Remove(obj);
            Destroy(obj);
        }
    }

    public void DestroyNPC(GameObject obj)
    {
        if (_npcs.Count != 0)
        {
            _npcs.Remove(obj);
            Destroy(obj);
        }
    }

    public GameObject SpawnNPC(Vector3 position, Quaternion rotation, bool infected = false)
    {
        if (_npcs.Count < _maxNPC)
        {
            GameObject obj;
            if (infected)
            {
                obj = Instantiate(_infectedPrefab, position, rotation);
            }
            else
            {
                obj = Instantiate(_healthyPrefab, position, rotation);
            }
            obj.transform.parent = GameObject.Find("NPCs").transform;
            obj.GetComponent<Navigation>().SetHome(position);
            obj.tag = "NPC";
            obj.name = infected ? $"NPC {_npcs.Count + 1} - infected" : $"NPC {_npcs.Count + 1}";
            _npcs.AddLast(obj);
            return obj;
        }
        return null;
    }
    public void UpdateAsset(GameObject obj)
    {
        GameObject newObj;
        if (obj.GetComponent<NPC>().IsInfected)
        {
            newObj = _assetChanger.UpdateAsset(_infectedPrefab, obj.transform.position, obj.transform.rotation);
            newObj.GetComponent<NPC>().Asset = NPC.AssetType.Infected;
            _infectedCount++;
            _healthyCount = _npcs.Count - _infectedCount;
        }
        else
        {
            newObj = _assetChanger.UpdateAsset(_healthyPrefab, obj.transform.position, obj.transform.rotation);
            newObj.GetComponent<NPC>().Asset = NPC.AssetType.Healthy;
            newObj.GetComponent<NPC>().Virus = null;
            _healthyCount++;
            _infectedCount = _npcs.Count - _healthyCount;
        }
        newObj.transform.parent = obj.transform.parent;
        newObj.GetComponent<NPC>().Copy(obj.GetComponent<NPC>());
        _npcs.Remove(obj);
        _npcs.AddLast(newObj);
        Destroy(obj);
    }

    public void CalculatePoliticalPower()
    {
        if (!float.IsNaN(_averageHappiness))
            _politicalPower += _averageHappiness * Time.deltaTime * _politicalPowerMultiplier;
    }

    private void Awake()
    {
        GameObject[] commercialWaypoints = GameObject.FindGameObjectsWithTag("Commercial");
        foreach (GameObject waypoint in commercialWaypoints)
        {
            _commercialDestinations.Add(waypoint);
        }

        GameObject[] residentialWaypoints = GameObject.FindGameObjectsWithTag("Residential");
        foreach (GameObject waypoint in residentialWaypoints)
        {
            _residentialDestinations.Add(waypoint);
        }

        GameObject[] medicalWaypoints = GameObject.FindGameObjectsWithTag("Medical");
        foreach (GameObject waypoint in medicalWaypoints)
        {
            _medicalDestinations.Add(waypoint);
        }

        foreach (Decision decision in _decisionList)
        {
            decision.OnDecisionEnact += Decision_OnDecisionEnact;
            decision.OnDecisionRevoke += Decision_OnDecisionRevoke;
        }
    }

    private void Start()
    {
        #region DEBUG
        {
            GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
            foreach (GameObject npc in npcs)
            {
                _npcs.AddFirst(npc);
            }

            List<GameObject> availableBuildings = _residentialDestinations.ToList();
            Residential building;
            GameObject waypoint;
            int randomIndex;
            for (int i = 0; i < _spawnInfectedAtStart; i++)
            {
                randomIndex = UnityEngine.Random.Range(0, availableBuildings.Count);
                waypoint = availableBuildings[randomIndex];
                building = waypoint.GetComponentInParent<Residential>();
                building.Occupancy++;
                if (building.Occupancy == building.Capacity)
                {
                    availableBuildings.Remove(waypoint);
                }
                SpawnNPC(waypoint.transform.position, waypoint.transform.rotation, true);
                _infectedCount++;
            }
            for (int i = 0; i < _spawnHealthyAtStart; i++)
            {
                randomIndex = UnityEngine.Random.Range(0, availableBuildings.Count);
                waypoint = availableBuildings[randomIndex];
                building = waypoint.GetComponentInParent<Residential>();
                building.Occupancy++;
                if (building.Occupancy == building.Capacity)
                {
                    availableBuildings.Remove(waypoint);
                }
                SpawnNPC(waypoint.transform.position, waypoint.transform.rotation);
                _healthyCount++;
            }
        }
        #endregion DEBUG
    }

    private void Decision_OnDecisionEnact(Decision decision, float normalizedPoliticalPower)
    {
        Debug.Log($"{decision.Title} was enacted");
        Debug.Log($"Normalized Political Power: {normalizedPoliticalPower}");

        decision.ApplyEffects(_npcs, normalizedPoliticalPower);
    }

    private void Decision_OnDecisionRevoke(Decision decision, float normalizedPoliticalPower)
    {
        Debug.Log($"{decision.Title} was removed");

        decision.RemoveEffects();
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
            Residential building;
            GameObject waypoint;
            do
            {
                int randomIndex = UnityEngine.Random.Range(0, _residentialDestinations.Count);
                waypoint = _residentialDestinations.ElementAt(randomIndex);
                building = waypoint.GetComponentInParent<Residential>();
                if (building.Occupancy == 0) break;
            } while (building.Occupancy == building.Capacity);
            building.Occupancy++;
            SpawnNPC(waypoint.transform.position, waypoint.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.V) && _godMode)
        {
            DestroyNPC();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && _godMode)
        {
            _timeManager.SetTimeScale(8);
        }
        #endregion GOD_MODE

        #region Save_Load
        if (Input.GetKeyDown(KeyCode.F5))
        {
            _saveManager.SaveGame(this, "QuickSave");
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            _saveManager.LoadGame(this, "QuickSave");
        }
        #endregion Save_Load

        CalculateAverageHappiness();
        CalculatePoliticalPower();
        _timeManager.OnKeyDown();
        _timeManager.Clock();
    }
}
