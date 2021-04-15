using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAttacks : MonoBehaviour
{
    [SerializeField] private Character2D _character;

    [SerializeField] private CharacterAction _firstAttack;
    [SerializeField] private CharacterAction _secondAttack;
    [SerializeField] private CharacterAction _thirdAttack;

    [SerializeField] private string _attackAnimationParameter;

    [SerializeField] private Transform _weaponTransform;

    private GameObject _currentSlash;

    void Start()
    {
        InputWindowStateMachineBehaviour[] behaviours =_character.Animator.GetBehaviours<InputWindowStateMachineBehaviour>();

        foreach (var behaviour in behaviours)
        {

            if(behaviour.BindingTag.Equals(_firstAttack.BindingTag))
            {
                behaviour.ActionPerformed += TrySecondAttack;
                continue;
            }

            if (behaviour.BindingTag.Equals(_secondAttack.BindingTag))
            {
                behaviour.ActionPerformed += TryThirdAttack;
            }
        }
    }

    public void StartAttackChain()
    {
        _character.SpendStamina(_firstAttack.StaminaCost);

    }

    private void TrySecondAttack()
    {
        if (_character.CurrentStamina <= 0)
            return;

        _character.SpendStamina(_secondAttack.StaminaCost);
        _character.Animator.SetInteger(_attackAnimationParameter, 1);
    }


    private void TryThirdAttack()
    {
        if (_character.CurrentStamina <= 0)
            return;

        _character.SpendStamina(_thirdAttack.StaminaCost);
        _character.Animator.SetInteger(_attackAnimationParameter, 2);
    }


    private void OnAttack1()
    {

        _currentSlash = Instantiate(_firstAttack.Prefab, _weaponTransform);
        _currentSlash.transform.SetParent(null);
    }

    private void OnAttack2()
    {
       

        _currentSlash = Instantiate(_secondAttack.Prefab, _weaponTransform);
        _currentSlash.transform.SetParent(null);
    }

    private void OnAttack3()
    {
        
        _currentSlash = Instantiate(_thirdAttack.Prefab, _weaponTransform);
        _currentSlash.transform.SetParent(null);
    }
}
