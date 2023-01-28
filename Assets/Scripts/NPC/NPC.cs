using System;
using System.IO;
using UnityEditor;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    enum AssetType
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

    [Space]

    [SerializeField]
    private AssetChanger _assetChanger;

    [SerializeField]
    private Virus _virus;

    private float _triggerCounter;
    private NavMeshAgent _agent;

    private bool _isHappinessDecayActive;


    public bool IsInfected { get => _isInfected; set => _isInfected = value; }
    public float Health { get => _health; set => _health = value; }
    public float Stamina { get => _stamina; set => _stamina = value; }
    public float Happiness { get => _happiness; set => _happiness = value; }
    public float HappinessDecayRate { get => _happinessDecayRate; set => _happinessDecayRate = value; }

    private void CheckInfection()
    {
        if (_isInfected && _assetType == AssetType.Healthy)
        {
            GameObject newAsset = _assetChanger.UpdateAsset(_isInfected, transform.position, transform.rotation);
            newAsset.transform.parent = transform.parent;
            _assetType = AssetType.Infected;
            CopyTo(newAsset);
            Destroy(gameObject);
            return;
        }

        if (!_isInfected && _assetType == AssetType.Infected)
        {
            GameObject newAsset = _assetChanger.UpdateAsset(_isInfected, transform.position, transform.rotation);
            newAsset.transform.parent = transform.parent;
            _assetType = AssetType.Healthy;
            CopyTo(newAsset);
            newAsset.GetComponent<NPC>()._virus = null;
            Destroy(gameObject);
            return;
        }
    }

    private void CopyTo(GameObject other)
    {
        NPC otherNpc = other.GetComponent<NPC>();
        otherNpc._isInfected = _isInfected;
        otherNpc._health = _health;
        otherNpc._stamina = _stamina;
        otherNpc._assetChanger = _assetChanger;
        otherNpc._assetType = _assetType;
        otherNpc._happiness = _happiness;
        otherNpc._triggerCounter = _triggerCounter;
        otherNpc._virus = _virus;
        var currentDestination = gameObject.GetComponent<Navigation>().GetDestination();
        otherNpc.GetComponent<Navigation>().UpdateDestination(currentDestination);
    }

    private void UpdateStamina()
    {
        if (_agent.velocity.magnitude > 0)
        {
            float decayRate = _virus ? _virus.StaminaDecayRate : 0.2f;
            _stamina -= decayRate * Time.deltaTime;
        }
    }

    private void UpdateHealth()
    {
        if (_isInfected && _virus)
        {
            _health -= _virus.HealthDecayRate * Time.deltaTime;
        }

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateHappiness()
    {
        UpdateHappinessDecayRate();
        _happiness -= _happinessDecayRate * Time.deltaTime;
        Debug.Log("Current Happiness: " + _happiness + " - Decay Rate: " + _happinessDecayRate);
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
        //InvokeRepeating(nameof(WriteToLogFile), 2f, 2f);
    }

    void Update()
    {
        UpdateStamina();
        UpdateHealth();
        CheckInfection();
        UpdateHappiness();
    }


    /*          DEBUGING            */
    //void WriteToLogFile()
    //{
    //    string path = "Logs/log.txt";
    //    string message = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " -- Name: " + gameObject.name + ", Health: " + _health + ", Stamina: " + _stamina + ", is Infected: " + _isInfected + ", Cough Rate: " + _virus.CoughRate + " Touch Rate: " + _virus.TouchRate + ", Stamina Decay Rate: " + _virus.StaminaDecayRate + ", Health Decay Rate: " + _virus.HealthDecayRate + "\n";

    //    using System.IO.StreamWriter logFile = new System.IO.StreamWriter(@path, true);
    //    logFile.WriteLine(message);
    //}
}
