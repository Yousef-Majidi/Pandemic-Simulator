using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Residential : Building
{
    [SerializeField]
    [Tooltip("The rate at which the NPC's stamin recovers")]
    protected float _staminaRecoveryRate = 5f;
    protected override void ReleaseNPC(GameObject npc)
    {
        int randomIndex;
        npc.SetActive(true);
        _visiting.Remove(npc);
        Navigation comp = npc.GetComponent<Navigation>();
        if (npc.GetComponent<NPC>().Health <= _gameManager.HealthThreshold)
        {
            randomIndex = Random.Range(0, _gameManager.MedicalDestinations.Count);
            comp.UpdateDestination(_gameManager.MedicalDestinations.ElementAt(randomIndex).transform);
            //comp.Destination = _gameManager.MedicalDestinations.ElementAt(randomIndex).transform;
            return;
        }
        randomIndex = Random.Range(0, _gameManager.CommercialDestinations.Count);
        comp.UpdateDestination(_gameManager.CommercialDestinations.ElementAt(randomIndex).transform);
        return;
    }

    private void RecoverStamina()
    {
        foreach (GameObject obj in _visiting.ToList())
        {
            NPC npc = obj.GetComponent<NPC>();
            if (npc.Stamina < 100f)
            {
                npc.Stamina += _staminaRecoveryRate * Time.deltaTime;
            }
            if (npc.Stamina > 100f)
            {
                npc.Stamina = 100f;
                ReleaseNPC(obj);
            }
        }
    }

    private void RecoverHappiness()
    {
        foreach (GameObject obj in _visiting.ToList())
        {
            NPC npc = obj.GetComponent<NPC>();
            if (npc.Happiness < 100f)
            {
                npc.Happiness += npc.HappinessRecoveryRate * Time.deltaTime;
            }
            if (npc.Happiness > 100f)
            {
                npc.Happiness = 100f;
            }
        }
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
        RecoverHappiness();
        CalculateHealth();
    }
}
