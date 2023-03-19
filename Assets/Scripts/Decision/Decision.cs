using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "Decision", menuName = "ScriptableObjects/Decision", order = 1)]
public class Decision : ScriptableObject
{
    [SerializeField]
    [Tooltip("The description of a decision")]
    private string _description = "This is a decision";

    [SerializeField]
    [Tooltip("The title of a decisions")]
    private string _title = "Decision";

    [SerializeField]
    [Tooltip("Maximum cost of the decision")]
    private int _maxCost = 0;

    [SerializeField]
    [Tooltip("The list of effects that apply to this decision")]
    private List<DecisionEffect> _effects = new();

    [SerializeField]
    [Tooltip("The list of NPCs following this decision")]
    private List<GameObject> _npcs = new();

    private bool _enacted = false;

    public delegate void DecisionEventHandler(Decision decision, float normalizedPoliticalPower = 0);
    public event DecisionEventHandler OnDecisionEnact;
    public event DecisionEventHandler OnDecisionRevoke;

    public string Description { get => _description; set => _description = value; }
    public string Title { get => _title; set => _title = value; }
    public int MaxCost { get => _maxCost; set => _maxCost = value; }
    public bool IsEnacted { get => _enacted; set => _enacted = value; }
    public List<DecisionEffect> Effects { get => _effects; set => _effects = value; }

    private void PopulateList(LinkedList<GameObject> npcs, float normalizedPoliticalPower)
    {
        int toSelect = (int)((npcs.Count) * (Math.Ceiling(normalizedPoliticalPower * 10) / 10));
        System.Random rand = new();

        GameObject[] reservoir = new GameObject[toSelect];
        int processed = 0;

        LinkedListNode<GameObject> npcNode = npcs.First;
        for (int i = 0; i < toSelect; i++)
        {
            reservoir[i] = npcNode.Value;
            npcNode = npcNode.Next;
            processed++;
        }

        while (npcNode != null)
        {
            float probability = (float)toSelect / (processed + 1);
            if (rand.NextDouble() < probability)
            {
                int toReplace = rand.Next(toSelect);
                reservoir[toReplace] = npcNode.Value;
            }

            npcNode = npcNode.Next;
            processed++;
        }

        _npcs = new List<GameObject>(reservoir);
    }
    public void NotifyDecisionStatusChange(Decision decision, float normalizedPoliticalPower)
    {
        if (decision != null)
        {
            if (!decision.IsEnacted)
            {
                OnDecisionEnact(decision, normalizedPoliticalPower);
                return;
            }
            OnDecisionRevoke(decision);
        }
    }

    public void ApplyEffects(LinkedList<GameObject> npcs, float normalizedPoliticalPower)
    {
        PopulateList(npcs, normalizedPoliticalPower);
        foreach (DecisionEffect effect in _effects)
        {
            effect.ApplyEffect(_npcs);
        }
    }

    public void RemoveEffects()
    {
        if (_npcs.Count == 0) return;

        foreach (DecisionEffect effect in _effects)
        {
            effect.RemoveEffect(_npcs);
        }
        _npcs.Clear();
    }
}
