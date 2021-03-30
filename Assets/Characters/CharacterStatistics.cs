using UnityEngine;

[CreateAssetMenu(fileName = "new Character Stats", menuName ="Erethan/Character/Statistics")]
public class CharacterStatistics : ScriptableObject
{
    private float _health;
    private float _mana;
    private float _stamina;

    public float Health => _health;
    public float Mana => _mana;
    public float Stamina => _stamina;



}
