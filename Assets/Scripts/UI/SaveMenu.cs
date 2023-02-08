using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _saveMenuUI;

    public void open()
    {
        setImage();
        _saveMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void close()
    {
        _saveMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Awake()
    {
        _saveMenuUI.SetActive(false);

    }

    void Update()
    {
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

        Sprite mysprite = loadSprite(Application.dataPath + "/images/Screenshots/" + "temp" + ".png");
        image.sprite = mysprite;
    }

}
