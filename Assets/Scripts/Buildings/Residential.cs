using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Residential : Building
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
        randomIndex = Random.Range(0, _gameManager.CommercialDestinations.Count);
        npc.GetComponent<Navigation>().UpdateDestination(_gameManager.CommercialDestinations.ElementAt(randomIndex).transform);
        return;
    }

    private void Start()
    {
        Awake();
        SetSpawnPoint(_gameManager.ResidentialDestinations);
    }

    private void Update()
    {
        DetectNPC();
        RecoverStamina();
        CalculateHealth();
        ElapsedTime();
    }
}
