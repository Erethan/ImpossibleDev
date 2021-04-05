using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarBlocker : MonoBehaviour, IHitBlocker
{
    public bool IsBlocking { get; private set; }
    
    [Range(0,180)][SerializeField] private float _blockArc = 10;

    public void ActivateBlocking()
    {
        IsBlocking = true;
    }

    public void DeactivateBlocking()
    {
        IsBlocking = false;
    }

    public bool TryBlock(Hit hit, Vector3 center)
    {
        if (!IsBlocking)
            return false;
        // Debug.Log($"TryBlock {Vector3.Angle(hit.Direction, transform.position - center)} --- {hit.HitGameObject}");

        float blockAngle = 180 - Vector3.Angle(hit.Direction, transform.position - center);
        Debug.Log($"Block Arc {_blockArc} - Block Angle  {blockAngle} - Blocked {blockAngle <= _blockArc}");
        return blockAngle <= _blockArc;
    }
}
