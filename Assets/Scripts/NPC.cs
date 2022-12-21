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

    // if touching an infected NPC, there is a chance of infection
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision with NPC");
        // if collision tag is NPC
        if (other.gameObject.CompareTag("NPC"))
        {
            NPC npc = other.gameObject.GetComponent<NPC>();
            if (npc.IsInfected())
                if (Random.Range(0f, 1f) < npc.GetTouchRate())
                    _isInfected = true;
        }
    }

    // if in the vecinity of an infected NPC, there is a chance of infection
    private void OnTriggerEnter(Collider other)
    {
        // if collision tag is NPC
        if (other.gameObject.CompareTag("NPC"))
        {
            Debug.Log("NPC: OnTriggerEnter");
            NPC npc = other.gameObject.GetComponent<NPC>();
            if (npc.IsInfected() && npc != null)
                if (Random.Range(0f, 1f) < npc.GetCoughRate())
                    _isInfected = true;
        }
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        InvokeRepeating(nameof(WriteToLogFile), 2f, 2f);
    }

    void Update()
    {
        // if walking, decrease stamina by _staminaDecayRate
        if (_agent.velocity.magnitude > 0)
            _stamina -= _staminaDecayRate * Time.deltaTime;

        // if infected, reduce health by _healthDecayRate
        if (_isInfected)
            _health -= _healthDecayRate * Time.deltaTime;

        // if health is 0, destroy the NPC
        if (_health <= 0)
            Destroy(gameObject);
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
