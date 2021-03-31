using System;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox2D : MonoBehaviour
{
    public Hit Hit { get; set; }

    public event Action<IHittable> HitEvent;

    private Collider2D[] _colliders;

    void Awake()
    {
        _colliders = GetComponents<Collider2D>();
        EnableTriggering(false);
    }

    void OnEnable() => EnableTriggering(true);
    void OnDisable() => EnableTriggering(false);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Hit == null)
            return;
        IHittable hittable = other.GetComponent<IHittable>();

        if(hittable == null)
            return;

        HitEvent?.Invoke(hittable);
        hittable.Hit(Hit);
    }

    private void EnableTriggering(bool enabled)
    {
        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = enabled;
        }
    }

   
}
