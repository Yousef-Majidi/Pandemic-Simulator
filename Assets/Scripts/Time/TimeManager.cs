using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeManager", menuName = "ScriptableObjects/TimeManager", order = 1)]
public class TimeManager : ScriptableObject
{
    private bool _isSpacePressed = false;
    private float _inGameHour = 0;
    private float _inGameMinute = 0;
    private float _inGameDay = 0;

    public float InGameHour { get => _inGameHour; }
    public float InGameMinute { get => _inGameMinute; }
    public float InGameDay { get => _inGameDay; }


    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void OnKeyDown()
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isSpacePressed)
            {
                Time.timeScale = 1;
                _isSpacePressed = false;
            }
            else
            {
                Time.timeScale = 0;
                _isSpacePressed = true;
            }
        }
    }

    public void Clock ()
    {
        // 24 in game hours = equal to 15 minutes in real life
        // 60 in game minutes = equal to 1 minute in real life
        _inGameMinute += Time.deltaTime * 60 / 15;
        if (_inGameMinute >= 60)
        {
            _inGameMinute = 0;
            _inGameHour++;
        }
        if (_inGameHour >= 24)
        {
            _inGameHour = 0;
            _inGameDay++;
        }

    }

}
