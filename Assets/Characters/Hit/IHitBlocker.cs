using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IHitBlocker 
{
    bool IsBlocking { get; }

    void ActivateBlocking();
    void DeactivateBlocking();

    bool TryBlock(Hit hit, Vector3 center);

}
