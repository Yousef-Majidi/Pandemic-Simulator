using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class InGameUI : MonoBehaviour
{
    public new AudioSource audio;
    private GameManager _gameManager;
    void Awake()
    {
        GameObject.Find("UIMenuPanel").SetActive(false);
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    void Update()
    {
        UpdateHealth();
        UpdateHappiness();
        UpdateTimeCounter();
        UpdateDayCounter();
        UpdatePoliticalPower();
    }

    void UpdateHealth()
    {
        int healthyNPCs = _gameManager.HealthyCount;
        float percentage = 0;
        if (_gameManager.NPCs.Count > 0)
        {
            percentage = (float)healthyNPCs / _gameManager.NPCs.Count;
        }

        GameObject.Find("HealthSlider").GetComponent<Slider>().value = percentage;
        GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>().text = "Healthy: " + (percentage * 100).ToString("0") + "%";
        GameObject.Find("InfectedText").GetComponent<TextMeshProUGUI>().text = "Infected: " + ((1 - percentage) * 100).ToString("0") + "%";
    }

    void UpdateHappiness()
    {
        float percentage = 0;
        if (_gameManager.NPCs.Count > 0)
        {
            percentage = _gameManager.AverageHappiness / 100;
        }

        GameObject.Find("HappinessSlider").GetComponent<UnityEngine.UI.Slider>().value = percentage;
        GameObject.Find("HappyText").GetComponent<TextMeshProUGUI>().text = "Happy: " + (percentage * 100).ToString("0") + "%";
        GameObject.Find("UnhappyText").GetComponent<TextMeshProUGUI>().text = "Unhappy: " + ((1 - percentage) * 100).ToString("0") + "%";
    }

    void UpdateTimeCounter(bool isPaused = false)
    {
        if (isPaused)
            GameObject.Find("TimeCount").GetComponent<TextMeshProUGUI>().text = "x0";
        else
            GameObject.Find("TimeCount").GetComponent<TextMeshProUGUI>().text = "x" + Time.timeScale.ToString();
    }

    void UpdateDayCounter()
    {
        GameObject.Find("Clock").GetComponent<TextMeshProUGUI>().text = _gameManager.TimeManager.InGameHour.ToString("00") + ":" + _gameManager.TimeManager.InGameMinute.ToString("00");
        GameObject.Find("DayCount").GetComponent<TextMeshProUGUI>().text = "Day " + _gameManager.TimeManager.InGameDay.ToString();
    }

    void UpdatePoliticalPower()
    {
        int PP = Mathf.RoundToInt(_gameManager.PoliticalPower);
        GameObject.Find("PoliticalPower").GetComponent<TextMeshProUGUI>().text = PP.ToString();
    }

    public void HandleUIpanel(GameObject panel)
    {
        audio.Play();
        if (panel.activeSelf)
            panel.SetActive(false);
        else
            panel.SetActive(true);
    }

    public void PauseGame()
    {
        if (Time.timeScale == 0)
        {
            _gameManager.TimeManager.SetTimeScale(1);
        }
        else
        {
            _gameManager.TimeManager.SetTimeScale(0);
            UpdateTimeCounter(true);
        }
    }

    public void FastForward()
    {
        switch (Time.timeScale)
        {
            case 0:
                _gameManager.TimeManager.SetTimeScale(1);
                break;
            case 1:
                _gameManager.TimeManager.SetTimeScale(2);
                break;
            case 2:
                _gameManager.TimeManager.SetTimeScale(3);
                break;
            case 4:
                _gameManager.TimeManager.SetTimeScale(1);
                break;
            default:
                _gameManager.TimeManager.SetTimeScale(1);
                break;
        }
    }
}
