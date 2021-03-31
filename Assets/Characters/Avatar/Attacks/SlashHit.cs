using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHit : MonoBehaviour
{
    [SerializeField] private HitType _hitType;

    [SerializeField] private float _secondsToLive;
    [SerializeField] private float _damage;

    private Hit _hit;
    
    // Start is called before the first frame update
    void Start()
    {
        _hit = new Hit 
        { 
            Type = _hitType, 
            Damage = _damage 
        };

        Destroy(gameObject, _secondsToLive);
    }

    private void OnTriggerEnter(Collider other)
    {
        IHittable hitObject = other.GetComponent<IHittable>();

        if (hitObject == null)
            return;

        hitObject.Hit(_hit);
    }


}
