using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Commercial : Building
{
    protected override void ReleaseNPC(GameObject npc)
    {
        throw new System.NotImplementedException();
    }

    private void TransmitVirus()
    {
        if (_elapsedTime >= 1)
        {
            foreach (GameObject obj in _visiting.ToList())
            {
                NPC npc = obj.GetComponent<NPC>();
                if (npc.IsInfected)
                {
                    foreach (GameObject obj2 in _visiting.ToList())
                    {
                        NPC otherNPC = obj2.GetComponent<NPC>();
                        if (!otherNPC.IsInfected)
                        {
                            npc.Virus.TransmitVirus(otherNPC);
                        }
                    }
                }
            }
        }
    }

    private void Start()
    {
        Awake();
        SetSpawnPoint(_gameManager.CommercialDestinations);
    }
    private void Update()
    {
        DetectNPC();
        RecoverStamina();
        CalculateHealth();
        ElapsedTime();
        TransmitVirus();
    }
}
