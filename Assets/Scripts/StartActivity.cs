using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartActivity : MonoBehaviour
{
    [SerializeField] private UnityEvent StartEvent;
    private void Start()
    {
        StartEvent.Invoke();
    }
}
