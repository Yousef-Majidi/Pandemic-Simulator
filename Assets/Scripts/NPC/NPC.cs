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

    [Space]

    [Header("The following attributes will be removed and retrieved from the Virus class when it is implemented")]

    [SerializeField]
    [Tooltip("Character Cough Rate: Chance of spreading infection by being near another NPC")]
    private float _coughRate = 0.5f;

    [SerializeField]
    [Tooltip("Character Touch Rate: Chance of spreading infection by touching another NPC")]
    private float _touchRate = 0.5f;

    [SerializeField]
    [Tooltip("Character Stamina Decay Rate")]
    private float _staminaDecayRate = 0.5f;

    [SerializeField]
    [Tooltip("Character Health Decay Rate")]
    private float _healthDecayRate = 0.5f;

    [Space]

    [Header("Prefab assets that will be used to change the NPC's appearance")]

    [SerializeField]
    private GameObject _healthyAsset;

    [SerializeField]
    private GameObject _infectedAsset;

    [SerializeField]
    private AssetType _assetType;

    private float _triggerCounter;
    private NavMeshAgent _agent;
    // private Virus _virus = null;                      <-- To be implemented...

    private void ChangeAsset()
    {
        if (_isInfected && _assetType == AssetType.Healthy)
        {
            Debug.Log("Changing to infected asset");
            _assetType = AssetType.Infected;
            GameObject newAsset = Instantiate(_infectedAsset, transform.position, transform.rotation);
            newAsset.transform.parent = transform.parent;
            CopyTo(newAsset);
            Destroy(gameObject);
            Debug.Log("Destorying: " + gameObject.name);
            return;
        }

        if (!_isInfected && _assetType == AssetType.Infected)
        {
            Debug.Log("Changing to healthy asset");
            _assetType = AssetType.Healthy;
            GameObject newAsset = Instantiate(_healthyAsset, transform.position, transform.rotation);
            newAsset.transform.parent = transform.parent;
            CopyTo(newAsset);
            Destroy(gameObject);
            Debug.Log("Destorying: " + gameObject.name);
            return;
        }
    }

    private void CopyTo(GameObject other)
    {
        NPC otherNpc = other.GetComponent<NPC>();
        otherNpc._isInfected = _isInfected;
        otherNpc._health = _health;
        otherNpc._stamina = _stamina;
        otherNpc._assetType = _assetType;
        otherNpc._coughRate = _coughRate;
        otherNpc._touchRate = _touchRate;
        otherNpc._happiness = _happiness;
        otherNpc._staminaDecayRate = _staminaDecayRate;
        otherNpc._healthDecayRate = _healthDecayRate;
        otherNpc._triggerCounter = _triggerCounter;
        otherNpc._agent = _agent;
        var currentDestination = gameObject.GetComponent<Navigation>().GetDestination();
        other.GetComponent<Navigation>().SetDestination(currentDestination);
    }

    public bool IsInfected()
    {
        return _isInfected;
    }

    public float GetCoughRate()
    {
        return _coughRate;
    }

    public float GetTouchRate()
    {
        return _touchRate;
    }

    public float GetHealth()
    {
        return _health;
    }

    public float GetStamin()
    {
        return _stamina;
    }

    public void SetStamina()
    {
        if (_agent.velocity.magnitude > 0)
            _stamina -= _staminaDecayRate * Time.deltaTime;
    }

    public void SetHealth()
    {
        if (_isInfected)
            _health -= _healthDecayRate * Time.deltaTime;

        if (_health <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            ++_triggerCounter;
            NPC npc = other.gameObject.GetComponent<NPC>();
            if (_triggerCounter == 4 && npc.IsInfected())
                if (Random.Range(0f, 1f) < npc.GetTouchRate())
                    _isInfected = true;
                else if (npc.IsInfected())
                    if (Random.Range(0f, 1f) < npc.GetCoughRate())
                        _isInfected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            if (_triggerCounter == 4)
                _triggerCounter = 0;
            else if (_triggerCounter > 0)
                --_triggerCounter;
        }
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        InvokeRepeating(nameof(WriteToLogFile), 2f, 2f);
    }

    void Update()
    {
        SetStamina();
        SetHealth();
        ChangeAsset();
    }


    /*          DEBUGING            */
    void WriteToLogFile()
    {
        string path = "Logs/log.txt";
        string message = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " -- Name: " + gameObject.name + ", Health: " + _health + ", Stamina: " + _stamina + ", is Infected: " + _isInfected + ", Cough Rate: " + _coughRate + " Touch Rate: " + _touchRate + ", Stamina Decay Rate: " + _staminaDecayRate + ", Health Decay Rate: " + _healthDecayRate + "\n";

        using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@path, true))
            logFile.WriteLine(message);
    }
}
