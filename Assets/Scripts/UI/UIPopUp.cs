using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopUp : MonoBehaviour
{
    public new AudioSource audio;
    private bool isPopUp = false;
    private GameObject _tempObj;
    private GameManager _gameManager;
    private int _intervalMin = 0;
    private int _intervalHour = 0;
    private int _intervalDay = 0;
    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        OnMouseDown();
        // add text if there is a canvas
        GameObject canvasTemp = GameObject.Find("CanvasPop");
        if (canvasTemp != null && _gameManager.TimeManager.TimeScale != 0)
        {
            GameObject text = GameObject.Find("PopUpText");
            addText();
        }

        GameObject saveCanTemp = GameObject.Find("SaveCanvasPop");
        if (saveCanTemp != null)
        {
            if (_intervalMin <= _gameManager.TimeManager.InGameMinute && _intervalHour <= _gameManager.TimeManager.InGameHour && _intervalDay <= _gameManager.TimeManager.InGameDay)
            {
                Destroy(saveCanTemp);
            }
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("NPC"))
                {
                    audio.Play();
                    NpcPopUp(hit.collider.gameObject);
                    isPopUp = true;
                }
                else
                {
                    if (isPopUp)
                    {
                        GameObject canvasTemp = GameObject.Find("CanvasPop");
                        if (canvasTemp != null)
                        {
                            Destroy(canvasTemp);
                            isPopUp = false;
                        }
                    }
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
        }
        _tempObj = obj;

        // create canvas
        GameObject canvas = new GameObject("CanvasPop", typeof(RectTransform));
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();


        // create panel
        GameObject panel = new GameObject("PopUpPanel", typeof(RectTransform));
        panel.transform.SetParent(canvas.transform, false);
        panel.AddComponent<Image>();

        Transform currentDestination = _tempObj.GetComponent<Navigation>().Destination;
        string destination = currentDestination.ToString();
        destination = destination.Substring(0, destination.Length - 23);
        GameObject text = new GameObject("PopUpText", typeof(RectTransform));
        text.transform.SetParent(panel.transform, false);
        text.AddComponent<TextMeshProUGUI>();
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        text.GetComponent<TextMeshProUGUI>().text = "Name: " + _tempObj.GetComponent<NPC>().Name + "\nHealth: " + _tempObj.GetComponent<NPC>().Health.ToString("F2") + "\nStamina: " + _tempObj.GetComponent<NPC>().Stamina.ToString("F2") + "\n Happiness: " + _tempObj.GetComponent<NPC>().Happiness.ToString("F2") + "\nDestination: " + destination + "\nInfected: " + _tempObj.GetComponent<NPC>().IsInfected;
        text.GetComponent<TextMeshProUGUI>().color = Color.black;
        text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        text.GetComponent<TextMeshProUGUI>().fontSize = 16;
        text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        text.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        //create button
        GameObject exitButton = new GameObject("Button", typeof(RectTransform));
        exitButton.transform.SetParent(panel.transform, false);
        exitButton.AddComponent<Button>();
        exitButton.AddComponent<Image>();
        exitButton.GetComponent<Button>().onClick.AddListener(() => Destroy(canvas));

        // add text from TextMeshPro to button 
        GameObject buttonText = new GameObject("ButtonText", typeof(RectTransform));
        buttonText.transform.SetParent(exitButton.transform, false);
        buttonText.AddComponent<TextMeshProUGUI>();

        // set canvas to only take up the bottom left corner of the display 
        canvas.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        canvas.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        canvas.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        canvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // set panel properties to be only top left corner of canvas
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        panel.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        panel.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        panel.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 30);
        panel.GetComponent<Image>().color = 0.75f * Color.white;


        exitButton.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        exitButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        exitButton.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        exitButton.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        exitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50);
        exitButton.GetComponent<RectTransform>().SetAsLastSibling();
        exitButton.GetComponent<Image>().color = Color.red;


        //set button text properties
        buttonText.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        buttonText.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        buttonText.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        buttonText.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        buttonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        buttonText.GetComponent<TextMeshProUGUI>().text = "X";
        buttonText.GetComponent<TextMeshProUGUI>().color = Color.black;
        buttonText.GetComponent<TextMeshProUGUI>().fontSize = 44;
        buttonText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
    }

    void addText()
    {
        GameObject text = GameObject.Find("PopUpText");
        if (text != null)
        {
            Transform currentDestination = _tempObj.GetComponent<Navigation>().Destination;
            string destination = currentDestination.ToString();
            destination = destination.Substring(0, destination.Length - 23);
            text.GetComponent<TextMeshProUGUI>().text = "Name: " + _tempObj.GetComponent<NPC>().Name + "\nHealth: " + _tempObj.GetComponent<NPC>().Health.ToString("F2") + "\nStamina: " + _tempObj.GetComponent<NPC>().Stamina.ToString("F2") + "\n Happiness: " + _tempObj.GetComponent<NPC>().Happiness.ToString("F2") + "\nDestination: " + destination + "\nInfected: " + _tempObj.GetComponent<NPC>().IsInfected;
        }
    }

    public void SaveLoadPopUp(string text)
    {
        GameObject canvasTemp = GameObject.Find("SaveCanvasPop");
        if (canvasTemp != null)
        {
            Destroy(canvasTemp);
        }

        GameObject canvas = new GameObject("SaveCanvasPop", typeof(RectTransform));
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();

        GameObject panel = new GameObject("Panel", typeof(RectTransform));
        panel.transform.SetParent(canvas.transform, false);
        panel.AddComponent<Image>();

        GameObject textObj = new GameObject("Text", typeof(RectTransform));
        textObj.transform.SetParent(panel.transform, false);
        textObj.AddComponent<TextMeshProUGUI>();

        canvas.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
        canvas.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
        canvas.GetComponent<RectTransform>().pivot = new Vector2(1, 0);
        canvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 50);
        panel.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0.5f);
        panel.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);
        panel.GetComponent<RectTransform>().pivot = new Vector2(1, 0);
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -150);
        panel.GetComponent<Image>().color = 0 * Color.white;

        textObj.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 50);
        textObj.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        textObj.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        textObj.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        textObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        textObj.GetComponent<TextMeshProUGUI>().text = text;
        textObj.GetComponent<TextMeshProUGUI>().color = Color.black;
        textObj.GetComponent<TextMeshProUGUI>().fontSize = 16;
        textObj.GetComponent<TextMeshProUGUI>().font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Roboto-Bold SDF");
        textObj.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        _intervalMin = _gameManager.TimeManager.InGameMinute + 2;
        _intervalHour = _gameManager.TimeManager.InGameHour;
        _intervalDay = _gameManager.TimeManager.InGameDay;
        if (_intervalMin >= 60)
        {
            _intervalHour = _gameManager.TimeManager.InGameHour + 1;
            _intervalMin = 0;
            if (_intervalHour >= 24)
            {
                _intervalDay = _gameManager.TimeManager.InGameDay + 1;
                _intervalHour = 0;
            }
        }
    }

}
