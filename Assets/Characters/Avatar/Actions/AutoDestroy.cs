using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float _secondsToLive = 1;
    
    void Start()
    {
        Destroy(gameObject, _secondsToLive);
    }

}
