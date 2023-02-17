using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Virus", menuName = "ScriptableObjects/Virus", order = 1)]
public class Virus : ScriptableObject
{
    [SerializeField]
    [Tooltip("The chance of spreading infection by being near another NPC")]
    private float _coughRate = 0.5f;

    [SerializeField]
    [Tooltip("The rate at which the NPC's stamina decays")]
    private float _staminaDecayRate = 0.5f;

    [SerializeField]
    [Tooltip("The rate at which the NPC's health decays")]
    private float _healthDecayRate = 0.5f;

    [SerializeField]
    [Tooltip("The chance of mutation when infecting another NPC")]
    private float _mutationChance = 0.5f;

    public float CoughRate { get => _coughRate; set => _coughRate = value; }
    public float StaminaDecayRate { get => _staminaDecayRate; set => _staminaDecayRate = value; }
    public float HealthDecayRate { get => _healthDecayRate; set => _healthDecayRate = value; }
    public float MutationChance { get => _mutationChance; set => _mutationChance = value; }

    public void Mutate()
    {
        _coughRate = Random.Range(0.01f, 0.1f);
        _staminaDecayRate = Random.Range(1f, 5f);
        _healthDecayRate = Random.Range(1f, 5f);
        _mutationChance = Random.Range(0.01f, 0.1f);
    }

    public void Copy(Virus source)
    {
        _coughRate = source._coughRate;
        _staminaDecayRate = source._staminaDecayRate;
        _healthDecayRate = source._healthDecayRate;
        _mutationChance = source._mutationChance;
    }

    public void TransmitVirus(NPC other)
    {
        float random = Random.Range(0f, 1f);
        if (random < _coughRate)
        {
            other.IsInfected = true;
        }

        if (other.IsInfected)
        {
            other.Virus = CreateInstance<Virus>();
            random = Random.Range(0f, 1f);
            if (random < _mutationChance)
            {
                other.Virus.Mutate();
            }
            else
            {
                other.Virus.Copy(this);
            }
        }
    }
}
