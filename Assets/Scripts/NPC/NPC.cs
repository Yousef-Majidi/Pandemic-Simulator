using System.IO;
using UnityEditor;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Character Infection Status")]
    private bool _isInfected = false;

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

    private float _triggerCounter;
    private NavMeshAgent _agent;
    // private Virus _virus = null;                      <-- To be implemented...

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

    private void SetStamina()
    {
        _stamina -= _staminaDecayRate * Time.deltaTime;
    }

    private void SetHealth()
    {
        if (_isInfected)
            _health -= _healthDecayRate * Time.deltaTime;

        if (_health <= 0)
            Destroy(gameObject);
    }

    // if in the vecinity of an infected NPC, there is a chance of infection
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            ++_triggerCounter;
            NPC npc = other.gameObject.GetComponent<NPC>();
            if (_triggerCounter == 4 && npc.IsInfected())
            {
                if (Random.Range(0f, 1f) < npc.GetTouchRate())
                    _isInfected = true;
            }
            else if (npc.IsInfected())
            {
                if (Random.Range(0f, 1f) < npc.GetCoughRate())
                    _isInfected = true;
            }
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
