using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Residential : Building
{
    override protected void ReleaseNPC(GameObject npc)
    {
        int randomIndex = Random.Range(0, _gameManager.CommercialDestinations.Count);
        npc.GetComponent<Navigation>().UpdateDestination(_gameManager.CommercialDestinations.ElementAt(randomIndex).transform);
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
        Debug.Log("Visiting: " + _visiting.Count);
    }
}
