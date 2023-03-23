using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Happiness Effect", menuName = "ScriptableObjects/Happiness Effect", order = 2)]
public class HappinessEffect : DecisionEffect
{
    [SerializeField]
    [Tooltip("Happiness decay rate")]
    private float _happinessDecayRate;
    public override void ApplyEffect(List<GameObject> npcs)
    {
        foreach (GameObject obj in npcs.ToList())
        {
            if (obj == null)
                npcs.Remove(obj);
            else
            {
                NPC npc = obj.GetComponent<NPC>();
                npc.HappinessDecayRate += _happinessDecayRate;
            }
        }
    }

    public override void RemoveEffect(List<GameObject> npcs)
    {
        foreach (GameObject obj in npcs.ToList())
        {
            if (obj == null)
                npcs.Remove(obj);
            else
            {
                NPC npc = obj.GetComponent<NPC>();
                npc.HappinessDecayRate -= _happinessDecayRate;
            }
        }
    }
}
