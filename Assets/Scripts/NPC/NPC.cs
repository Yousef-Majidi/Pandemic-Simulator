using System;
using System.IO;
using UnityEditor;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Current Asset Type")]
    private AssetType _assetType;

    [SerializeField]
    [Tooltip("Character Infection Status")]
    private bool _isInfected;

    [SerializeField]
    [Tooltip("Character Health")]
    private float _health = 100f;

    [SerializeField]
    [Tooltip("Character Stamina")]
    private float _stamina = 100f;

    [SerializeField]
    [Tooltip("Character Happiness")]
    private float _happiness = 100f;

    [SerializeField]
    [Tooltip("Character Happiness Decay Rate")]
    private float _happinessDecayRate = 0f;

    [SerializeField]
    [Tooltip("Base rate for happiness decay rate")]
    private float _happinessDecayBase = 1f;

    [SerializeField]
    private Virus _virus;

    private bool _isHappinessDecayActive;
    private NavMeshAgent _agent;
    private GameManager _gameManager;

    public enum AssetType
    {
        Healthy,
        Infected
    }

    public bool IsInfected { get => _isInfected; set => _isInfected = value; }
    public float Health { get => _health; set => _health = value; }
    public float Stamina { get => _stamina; set => _stamina = value; }
    public float Happiness { get => _happiness; set => _happiness = value; }
    public float HappinessDecayRate { get => _happinessDecayRate; set => _happinessDecayRate = value; }
    public AssetType Asset { get => _assetType; set => _assetType = value; }
    public Virus Virus { get => _virus; set => _virus = value; }

    private void CheckInfection()
    {
        if ((_isInfected && _assetType == AssetType.Healthy) || (!_isInfected && _assetType == AssetType.Infected))
        {
            _gameManager.UpdateAsset(gameObject);
        }
    }

    public void Copy(GameObject source)
    {
        NPC sourceNpc = source.GetComponent<NPC>();
        _isInfected = sourceNpc._isInfected;
        _health = sourceNpc._health;
        _stamina = sourceNpc._stamina;
        _happiness = sourceNpc._happiness;
        _virus = sourceNpc._virus;
        var currentDestination = source.GetComponent<Navigation>().Destination;
        var home = source.GetComponent<Navigation>().Home;
        GetComponent<Navigation>().Home = home;
        GetComponent<Navigation>().UpdateDestination(currentDestination);
    }

    private void UpdateStamina()
    {
        if (_agent.velocity.magnitude > 0)
        {
            float decayRate = _virus ? _virus.StaminaDecayRate + _happinessDecayBase : _happinessDecayBase;
            if (_stamina > 0)
            {
                _stamina -= decayRate * Time.deltaTime;
            }
        }
    }

    private void UpdateHealth()
    {
        if (_isInfected && _virus)
        {
            _health -= _virus.HealthDecayRate * Time.deltaTime;
        }

        if (_health <= 0 && !_gameManager.GodMode)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateHappiness()
    {
        if (!_gameManager.GodMode)
        {
            UpdateHappinessDecayRate();
            _happiness -= _happinessDecayRate * Time.deltaTime;
            Debug.Log("Current Happiness: " + _happiness + " - Decay Rate: " + _happinessDecayRate);
        }
    }

    private void UpdateHappinessDecayRate()
    {
        if (_health < 25 && !_isHappinessDecayActive)
        {
            _happinessDecayRate += 1f;
            _isHappinessDecayActive = true;
        }
        else if (_health >= 25 && _isHappinessDecayActive)
        {
            _happinessDecayRate -= 1f;
            _isHappinessDecayActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC") && !_isInfected)
        {
            NPC otherNPC = other.gameObject.GetComponent<NPC>();
            if (otherNPC.IsInfected && otherNPC._virus)
            {
                otherNPC._virus.TransmitVirus(this);
            }
        }
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _assetType = IsInfected ? AssetType.Infected : AssetType.Healthy;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        UpdateStamina();
        UpdateHealth();
        CheckInfection();
        UpdateHappiness();
    }
}
