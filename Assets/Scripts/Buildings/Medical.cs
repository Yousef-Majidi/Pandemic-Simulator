using System.Collections;
using System.Linq;
using UnityEngine;


public class Medical : Building
{

    [SerializeField]
    [Tooltip("The rate at which the NPC's stamina recovers")]
    private float _staminaRecoveryRate = 5f;

    [SerializeField]
    [Tooltip("The rate at which the NPC's health recovers")]
    private float _healthRecoveryRate = 5f;

    protected override bool UpdateStamina(NPC npc)
    {
        if (!_gameManager.GodMode)
        {
            npc.Stamina += _staminaRecoveryRate * Time.deltaTime;
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
            npc.IsInfected = false;
            npc.Health += _healthRecoveryRate * Time.deltaTime;
            if (npc.Health > 100f)
            {
                npc.Health = 100f;
                return true;
            }
        }
        return false;
    }

    protected override bool UpdateHappiness(NPC npc)
    {
        if (!_gameManager.GodMode)
        {
            npc.Happiness += npc.HappinessDecayBase * Time.deltaTime;
            if (npc.Happiness > 100f)
            {
                npc.Happiness = 100f;
                return false;
            }
            return false;
        }
        return false;
    }

    protected override void ReleaseNPC(GameObject npc)
    {
        if (!_gameManager.GodMode)
        {
            if (npc.GetComponent<NPC>().Health == 100f && npc.GetComponent<NPC>().Stamina == 100f)
            {
                int randomIndex;
                npc.SetActive(true);
                _visiting.Remove(npc);

                Navigation comp = npc.GetComponent<Navigation>();
                randomIndex = Random.Range(0, _gameManager.CommercialDestinations.Count);
                comp.UpdateDestination(_gameManager.CommercialDestinations.ElementAt(randomIndex).transform, BuildingType.Commercial);
            }
        }
        return;
    }

    void Start()
    {
        Awake();
        SetSpawnPoint(_gameManager.MedicalDestinations);
    }

    private void Update()
    {
        DetectNPC();
        CalculateAttributes();
    }
}
