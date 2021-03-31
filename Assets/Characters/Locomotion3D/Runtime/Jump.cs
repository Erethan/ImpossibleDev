using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{

#pragma warning disable CS0649
    [SerializeField] protected new Rigidbody rigidbody; //movement is relative to the reference Transform;
    [SerializeField] protected Grounding Grounding;

    [SerializeField] protected float impulseForce = 4f;
    [SerializeField] protected float floatTimeAfterJumnp = 0.1f;

#pragma warning restore CS0649


    public void TryJump()
    {
        if (!Grounding.Grounded)
            return;

        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        rigidbody.AddForce(Vector3.up * impulseForce,ForceMode.Impulse);
        if(floatTimeAfterJumnp > 0)
        {
            Grounding.ForceFloating(floatTimeAfterJumnp);
        }

    }

}
