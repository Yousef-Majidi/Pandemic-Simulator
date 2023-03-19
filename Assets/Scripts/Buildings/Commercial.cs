using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;


public class Commercial : Building
{
    [SerializeField]
    [Tooltip("The threshold at which NPCs will leave the building")]
    private float _leaveThreshold = 50f;

    protected override bool UpdateStamina(NPC npc)
    {
        if (!_gameManager.GodMode)
        {
            float decayRate = npc.IsInfected ? (npc.Virus.StaminaDecayRate + npc.StaminaDecayBase) : npc.StaminaDecayBase;
            npc.Stamina -= decayRate * Time.deltaTime;
            if (npc.Stamina <= _leaveThreshold)
            {
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
                npc.Health -= npc.Virus.HealthDecayRate * Time.deltaTime;
                if (npc.Health <= _gameManager.HealthThreshold)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected override bool UpdateHappiness(NPC npc)
    {
        if (!_gameManager.GodMode)
        {
            npc.Happiness -= npc.HappinessDecayRate * Time.deltaTime;
            if (npc.Happiness < 0)
            {
                npc.Happiness = 0;
                return true;
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
        nav.IsCommuting = false;
        return;
    }
    private void ElapsedTime()
    {
        if (_gameManager.TimeManager.ElapsedTime >= 1f)
        {
            TransmitVirus();
        }
    }
    private void TransmitVirus()
    {
        foreach (GameObject obj in _visiting.ToList())
        {
            if (obj != null)
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
            else
            {
                _visiting.Remove(obj);
            }
        }
    }

    private new void Awake()
    {
        base.Awake();
        _capacity = Mathf.CeilToInt((float)_gameManager.MaxNPCs / _gameManager.CommercialDestinations.Count);
        SetSpawnPoint(_gameManager.CommercialDestinations);
    }

    private void Update()
    {
        _occupancy = _visiting.Count;
        ElapsedTime();
        CalculateAttributes();
    }
}
