using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{

    void Awake()
    {
        GameObject.Find("UIMenuPanel").SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
        UpdateHappinessBar();
        UpdateTimeCounter();
        UpdateDayCounter();
        keyPress();
        
    }

    void UpdateHealthBar()
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
        // get the percentage of healthy NPCs to total NPCs
        float percentage = (float)healthyNPCs / (float)totalNPCs.Length;
        // update the health bar
        GameObject.Find("HealthSlider").GetComponent<UnityEngine.UI.Slider>().value = percentage;
    }

    void UpdateHappinessBar()
    {
        // calculate happiness bar by getting the percentage of happy NPCs to total NPCs
        // get the number of happy NPCs
        GameObject[] totalNPCs = GameObject.FindGameObjectsWithTag("NPC");
        float totalHappiness = 0;
        for (int i = 0; i < totalNPCs.Length; i++)
        {
            totalHappiness += totalNPCs[i].GetComponent<NPC>().Happiness;
        }
        // get the percentage of happy NPCs to total NPCs
        float percentage = totalHappiness / (totalNPCs.Length * 100);
        // update the happiness bar
        GameObject.Find("HappinessSlider").GetComponent<UnityEngine.UI.Slider>().value = percentage;
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
        // if the game is paused, unpause it
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

    void keyPress()
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
