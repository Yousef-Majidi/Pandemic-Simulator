using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Decisions", menuName = "ScriptableObjects/Decisions", order = 1)]
public class Decisions : ScriptableObject
{
    string _description = "This is a decision";
    string _title = "Decision";
    double _healthEffect = 0;
    double _virusEffect = 0;
    double _happyEffect = 0;
    bool _isActive = false;

    public string description
    {
        get { return _description; }
        set { _description = value; }
    }

    public string title
    {
        get { return _title; }
        set { _title = value; }
    }


    public double healthEffect
    {
        get { return _healthEffect; }
        set { _healthEffect = value; }
    }

    public double virusEffect
    {
        get { return _virusEffect; }
        set { _virusEffect = value; }
    }

    public double happyEffect
    {
        get { return _happyEffect; }
        set { _happyEffect = value; }
    }

    public bool isActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }

    public Decisions()
    {
        _description = "This is a decision";
        _title = "Decision";
        _healthEffect = 0;
        _virusEffect = 0;
        _happyEffect = 0;
    }

    public Decisions(string description, string title, double healthEffect, double virusEffect, double happyEffect)
    {
        _description = description;
        _title = title;
        _healthEffect = healthEffect;
        _virusEffect = virusEffect;
        _happyEffect = happyEffect;
    }

}
