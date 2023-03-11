using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class LoadMenu : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _loadMenuUI;

    // Start is called before the first frame update
    void Awake()
    {
        _loadMenuUI.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        _loadMenuUI.SetActive(true);
        _gameManager.TimeManager.SetTimeScale(0);
    }

    public void Close()
    {
        _loadMenuUI.SetActive(false);
        _gameManager.TimeManager.SetTimeScale(1);
    }

    public void FillList()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath +"/saves", "*.dat");
        GameObject content = GameObject.Find("LoadMenuContent");
        int buffer = 50;
        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            fileName = fileName.Substring(0, fileName.Length - 4);
            GameObject panel = new GameObject("Panel", typeof(RectTransform));
            panel.transform.SetParent(content.transform, false);
            panel.AddComponent<Image>();
            panel.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            panel.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(2100, 150);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panel.GetComponent<RectTransform>().anchoredPosition.y - buffer);
            buffer += 200;

            GameObject image = new GameObject("Image", typeof(RectTransform));
            image.transform.SetParent(panel.transform, false);
            image.AddComponent<Image>();
            image.GetComponent<Image>().color = Color.white;
            Texture2D texture = new Texture2D(2, 2);
            byte[] fileData;
            if (File.Exists(Application.persistentDataPath + "/images/Saves/" + fileName + ".png"))
            {
                fileData = File.ReadAllBytes(Application.persistentDataPath + "/images/Saves/" + fileName + ".png");
                texture.LoadImage(fileData);
            }
            image.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
            image.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
            image.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
            image.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            image.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
            

            GameObject text = new GameObject("Text", typeof(RectTransform));
            text.transform.SetParent(panel.transform, false);
            text.AddComponent<TextMeshProUGUI>();
            text.GetComponent<TextMeshProUGUI>().text = fileName;
            text.GetComponent<TextMeshProUGUI>().fontSize = 40;
            text.GetComponent<TextMeshProUGUI>().color = Color.black;
            text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            text.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            text.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 150);


            GameObject button = new GameObject("LoadButton", typeof(RectTransform));
            button.transform.SetParent(panel.transform, false);
            button.AddComponent<Button>();
            button.AddComponent<Image>();
            button.GetComponent<Image>().color = Color.white;
            button.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0.5f);
            button.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 150);
            //button.GetComponent<Button>().onClick.AddListener(() => LoadGame(fileName));

            GameObject buttonText = new GameObject("Text", typeof(RectTransform));
            buttonText.transform.SetParent(button.transform, false);
            buttonText.AddComponent<TextMeshProUGUI>();
            buttonText.GetComponent<TextMeshProUGUI>().text = "Load";
            buttonText.GetComponent<TextMeshProUGUI>().fontSize = 40;
            buttonText.GetComponent<TextMeshProUGUI>().color = Color.black;
            buttonText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            buttonText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            buttonText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            buttonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            buttonText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 150);

        }
    }
}
