using System.IO;
using UnityEditor;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

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

    [Space]

    [SerializeField]
    private AssetChanger _assetChanger;

    [SerializeField]
    private Virus _virus;

    private float _triggerCounter;
    private NavMeshAgent _agent;


    public bool IsInfected { get => _isInfected; set => _isInfected = value; }
    public float Health { get => _health; set => _health = value; }
    public float Stamina { get => _stamina; set => _stamina = value; }
    public float Happiness { get => _happiness; set => _happiness = value; }

    private void CheckInfection()
    {
        if (_isInfected && _assetType == AssetType.Healthy)
        {
            GameObject newAsset = _assetChanger.UpdateAsset(_isInfected, transform.position, transform.rotation);
            newAsset.transform.parent = transform.parent;
            _assetType = AssetType.Infected;
            CopyTo(newAsset);
            if (Random.Range(0f, 1f) < _virus.MutationChance)
            {
                Virus newVirus = ScriptableObject.CreateInstance<Virus>();
                newVirus.Mutate();
                _virus = newVirus;
            }
            Destroy(gameObject);
            return;
        }

        if (!_isInfected && _assetType == AssetType.Infected)
        {
            GameObject newAsset = _assetChanger.UpdateAsset(_isInfected, transform.position, transform.rotation);
            newAsset.transform.parent = transform.parent;
            _assetType = AssetType.Healthy;
            CopyTo(newAsset);
            // TODO: the virus should be set to null at this point
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
            {
                if (Random.Range(0f, 1f) < npc._virus.TouchRate)
                    _isInfected = true;
            }
            else if (npc.IsInfected)
                if (Random.Range(0f, 1f) < npc._virus.CoughRate)
                    _isInfected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            // TODO: potential bug here
            if (_triggerCounter == 4)
                _triggerCounter = 0;
            else if (_triggerCounter > 0)
                --_triggerCounter;
        }
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _assetType = IsInfected ? AssetType.Infected : AssetType.Healthy;
        InvokeRepeating(nameof(WriteToLogFile), 2f, 2f);
        OnMouseDown();
    }

    void Update()
    {
        UpdateStamina();
        UpdateHealth();
        CheckInfection();
    }


    /*          DEBUGING            */
    void WriteToLogFile()
    {
        string path = "Logs/log.txt";
        string message = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " -- Name: " + gameObject.name + ", Health: " + _health + ", Stamina: " + _stamina + ", is Infected: " + _isInfected + ", Cough Rate: " + _virus.CoughRate + " Touch Rate: " + _virus.TouchRate + ", Stamina Decay Rate: " + _virus.StaminaDecayRate + ", Health Decay Rate: " + _virus.HealthDecayRate + "\n";

        using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@path, true))
            logFile.WriteLine(message);
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.CompareTag("NPC"))
                {
                    NpcPopUp(hit.collider.gameObject);
                }
            }
        }
    }

    void NpcPopUp(GameObject obj)
    {
        // see if there is already a canvas
        GameObject canvasTemp = GameObject.Find("CanvasPop");
        if (canvasTemp != null)
        {
            Destroy(canvasTemp);
            return;
        }

        // create canvas
        GameObject canvas = new GameObject("CanvasPop", typeof(RectTransform));
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
        

        // create panel
        GameObject panel = new GameObject("Panel", typeof(RectTransform));
        panel.transform.SetParent(canvas.transform, false);
        panel.AddComponent<Image>();

        //create button
        GameObject exitButton = new GameObject("Button", typeof(RectTransform));
        exitButton.transform.SetParent(panel.transform, false);
        exitButton.AddComponent<Button>();
        exitButton.AddComponent<Image>();
        exitButton.GetComponent<Button>().onClick.AddListener(() => Destroy(canvas));

        // add text from TextMeshPro to button 
        GameObject buttonText = new GameObject("Text", typeof(RectTransform));
        buttonText.transform.SetParent(exitButton.transform, false);
        buttonText.AddComponent<TextMeshProUGUI>();

        //create text
        GameObject text = new GameObject("Text", typeof(RectTransform));
        text.transform.SetParent(panel.transform, false);
        text.AddComponent<TextMeshProUGUI>();

        // set canvas to only take up the bottom left corner of the display 
        canvas.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        canvas.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        canvas.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        canvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        // set panel properties to be only top left corner of canvas
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(900, 900);
        panel.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
        panel.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
        panel.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        panel.GetComponent<Image>().color = Color.white;


        exitButton.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        exitButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        exitButton.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        exitButton.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        exitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
        exitButton.GetComponent<RectTransform>().SetAsLastSibling();
        exitButton.GetComponent<Image>().color = Color.red;


        //set button text properties
        buttonText.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        buttonText.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        buttonText.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        buttonText.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        buttonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        buttonText.GetComponent<TextMeshProUGUI>().text = "X";
        buttonText.GetComponent<TextMeshProUGUI>().color = Color.black;
        buttonText.GetComponent<TextMeshProUGUI>().fontSize = 85;
        buttonText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        // set text properties and place in center of panel
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(900, 900);
        text.GetComponent<TextMeshProUGUI>().text = "Name: " + obj.name + "\nHealth: " + obj.GetComponent<NPC>()._health + "\nStamina: " + obj.GetComponent<NPC>()._stamina + "\nInfected: " + obj.GetComponent<NPC>()._isInfected;
        text.GetComponent<TextMeshProUGUI>().color = Color.black;
        text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        text.GetComponent<TextMeshProUGUI>().fontSize = 75;
        text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        text.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }
}
