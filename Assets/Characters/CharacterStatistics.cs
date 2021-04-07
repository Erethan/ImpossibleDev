using UnityEngine;

[CreateAssetMenu(fileName = "new Character Stats", menuName ="Erethan/Character/Statistics")]
public class CharacterStatistics : ScriptableObject
{
    [SerializeField] private float _health;
    [SerializeField] private float _healthRegen;
    [SerializeField] private float _healthRegenDelay;

    [SerializeField] private float _mana;
    [SerializeField] private float _manaRegen;
    [SerializeField] private float _manaRegenDelay;

    [SerializeField] private float _stamina;
    [SerializeField] private float _staminaRegen;
    [SerializeField] private float _staminaRegenDelay;

    public float Health => _health;
    public float HealthRegen => _healthRegen;
    public float HealthRegenDelay => _healthRegenDelay;

    public float Mana => _mana;
    public float ManaRegen => _manaRegen;
    public float ManaRegenDelay => _manaRegenDelay;

    public float Stamina => _stamina;
    public float StaminaRegen => _staminaRegen;
    public float StaminaRegenDelay => _staminaRegenDelay;



}
