using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAttacks : MonoBehaviour
{
    [SerializeField] private GameObject _attack1Prefab;
    [SerializeField] private GameObject _attack2Prefab;
    [SerializeField] private GameObject _attack3Prefab;

    [SerializeField] private Transform _weaponTransform;


    private GameObject _currentSlash;

    private void OnAttack1()
    {
        _currentSlash = Instantiate(_attack1Prefab, _weaponTransform);
        _currentSlash.transform.SetParent(null);
    }

    private void OnAttack2()
    {
        _currentSlash = Instantiate(_attack2Prefab, _weaponTransform);
        _currentSlash.transform.SetParent(null);
    }

    private void OnAttack3()
    {
        _currentSlash = Instantiate(_attack3Prefab, _weaponTransform);
        _currentSlash.transform.SetParent(null);
    }
}
