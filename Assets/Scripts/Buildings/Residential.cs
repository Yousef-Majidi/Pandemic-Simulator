using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Residential : Building
{
    [SerializeField]
    [Tooltip("The rate at which the NPC's stamin recovers")]
    private float _staminaRecoveryRate = 5f;

    [SerializeField]
    [Tooltip("Base rate for happiness recovery rate")]
    private float _happinessRecoveryRate = 5f;

    [SerializeField]
    [Tooltip("The multiplier applied to stamina recovery when NPC is infected")]
    private float _staminaRecoveryMultiplier = 0.5f;

    protected override bool UpdateStamina(NPC npc)
    {
        if (!_gameManager.GodMode)
        {
            if (npc.IsInfected)
            {
                if (npc.Stamina < 100f)
                {
                    npc.Stamina += _staminaRecoveryMultiplier * npc.Virus.StaminaDecayRate * Time.deltaTime;
                    if (npc.Stamina > 100f)
                    {
                        npc.Stamina = 100f;
                        return true;
                    }
                }
            }
            else
            {
                if (npc.Stamina < 100f)
                {
                    npc.Stamina += _staminaRecoveryRate * Time.deltaTime;
                    if (npc.Stamina > 100f)
                    {
                        npc.Stamina = 100f;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    protected override bool UpdateHealth(NPC npc)
    {
        if (!_gameManager.GodMode)
        {
            if (npc.IsInfected)
            {
                if (npc.Health <= _gameManager.HealthThreshold)
                {
                    return true;
                }
                npc.Health -= npc.Virus.HealthDecayRate * Time.deltaTime;
            }
        }
        return false;
    }

    protected override bool UpdateHappiness(NPC npc)
    {
        if (!_gameManager.GodMode)
        {
            if (npc.IsInfected)
            {
                if (npc.Happiness > 0)
                {
                    npc.Happiness -= npc.HappinessDecayBase * Time.deltaTime;
                    if (npc.Happiness < 0)
                    {
                        npc.Happiness = 0;
                    }
                }
                return false;
            }
            else
            {
                if (npc.Happiness < 100)
                {
                    npc.Happiness += _happinessRecoveryRate * Time.deltaTime;
                    if (npc.Happiness > 100)
                    {
                        npc.Happiness = 100;
                    }
                }
                return true;
            }
        }
        return false;
    }

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
            return;
        }
        randomIndex = Random.Range(0, _gameManager.CommercialDestinations.Count);
        comp.UpdateDestination(_gameManager.CommercialDestinations.ElementAt(randomIndex).transform);
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
        CalculateAttributes();
    }
}
