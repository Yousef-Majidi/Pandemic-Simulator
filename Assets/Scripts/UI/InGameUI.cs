using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class InGameUI : MonoBehaviour
{
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
        KeyPress();
    }

    void UpdateHealth()
    {
        int healthyNPCs = 0;
        foreach (GameObject npc in _gameManager.NPCs.ToList())
        {
            if (!npc.GetComponent<NPC>().IsInfected)
            {
                healthyNPCs++;
            }
        }
        float percentage = 0;
        if (_gameManager.NPCs.Count > 0)
        {
            percentage = (float)healthyNPCs / _gameManager.NPCs.Count;
        }

        // update the health bar
        GameObject.Find("HealthSlider").GetComponent<Slider>().value = percentage;

        // update the health text
        GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>().text = "Healthy: " + (percentage * 100).ToString("0") + "%";

        // update the infected text
        GameObject.Find("InfectedText").GetComponent<TextMeshProUGUI>().text = "Infected: " + ((1 - percentage) * 100).ToString("0") + "%";
    }

    void UpdateHappiness()
    {
        float totalHappiness = 0;
        for (int i = 0; i < _gameManager.NPCs.Count; i++)
        {
            totalHappiness += _gameManager.NPCs.ElementAt(i).GetComponent<NPC>().Happiness;
        }

        float percentage = 0;
        if (_gameManager.NPCs.Count > 0)
        {
            percentage = totalHappiness / (_gameManager.NPCs.Count * 100);
        }

        // update the happiness bar
        GameObject.Find("HappinessSlider").GetComponent<UnityEngine.UI.Slider>().value = percentage;

        // update the happiness text
        GameObject.Find("HappyText").GetComponent<TextMeshProUGUI>().text = "Happy: " + (percentage * 100).ToString("0") + "%";

        // update the unhappy text
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
        int day = (int)(Time.time / 60 / 60 / 24);
        GameObject.Find("DayCount").GetComponent<TextMeshProUGUI>().text = "Day " + day.ToString();
    }

    public void HandleUIpanel(GameObject panel)
    {
        if (panel.activeSelf)
            panel.SetActive(false);
        else
            panel.SetActive(true);
    }

    public void PauseGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
            UpdateTimeCounter(true);
        }
    }

    public void FastForward()
    {
        switch (Time.timeScale)
        {
            case 0:
                Time.timeScale = 1;
                break;
            case 1:
                Time.timeScale = 2;
                break;
            case 2:
                Time.timeScale = 4;
                break;
            case 4:
                Time.timeScale = 1;
                break;
            default:
                Time.timeScale = 1;
                break;
        }
    }

    void KeyPress()
    {
        switch (Input.inputString)
        {
            case "1":
                Time.timeScale = 1;
                break;
            case "2":
                Time.timeScale = 2;
                break;
            case "3":
                Time.timeScale = 4;
                break;
            case "0":
                Time.timeScale = 0;
                break;
            default:
                break;
        }
    }

}
