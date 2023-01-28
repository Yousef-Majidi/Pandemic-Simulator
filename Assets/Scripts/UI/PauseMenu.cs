using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private static bool _gameIsPaused = false;

    [SerializeField]
    private GameObject _pauseMenuUI;

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _gameIsPaused = false;
    }

    public void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _gameIsPaused = true;
    }

    void Awake()
    {
        _pauseMenuUI.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (_gameIsPaused)
                Resume();
            else
                Pause();
    }

    public void openMenu()
    {
        _pauseMenuUI.SetActive(true);
        Pause();
    }
}
