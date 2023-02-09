using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    private GameManager _gameManager;


    void Awake()
    {
        GameObject.Find("UIMenuPanel").SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        UpdateHappiness();
        UpdateTimeCounter();
        UpdateDayCounter();
    }

    void UpdateHealth()
    {
        // calculate health bar by getting the percentage of healthy NPCs to total NPCs
        // get the number of healthy NPCs
        GameObject[] totalNPCs = GameObject.FindGameObjectsWithTag("NPC");
        int healthyNPCs = 0;
        for (int i = 0; i < totalNPCs.Length; i++)
        {
            if (totalNPCs[i].GetComponent<NPC>().IsInfected == false)
            {

                // increment healthyNPCs
                healthyNPCs++;
            }
        }
        float percentage = 0;
        if (totalNPCs.Length > 0){
            // get the percentage of healthy NPCs to total NPCs
            percentage = (float)healthyNPCs / (float)totalNPCs.Length;
        }

        // update the health bar
        GameObject.Find("HealthSlider").GetComponent<UnityEngine.UI.Slider>().value = percentage;

        // update the health text
        GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>().text = "Healthy: " + (percentage * 100).ToString("0") + "%";

        // update the infected text
        GameObject.Find("InfectedText").GetComponent<TextMeshProUGUI>().text = "Infected: " + ((1 - percentage) * 100).ToString("0") + "%";
    }

    void UpdateHappiness()
    {
        // calculate happiness bar by getting the percentage of happy NPCs to total NPCs
        // get the number of happy NPCs
        GameObject[] totalNPCs = GameObject.FindGameObjectsWithTag("NPC");
        float totalHappiness = 0;
        for (int i = 0; i < totalNPCs.Length; i++)
        {
            totalHappiness += totalNPCs[i].GetComponent<NPC>().Happiness;
        }
        
        float percentage = 0;
        if (totalNPCs.Length > 0){
            // get the percentage of happy NPCs to total NPCs
            percentage = totalHappiness / (totalNPCs.Length * 100);
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
        // update the time aceleration counte
        if (isPaused)
            GameObject.Find("TimeCount").GetComponent<TextMeshProUGUI>().text = "x0";
        else
            GameObject.Find("TimeCount").GetComponent<TextMeshProUGUI>().text = "x" + Time.timeScale.ToString();
    }

    void UpdateDayCounter()
    {
        // update the day counter
        GameObject.Find("Clock").GetComponent<TextMeshProUGUI>().text = _gameManager.TimeManager.InGameHour.ToString("00") + ":" + _gameManager.TimeManager.InGameMinute.ToString("00");
        GameObject.Find("DayCount").GetComponent<TextMeshProUGUI>().text = "Day " + _gameManager.TimeManager.InGameDay.ToString();
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
        // if the game is paused, unpause it
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
                _gameManager.TimeManager.SetTimeScale(4);
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
