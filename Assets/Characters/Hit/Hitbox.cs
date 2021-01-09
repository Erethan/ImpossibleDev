using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class HitUnityEvent : UnityEvent<Hitbox,HitReceiver> { }

public class Hitbox : MonoBehaviour
{
    private Collider[] colliders;

    public event Action<Hitbox, HitReceiver> Hit;

    void Awake()
    {
        colliders = GetComponents<Collider>();
        EnableTriggering(false);
    }

    void OnEnable() => EnableTriggering(true);
    void OnDisable() => EnableTriggering(false);

    
    void OnTriggerEnter(Collider other)
    {
        HitReceiver hitReceiver = other.GetComponent<HitReceiver>();
        if(hitReceiver != null)
        {
            Hit.Invoke(this, hitReceiver);
        }
    }

    private void EnableTriggering(bool enabled)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = enabled;
        }
    }

   
}
