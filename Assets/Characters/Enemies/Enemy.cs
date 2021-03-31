using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character2D
{
    public override void Hit(Hit hit)
    {
        base.Hit(hit);
        Debug.Log($"I'm hit! Current health: {CurrentHealth}");
    }


    protected override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
