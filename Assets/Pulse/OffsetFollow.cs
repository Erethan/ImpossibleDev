using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private bool lookAtTarget;


    void LateUpdate()
    {
        transform.position = target.transform.position + offset;

        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }
}
