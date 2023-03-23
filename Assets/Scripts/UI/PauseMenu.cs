using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PauseMenu : MonoBehaviour
{
    public new AudioSource audio;

    private GameManager _gameManager;

    [SerializeField]
    private static bool _gameIsPaused = false;

    [SerializeField]
    private GameObject _pauseMenuUI;

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        _gameManager.TimeManager.SetTimeScale(1);
        _gameIsPaused = false;
    }

    public void Pause()
    {
        _pauseMenuUI.SetActive(true);
        _gameManager.TimeManager.SetTimeScale(0);
        _gameIsPaused = true;
    }

    void Awake()
    {
        _pauseMenuUI.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // take screenshot
    public void TakeScreenshot()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/images/Screenshots/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/images/Screenshots/");
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/images/Screenshots/" + "temp" + ".png");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameIsPaused)
                Resume();
            else{
                TakeScreenshot();
                audio.Play();
                Pause();
            }
        }
    }

    public void openMenu()
    {
        _pauseMenuUI.SetActive(true);
        Pause();
    }
}
