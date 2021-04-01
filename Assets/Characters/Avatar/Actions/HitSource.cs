using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSource : MonoBehaviour
{
    [SerializeField] private HitType _hitType;

    [SerializeField] private float _damage;


    private void OnTriggerEnter2D(Collider2D other)
    {
        IHittable hitObject = other.attachedRigidbody != null
            ? other.attachedRigidbody.GetComponent<IHittable>()
            : other.GetComponent<IHittable>();

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
