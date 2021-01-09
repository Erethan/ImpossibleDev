using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Transform Target{ get { return target; } set { target = value; } }

    void Update()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
        transform.localScale = target.localScale;
    }

}
