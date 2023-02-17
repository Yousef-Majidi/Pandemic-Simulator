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

        if (npc.GetComponent<NPC>().Stamina <= _gameManager.StaminaThreshold)
        {
            comp.UpdateDestination(comp.Home);
            return;
        }

        randomIndex = Random.Range(0, _gameManager.CommercialDestinations.Count);
        comp.Destination = _gameManager.CommercialDestinations.ElementAt(randomIndex).transform;
        return;
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

    private void ReduceStamina()
    {
        foreach (GameObject obj in _visiting.ToList())
        {
            NPC npc = obj.GetComponent<NPC>();
            npc.UpdateStamina();
            if (npc.Stamina <= _gameManager.StaminaThreshold)
            {
                ReleaseNPC(obj);
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
        CalculateHealth();
        ReduceStamina();
        ElapsedTime();
    }
}
