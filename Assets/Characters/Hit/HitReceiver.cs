using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class HitReceiver : MonoBehaviour
{

#pragma warning disable CS0649
    [SerializeField] private HitBodyType bodyType;
    [SerializeField] private HitTypeUnityEvent onHit;
#pragma warning restore CS0649

    public void Hit(HitType attack)
    {
        onHit.Invoke(attack);
    }

    public void AddLisener(UnityAction<HitType> hitAction)
    {
        onHit.AddListener(hitAction);
    }

    public void RemoveLisener(UnityAction<HitType> hitAction)
    {
        onHit.RemoveListener(hitAction);
    }
    


}
