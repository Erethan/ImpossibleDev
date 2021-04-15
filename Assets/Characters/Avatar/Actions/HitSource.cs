using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSource : MonoBehaviour
{
    [SerializeField] private HitType _hitType;

    [SerializeField] private float _damage;

    List<IHittable> hitObjects = new List<IHittable>();

    public void OnTriggerEnter2D(Collider2D other)
    {
        IHittable hitObject = other.GetComponent<IHittable>();
        GameObject hitGameObject = other.gameObject;

        if(hitObject == null && other.attachedRigidbody != null)
        {
            hitObject = other.attachedRigidbody.GetComponent<IHittable>();
            hitGameObject = other.attachedRigidbody.gameObject;
        }

        if (hitObject == null)
            return;

        if (hitObjects.Contains(hitObject))
            return;

        Hit hit = new Hit
        {
            Type = _hitType,
            Damage = _damage,
            SourceGameObject = gameObject,
            HitGameObject = hitGameObject
        };
        hitObjects.Add(hitObject);
        hitObject.Hit(hit);
    }


}
