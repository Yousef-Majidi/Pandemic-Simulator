using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Cough Rate Effect", menuName = "ScriptableObjects/Cough Rate Effect", order = 2)]
public class CoughRateEffect : DecisionEffect
{
    [SerializeField]
    [Tooltip("The modifier applied to the virus cough rate: 0 - 1")]
    private float _coughRateModifier;
    public override void ApplyEffect(List<GameObject> npcs)
    {
        foreach (GameObject obj in npcs.ToList())
        {
            if (obj == null)
                npcs.Remove(obj);
            else if (obj.TryGetComponent(out NPC npc) && npc.IsInfected)
            {
                npc.Virus.CoughRate *= _coughRateModifier;
            }
        }
    }

    public override void RemoveEffect(List<GameObject> npcs)
    {
        foreach (GameObject obj in npcs.ToList())
        {
            if (obj == null)
                npcs.Remove(obj);
            else if (obj.TryGetComponent(out NPC npc) && npc.IsInfected)
            {
                npc.Virus.CoughRate /= _coughRateModifier;
            }
        }
    }
}
