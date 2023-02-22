using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Decisions", menuName = "ScriptableObjects/Decisions", order = 1)]
public class Decision : ScriptableObject
{
    [SerializeField]
    [Tooltip("The description of a decision")]
    string _description = "This is a decision";

    [SerializeField]
    [Tooltip("The title of a decisions")]
    string _title = "Decision";

    [SerializeField]
    [Tooltip("The health effect multiplier")]
    double _healthEffect = 0;

    [SerializeField]
    [Tooltip("The virus effect multiplier")]
    double _virusEffect = 0;

    [SerializeField]
    [Tooltip("The happiness effect multiplier")]
    double _happyEffect = 0;

    [SerializeField]
    [Tooltip("Is the decision active?")]
    bool _enacted = false;

    public string Description { get => _description; set => _description = value; }
    public string Title { get => _title; set => _title = value; }
    public double HealthEffect { get => _healthEffect; set => _healthEffect = value; }
    public double VirusEffect { get => _virusEffect; set => _virusEffect = value; }
    public double HappyEffect { get => _happyEffect; set => _happyEffect = value; }
    public bool IsEnacted { get => _enacted; set => _enacted = value; }
}
