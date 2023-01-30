using System;
using System.IO;
using UnityEditor;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public enum AssetType
    {
        Healthy,
        Infected
    }

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

    private bool _isHappinessDecayActive;

    [Space]

    [SerializeField]
    private Virus _virus;

    private float _triggerCounter;
    private NavMeshAgent _agent;

    GameManager _gameManager;

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
        _triggerCounter = sourceNpc._triggerCounter;
        _virus = sourceNpc._virus;
        var currentDestination = source.GetComponent<Navigation>().GetDestination();
        GetComponent<Navigation>().UpdateDestination(currentDestination);
    }

    private void UpdateStamina()
    {
        if (_agent.velocity.magnitude > 0)
        {
            float decayRate = _virus ? _virus.StaminaDecayRate + 1f : 1f;
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

        if (_health <= 0 && _gameManager.GodMode == false)
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
            ++_triggerCounter;
            NPC npc = other.gameObject.GetComponent<NPC>();
            if (_triggerCounter == 4 && npc.IsInfected && npc._virus && UnityEngine.Random.Range(0f, 1f) < npc._virus.TouchRate)
            {
                _isInfected = true;
            }
            else if (npc.IsInfected && npc._virus && UnityEngine.Random.Range(0f, 1f) < npc._virus.CoughRate)
            {
                _isInfected = true;
            }
            if (_isInfected && npc._virus)
            {
                _virus = ScriptableObject.CreateInstance<Virus>();
                if (UnityEngine.Random.Range(0f, 1f) < npc._virus.MutationChance)
                {
                    _virus.Mutate();
                }
                else
                {
                    _virus.Copy(npc._virus);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            // TODO: potential bug here
            if (_triggerCounter == 4)
            {
                _triggerCounter = 0;
            }
            else if (_triggerCounter > 0)
            {
                --_triggerCounter;
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
