using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSticker : MonoBehaviour
{
    [SerializeField] private float maxDistance = 1.5f;


    void Update()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, maxDistance))
        {
            transform.position = hitInfo.point;
        }
    }
}
