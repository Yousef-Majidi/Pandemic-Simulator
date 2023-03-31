using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadMenu : MonoBehaviour
{
    private GameManager _gameManager;
    private SaveManager _saveManager = new();
    private MainMenuScripts _mainMenuScripts;
    UIPopUp _uiPopUp;

    [SerializeField]
    private string _savePath;

    [SerializeField]
    private string _imagePath;


    [SerializeField]
    private GameObject _loadMenuUI;

    private GameObject _toBeDestroyed;

    // Start is called before the first frame update
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "LoadGameMenu")
        {
            _loadMenuUI.SetActive(true);
            FillList();
            _mainMenuScripts = GameObject.Find("LevelLoader").GetComponent<MainMenuScripts>();
            _toBeDestroyed = this.gameObject;
            DontDestroyOnLoad(_toBeDestroyed);
        }
        else
        {
            _loadMenuUI.SetActive(false);
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _uiPopUp = GameObject.Find("NPCs").GetComponent<UIPopUp>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        _loadMenuUI.SetActive(true);
        _gameManager.TimeManager.SetTimeScale(0);
        FillList();
    }

    public void Close()
    {
        _loadMenuUI.SetActive(false);
        _gameManager.TimeManager.SetTimeScale(1);
    }

    public void FillList()
    {
        if (_savePath == null || _savePath == ""){
            _savePath = Application.persistentDataPath + "/saves/";
        }
        string[] files = Directory.GetFiles(_savePath, "*.dat");
        GameObject content = GameObject.Find("LoadMenuContent");
        GameObject ScrollView = GameObject.Find("Scroll ViewLoad");
        int buffer = 50;
        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string fileNameNoExten = fileName.Substring(0, fileName.Length - 4);
            GameObject panel = new GameObject("Panel", typeof(RectTransform));
            panel.transform.SetParent(content.transform, false);
            panel.AddComponent<Image>();
            panel.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            panel.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(ScrollView.GetComponent<RectTransform>().rect.width - 25, 75);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panel.GetComponent<RectTransform>().anchoredPosition.y - buffer);
            buffer += 100;

            GameObject image = new GameObject("Image", typeof(RectTransform));
            image.transform.SetParent(panel.transform, false);
            image.AddComponent<Image>();
            image.GetComponent<Image>().color = Color.white;
            Texture2D texture = new Texture2D(2, 2);
            byte[] fileData;
            if (_imagePath == null || _imagePath == "")
                _imagePath = Application.persistentDataPath + "/images/Saves/";
            if (File.Exists(_imagePath + fileNameNoExten + ".png"))
            {
                fileData = File.ReadAllBytes(_imagePath + fileNameNoExten + ".png");
                texture.LoadImage(fileData);
            }
            image.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
            image.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
            image.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
            image.GetComponent<RectTransform>().anchoredPosition = new Vector2(75, 0);
            image.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 55);

            SaveManager.GameDisplayData data = _saveManager.LoadDisplay(fileName);
            GameObject textRow1 = new GameObject("Text", typeof(RectTransform));
            textRow1.transform.SetParent(panel.transform, false);
            textRow1.AddComponent<TextMeshProUGUI>();
            textRow1.GetComponent<TextMeshProUGUI>().text = fileNameNoExten + "\nDay:" + data._inGameDay + "\nTime:" + data._inGameHour.ToString("00") +":" + data._inGameMinute.ToString("00");
            textRow1.GetComponent<TextMeshProUGUI>().fontSize = 15;
            textRow1.GetComponent<TextMeshProUGUI>().color = Color.black;
            textRow1.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
            textRow1.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            textRow1.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            textRow1.GetComponent<RectTransform>().anchoredPosition = new Vector2(-125, 0);
            textRow1.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 75);

            GameObject textRow2 = new GameObject("Text", typeof(RectTransform));
            textRow2.transform.SetParent(panel.transform, false);
            textRow2.AddComponent<TextMeshProUGUI>();
            //round political power to 0 decimal places
            data._politicalPower = Mathf.RoundToInt(data._politicalPower);
            textRow2.GetComponent<TextMeshProUGUI>().text = "P Power:" + data._politicalPower + "\nPopulation:" + data._population + "\nTotal Infected:" + data._totalInfected;
            textRow2.GetComponent<TextMeshProUGUI>().fontSize = 15;
            textRow2.GetComponent<TextMeshProUGUI>().color = Color.black;
            textRow2.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
            textRow2.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            textRow2.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            textRow2.GetComponent<RectTransform>().anchoredPosition = new Vector2(-25, 0);
            textRow2.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 75);



            GameObject loadButton = new GameObject("LoadButton", typeof(RectTransform));
            loadButton.transform.SetParent(panel.transform, false);
            loadButton.AddComponent<Button>();
            loadButton.AddComponent<Image>();
            loadButton.GetComponent<Image>().color = Color.green;
            loadButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0.5f);
            loadButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);
            loadButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150,0);
            loadButton.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 50);
            loadButton.GetComponent<Button>().onClick.AddListener(() => {
                if (SceneManager.GetActiveScene().name == "LoadGameMenu"){
                    _mainMenuScripts.ChangeSceneWTransition("City");
                    if(SceneManager.GetActiveScene().name != "City"){
                        StartCoroutine(WaitForSceneLoad("City", fileNameNoExten));
                    }
                }
                else{
                    _saveManager.LoadGame(_gameManager,fileNameNoExten);
                    _loadMenuUI.SetActive(false);
                    _uiPopUp.SaveLoadPopUp(fileNameNoExten+" Loaded");
                    _gameManager.TimeManager.SetTimeScale(0);
                }

            });

            GameObject deleteButton = new GameObject("DeleteButton", typeof(RectTransform));
            deleteButton.transform.SetParent(panel.transform, false);
            deleteButton.AddComponent<Button>();
            deleteButton.AddComponent<Image>();
            deleteButton.GetComponent<Image>().color = Color.red;
            deleteButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0.5f);
            deleteButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);
            deleteButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 0);
            deleteButton.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 50);
            deleteButton.GetComponent<Button>().onClick.AddListener(() =>{
                _saveManager.RemoveSave(_savePath + fileNameNoExten + ".dat", _imagePath + fileNameNoExten + ".png");
                Destroy(panel);
            });

            GameObject deleteButtonText = new GameObject("Text", typeof(RectTransform));
            deleteButtonText.transform.SetParent(deleteButton.transform, false);
            deleteButtonText.AddComponent<TextMeshProUGUI>();
            deleteButtonText.GetComponent<TextMeshProUGUI>().text = "Delete";
            deleteButtonText.GetComponent<TextMeshProUGUI>().fontSize = 15;
            deleteButtonText.GetComponent<TextMeshProUGUI>().color = Color.black;
            deleteButtonText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            deleteButtonText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            deleteButtonText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            deleteButtonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            deleteButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 50);


            GameObject loadButtonText = new GameObject("Text", typeof(RectTransform));
            loadButtonText.transform.SetParent(loadButton.transform, false);
            loadButtonText.AddComponent<TextMeshProUGUI>();
            loadButtonText.GetComponent<TextMeshProUGUI>().text = "Load";
            loadButtonText.GetComponent<TextMeshProUGUI>().fontSize = 15;
            loadButtonText.GetComponent<TextMeshProUGUI>().color = Color.black;
            loadButtonText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            loadButtonText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            loadButtonText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            loadButtonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            loadButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 50);

        }
    }

    IEnumerator WaitForSceneLoad(string sceneName, string fileName){
        while (SceneManager.GetActiveScene().name != sceneName){
            yield return null;
        }
        if (SceneManager.GetActiveScene().name == sceneName){
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _uiPopUp = GameObject.Find("NPCs").GetComponent<UIPopUp>();
            _saveManager.LoadGame(_gameManager, fileName);
            _loadMenuUI.SetActive(false);
            _gameManager.TimeManager.SetTimeScale(1);
            Destroy(_toBeDestroyed);
        }
    }

    public void DestroyOnClick(){
        Destroy(_toBeDestroyed);
    }
}
