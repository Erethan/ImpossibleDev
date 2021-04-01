using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHit : MonoBehaviour
{
    [SerializeField] private HitType _hitType;

    [SerializeField] private float _secondsToLive;
    [SerializeField] private float _damage;

    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _secondsToLive);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IHittable hitObject = other.GetComponent<IHittable>();

        if (hitObject == null)
            return;

        Hit hit = new Hit
        {
            Type = _hitType,
            Damage = _damage,
            SourceGameObject = gameObject,
            HitGameObject = other.gameObject
        };
        hitObject.Hit(hit);
    }


}
