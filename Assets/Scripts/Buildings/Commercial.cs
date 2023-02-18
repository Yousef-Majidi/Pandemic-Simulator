using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;


public class Commercial : Building
{
    [SerializeField]
    [Tooltip("The transmission timer")]
    private float _elapsedTime = 0;

    [SerializeField]
    [Tooltip("The threshold at which NPCs will leave the building")]
    private float _leaveThreshold = 50f;

    protected override bool UpdateStamina(NPC npc)
    {
        if (!_gameManager.GodMode)
        {
            float recoverRate = npc.IsInfected ? (npc.Virus.StaminaDecayRate + npc.StaminaDecayBase) : npc.StaminaDecayBase;
            npc.Stamina -= recoverRate * Time.deltaTime;
            if (npc.Stamina <= _gameManager.StaminaThreshold)
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
            if (npc.IsInfected)
            {
                npc.Happiness -= npc.HappinessDecayBase * Time.deltaTime;
                if (npc.Happiness < 0)
                {
                    npc.Happiness = 0;
                    return true;
                }
                return false;
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
            comp.UpdateDestination(_gameManager.MedicalDestinations.ElementAt(randomIndex).transform, BuildingType.Medical);
            return;
        }

        if (npc.GetComponent<NPC>().Stamina <= _leaveThreshold)
        {
            comp.UpdateDestination(comp.Home, BuildingType.Residential);
            return;
        }

        randomIndex = Random.Range(0, _gameManager.CommercialDestinations.Count);
        comp.UpdateDestination(_gameManager.CommercialDestinations.ElementAt(randomIndex).transform, BuildingType.Commercial);
        return;
    }
    private void ElapsedTime()
    {
        if (_elapsedTime >= 1f)
        {
            TransmitVirus();
            _elapsedTime = 0f;
        }
        else
        {
            _elapsedTime += Time.deltaTime;
        }
    }
    private void TransmitVirus()
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

    private void Start()
    {
        Awake();
        SetSpawnPoint(_gameManager.CommercialDestinations);
    }

    private void Update()
    {
        DetectNPC();
        ElapsedTime();
        CalculateAttributes();
    }
}
