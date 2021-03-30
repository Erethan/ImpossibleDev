using System;
using UnityEngine;
using UnityEngine.Events;

public class HitReceiver : MonoBehaviour, IHittable
{
    [SerializeField] private HitBodyType bodyType;
    [SerializeField] private UnityEvent<Hit> onHit;

    public event Action<Hit> HitEvent;

    public void Hit(Hit hit)
    {
        onHit.Invoke(hit);
        HitEvent?.Invoke(hit);
    }

    
}
