using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeManager", menuName = "ScriptableObjects/TimeManager", order = 1)]
public class TimeManager : ScriptableObject
{
    private bool _isPaused = false;

    [SerializeField]
    [Tooltip("In game hour")]
    private int _inGameHour = 0;

    [SerializeField]
    [Tooltip("In game minutes")]
    private int _inGameMinute = 0;

    [SerializeField]
    [Tooltip("In game days")]
    private int _inGameDay = 0;

    [SerializeField]
    [Tooltip("Delta Time")]
    private float _elapsedTime = 0f;

    [SerializeField]
    [Tooltip("Time scale")]
    private float _timeScale = 1f;

    private void Awake()
    {
        _timeScale = Time.timeScale;
    }

    public bool IsPaused { get => _isPaused; set => _isPaused = value; }
    public int InGameHour { get => _inGameHour; set => _inGameHour = value; }
    public int InGameMinute { get => _inGameMinute; set => _inGameMinute = value; }
    public int InGameDay { get => _inGameDay; set => _inGameDay = value; }
    public float ElapsedTime { get => _elapsedTime; set => _elapsedTime = value; }
    public float TimeScale { get => _timeScale; set => _timeScale = value; }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
        _timeScale = Time.timeScale;
    }
    public void OnKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1f;
            _timeScale = Time.timeScale;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2f;
            _timeScale = Time.timeScale;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 3f;
            _timeScale = Time.timeScale;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isPaused = Time.timeScale != 0;
            Time.timeScale = _isPaused ? 0 : 1;
            _timeScale = Time.timeScale;
        }
    }

    public void Clock()
    {
        if (_elapsedTime >= 1f)
        {
            _elapsedTime = 0f;
            _inGameMinute++;
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
            return;
        }
        _elapsedTime += Time.deltaTime;
    }
}
