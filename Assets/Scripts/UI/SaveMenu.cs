using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    private GameManager _gameManager;
    SaveManager _saveManager = new();
    private string _saveName;
    private GameObject _saveButton;
    UIPopUp _uiPopUp;

    [SerializeField]
    private GameObject _saveMenuUI;

    public void open()
    {
        setImage();
        _saveMenuUI.SetActive(true);
        _gameManager.TimeManager.SetTimeScale(0);
    }

    public void close()
    {
        _saveMenuUI.SetActive(false);
        _gameManager.TimeManager.SetTimeScale(1);
    }

    void Awake()
    {
        _saveMenuUI.SetActive(false);
        _uiPopUp = GameObject.Find("NPCs").GetComponent<UIPopUp>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _saveButton = transform.GetChild(1).GetChild(2).gameObject;

    }

    void Update()
    {
        if(_saveName == null || _saveName == ""){
            _saveButton.GetComponent<Button>().interactable = false;
        }
        else{
            _saveButton.GetComponent<Button>().interactable = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            if (_saveMenuUI.activeSelf)
                close();
    }

    public Texture2D LoadTexture(string FilePath)
    {
        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);
            if (Tex2D.LoadImage(FileData))
                return Tex2D;
        }
        return null;
    }

    public Sprite loadSprite(string path)
    {
        Texture2D SpriteTexture = LoadTexture(path);
        Sprite newSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        return newSprite;
    }

    public void setImage()
    {
        GameObject temp = transform.GetChild(1).GetChild(4).gameObject;
        Image image = temp.GetComponent<Image>();
        Sprite mysprite = loadSprite(Application.persistentDataPath + "/images/Screenshots/" + "temp" + ".png");
        image.sprite = mysprite;
    }

    public void setSaveName(string name)
    {
        _saveName = name;
    }

    public void Save(){
        _saveManager.SaveGame(_gameManager, _saveName);
        //move screenshot to save images folder
        if (!Directory.Exists(Application.persistentDataPath + "/images/Saves/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/images/Saves/");
        File.Move(Application.persistentDataPath + "/images/Screenshots/" + "temp" + ".png", Application.persistentDataPath + "/images/Saves/" + _saveName + ".png");
        _uiPopUp.SaveLoadPopUp(_saveName +" Saved");
        close();
    }

}
