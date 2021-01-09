using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFollow : MonoBehaviour
{
    public Vector3 offset;
    public Transform target;
    public bool lookAtTarget;
    public bool changeHeight;
    

    void LateUpdate()
    {
        float height = transform.position.y;
        transform.position = target.transform.position + offset;

        if (!changeHeight)
        {
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }

        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }
}
