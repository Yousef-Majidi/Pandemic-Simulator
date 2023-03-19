using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionEffect : ScriptableObject
{
    public abstract void ApplyEffect(List<GameObject> npcs);
    public abstract void RemoveEffect(List<GameObject> npcs);
}
