using UnityEngine;

[CreateAssetMenu(fileName = "new Character Stats", menuName ="Erethan/Character/Statistics")]
public class CharacterStatistics : ScriptableObject
{
    [SerializeField] private float _health;
    [SerializeField] private float _mana;
    [SerializeField] private float _stamina;

    public float Health => _health;
    public float Mana => _mana;
    public float Stamina => _stamina;



}
