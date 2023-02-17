using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Commercial : Building
{
    protected override void ReleaseNPC(GameObject npc)
    {
        int randomIndex;
        npc.SetActive(true);
        _visiting.Remove(npc);
        if (npc.GetComponent<NPC>().Health <= _gameManager.HealthThreshold)
        {
            randomIndex = Random.Range(0, _gameManager.MedicalDestinations.Count);
            npc.GetComponent<Navigation>().Destination = _gameManager.MedicalDestinations.ElementAt(randomIndex).transform;
            return;
        }
        npc.GetComponent<Navigation>().UpdateDestination(npc.GetComponent<Navigation>().Home);
        return;
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
