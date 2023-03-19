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
            float recoveryRate = npc.IsInfected ? _staminaRecoveryMultiplier * _staminaRecoveryRate : _staminaRecoveryRate;
            npc.Stamina += recoveryRate * Time.deltaTime;

            if (npc.Stamina > 100f)
            {
                npc.Stamina = 100f;
                return true;
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
                npc.Happiness -= npc.HappinessDecayBase * Time.deltaTime;
                if (npc.Happiness < 0)
                {
                    npc.Happiness = 0;
                }
                return false;
            }
            npc.Happiness += _happinessRecoveryRate * Time.deltaTime;
            if (npc.Happiness > 100)
            {
                npc.Happiness = 100;
            }
            return false;
        }
        return false;
    }

    protected override void ReleaseNPC(GameObject npc)
    {
        npc.SetActive(true);
        _visiting.Remove(npc);
        Navigation nav = npc.GetComponent<Navigation>();
        nav.IsTravelling = false;
        return;
    }

    private new void Awake()
    {
        base.Awake();
        _capacity = Mathf.CeilToInt((float)_gameManager.MaxNPCs / _gameManager.ResidentialDestinations.Count);
        SetSpawnPoint(_gameManager.ResidentialDestinations);
    }

    private void Update()
    {
        CalculateAttributes();
    }
}
