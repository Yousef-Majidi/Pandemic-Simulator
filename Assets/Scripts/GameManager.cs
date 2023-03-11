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
    [Tooltip("Number of npcs to spawn on start")]
    private int _startNpcs;

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

    [Space]
    [Header("Time Manager")]

    [SerializeField]
    private TimeManager _timeManager;

    [Space]
    [Header("Decisions")]

    [SerializeField]
    [Tooltip("The list of all decisions")]
    private List<Decision> _decisionList = new();

    SaveManager _saveManager = new();

    UIPopUp _uiPopUp;

    private LinkedList<GameObject> _commercialDestinations = new();
    private LinkedList<GameObject> _residentialDestinations = new();
    private LinkedList<GameObject> _medicalDestinations = new();
    private LinkedList<GameObject> _npcs = new();

    public bool GodMode { get => _godMode; set => _godMode = value; }
    public int MaxNPCs { get => _maxNPC; set => _maxNPC = value; }
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
    public LinkedList<GameObject> CommercialDestinations { get => _commercialDestinations; }
    public LinkedList<GameObject> MedicalDestinations { get => _medicalDestinations; }
    public LinkedList<GameObject> ResidentialDestinations { get => _residentialDestinations; }
    public LinkedList<GameObject> NPCs { get => _npcs; }
    public List<Decision> DecisionList { get => _decisionList; }

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

    public GameObject SpawnNPC(Vector3 position, Quaternion rotation)
    {
        if (_npcs.Count < _maxNPC)
        {
            GameObject obj = Instantiate(_healthyPrefab, position, rotation);
            obj.transform.parent = GameObject.Find("NPCs").transform;
            obj.GetComponent<Navigation>().SetHome(position);
            obj.tag = "NPC";
            _npcs.AddFirst(obj);
            return obj;
        }
        return null;
    }
    public void UpdateAsset(GameObject npc)
    {
        GameObject obj;
        if (npc.GetComponent<NPC>().IsInfected)
        {
            obj = _assetChanger.UpdateAsset(_infectedPrefab, npc.transform.position, npc.transform.rotation);
            obj.GetComponent<NPC>().Asset = NPC.AssetType.Infected;
        }
        else
        {
            obj = _assetChanger.UpdateAsset(_healthyPrefab, npc.transform.position, npc.transform.rotation);
            obj.GetComponent<NPC>().Asset = NPC.AssetType.Healthy;
            obj.GetComponent<NPC>().Virus = null;
        }
        obj.transform.parent = npc.transform.parent;
        obj.GetComponent<NPC>().Copy(npc.GetComponent<NPC>());
        _npcs.Remove(npc);
        _npcs.AddFirst(obj);
        Destroy(npc);
    }

    public void CalculatePoliticalPower()
    {
        if (!float.IsNaN(_averageHappiness))
            _politicalPower += _averageHappiness * Time.deltaTime * _politicalPowerMultiplier;
    }

    void Awake()
    {
        _uiPopUp = GameObject.Find("NPCs").GetComponent<UIPopUp>();
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
        }

        for (int i = 0; i < _startNpcs; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, _residentialDestinations.Count);
            GameObject spawnPoint = _residentialDestinations.ElementAt(randomIndex);
            SpawnNPC(spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
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
            GameObject spawnPoint = _residentialDestinations.ElementAt(randomIndex);
            SpawnNPC(spawnPoint.transform.position, spawnPoint.transform.rotation);
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
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/images/Saves/" + "QuickSave" + ".png");
            _uiPopUp.SaveLoadPopUp("Quick Saved");
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            _saveManager.LoadGame(this, "QuickSave");
            _uiPopUp.SaveLoadPopUp("Quick Loaded");
        }
        #endregion Save_Load

        CalculateAverageHappiness();
        CalculatePoliticalPower();
        _timeManager.OnKeyDown();
        _timeManager.Clock();
        _npcCount = _npcs.Count;
    }
}
