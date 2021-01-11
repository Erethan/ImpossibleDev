using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRotation : MonoBehaviour
{

    [Tooltip("Ground component that checks wether the player is grounded and can rotate.")]
    [SerializeField] protected Grounding grounding = default;

    [Tooltip("Rotation speed if the rotation size is less than 90 degrees.")]
    [SerializeField] protected float _rotationSpeed = 1;

    [Tooltip("Time in seconds that this scripts becomes idle (stops rotating) after receiving no input.")]
    [SerializeField] protected float _secondsToIdle = 0.5f;

    protected float _lastInputTime;
    public bool IgnoreWhenIdle { get; set; } = true;

    protected Quaternion _rotationTarget = Quaternion.identity;
    protected Vector2 _rotationInput;
    public Vector2 RotationInput
    {
        get { return _rotationInput; }
        set
        {
            _rotationInput = value;
            if (_rotationInput.sqrMagnitude > 1)
            {
                _rotationInput.Normalize();
            }

            float inputAngle = 90 - (Mathf.Atan2(_rotationInput.y, _rotationInput.x) * Mathf.Rad2Deg);
            
            _rotationTarget = AngleToRotation(inputAngle);
            _lastInputTime = Time.time;
        }
    }

    private Quaternion RotationStep
    {
        get
        {

            float rotationAngle = Quaternion.Angle(grounding.Rigidbody.rotation, _rotationTarget);
            return rotationAngle >= 90
                ? _rotationTarget
                : Quaternion.RotateTowards(grounding.Rigidbody.rotation, _rotationTarget, rotationAngle * _rotationSpeed);
        }
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {

        if (grounding.Grounded)
        {
            if(IgnoreWhenIdle 
                && Time.time > _lastInputTime + _secondsToIdle)
            {
                if (grounding.Rigidbody.velocity.sqrMagnitude > 0.1f && _rotationInput.sqrMagnitude < float.Epsilon)
                {
                    //TODO: Get movement from Input
                    float movementAngle = Vector3.SignedAngle(Vector3.forward, grounding.Rigidbody.velocity, Vector3.up);
                    _rotationTarget = AngleToRotation(movementAngle - 90);
                }
            }

            grounding.Rigidbody.MoveRotation(RotationStep);
        }
    }

    protected Quaternion AngleToRotation(float angle)
    {
        Vector3 referenceUp = Vector3.ProjectOnPlane(grounding.DirectionReference.up, Vector3.up);
        angle += Vector3.SignedAngle(Vector3.forward, referenceUp, Vector3.up);

        //TODO: Account for grounding.NormalAxis
        return Quaternion.AngleAxis(angle, Vector3.up); 
    }


}
