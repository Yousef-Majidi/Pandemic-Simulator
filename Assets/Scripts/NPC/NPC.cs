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

    [Header("Prefab assets that will be used to change the NPC's appearance")]

    [SerializeField]
    private GameObject _healthyAsset;

    [SerializeField]
    private GameObject _infectedAsset;

    [SerializeField]
    private AssetType _assetType;

    private float _triggerCounter;
    private NavMeshAgent _agent;

    [SerializeField]
    private Virus _virus;

    public bool IsInfected { get => _isInfected; set => _isInfected = value; }
    public float Health { get => _health; set => _health = value; }
    public float Stamina { get => _stamina; set => _stamina = value; }
    public float Happiness { get => _happiness; set => _happiness = value; }

    private void UpdateAsset()
    {
        if (_isInfected && _assetType == AssetType.Healthy)
        {
            _assetType = AssetType.Infected;
            GameObject newAsset = Instantiate(_infectedAsset, transform.position, transform.rotation);
            newAsset.transform.parent = transform.parent;
            CopyTo(newAsset);
            if (Random.Range(0f, 1f) < _virus.MutationChance)
            {
                Virus newVirus = ScriptableObject.CreateInstance<Virus>();
                newVirus.Mutate();
                _virus = newVirus;
                Debug.Log("After mutation: Cough Rate: " + newVirus.CoughRate + " - Touch Rate: " + newVirus.TouchRate + " - Stamina Decay Rate: " + newVirus.StaminaDecayRate + " - Health Decay Rate: " + newVirus.HealthDecayRate + " - Mutation Chance: " + newVirus.MutationChance);
            }
            Destroy(gameObject);
            return;
        }

        if (!_isInfected && _assetType == AssetType.Infected)
        {
            _assetType = AssetType.Healthy;
            GameObject newAsset = Instantiate(_healthyAsset, transform.position, transform.rotation);
            newAsset.transform.parent = transform.parent;
            CopyTo(newAsset);
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
        otherNpc._assetType = _assetType;
        otherNpc._happiness = _happiness;
        otherNpc._triggerCounter = _triggerCounter;
        otherNpc._virus = _virus;
        var currentDestination = gameObject.GetComponent<Navigation>().GetDestination();
        other.GetComponent<Navigation>().SetDestination(currentDestination);
    }



    private void UpdateStamina()
    {
        if (_agent.velocity.magnitude > 0)
            _stamina -= _virus.StaminaDecayRate * Time.deltaTime;
    }

    private void UpdateHealth()
    {
        if (_isInfected)
            _health -= _virus.HealthDecayRate * Time.deltaTime;

        if (_health <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            ++_triggerCounter;
            NPC npc = other.gameObject.GetComponent<NPC>();
            if (_triggerCounter == 4 && npc.IsInfected)
                if (Random.Range(0f, 1f) < npc._virus.TouchRate)
                    _isInfected = true;
                else if (npc.IsInfected)
                    if (Random.Range(0f, 1f) < npc._virus.CoughRate)
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
        UpdateStamina();
        UpdateHealth();
        UpdateAsset();
    }


    /*          DEBUGING            */
    void WriteToLogFile()
    {
        string path = "Logs/log.txt";
        string message = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " -- Name: " + gameObject.name + ", Health: " + _health + ", Stamina: " + _stamina + ", is Infected: " + _isInfected + ", Cough Rate: " + _virus.CoughRate + " Touch Rate: " + _virus.TouchRate + ", Stamina Decay Rate: " + _virus.StaminaDecayRate + ", Health Decay Rate: " + _virus.HealthDecayRate + "\n";

        using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@path, true))
            logFile.WriteLine(message);
    }
}
