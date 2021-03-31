using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRotation : MonoBehaviour
{

    [Tooltip("Checks wether the player is grounded and can rotate.")]
    [SerializeField] protected Grounding _grounding = default;


    [Tooltip("Rotation speed if the rotation size is less than 90 degrees.")]
    [SerializeField] protected float _rotationSpeed = 1;

    [Tooltip("Time in seconds that this scripts becomes idle (stops rotating) after receiving no input.")]
    [SerializeField] protected float _secondsToIdle = 0.5f;
    
    [Tooltip("Rotates towards movement input when no rotation input is provided.")]
    [SerializeField] protected GroundMovement _movement = default;

    public bool IgnoreRotationInputWhenIdle { get; set; } = true;
    public bool IsIdle => Time.time > _lastInputTime + _secondsToIdle;
    protected float _lastInputTime;


    public enum InputMode { Directional, ScreenPosition}
    public InputMode Mode { get; set; }

    protected Quaternion _rotationTarget = Quaternion.identity;
    protected Vector2 _rotationInput = new Vector3();
    public Vector2 RotationInput
    {
        get 
        {
            float inputAngle;
            switch (Mode)
            {
                case InputMode.Directional:
                    inputAngle = 90 - (Mathf.Atan2(_rotationInput.y, _rotationInput.x) * Mathf.Rad2Deg);
                    if (_rotationInput.sqrMagnitude > 1)
                    {
                        _rotationInput.Normalize();
                    }
                    break;
                case InputMode.ScreenPosition:
                    Vector3 avatarScreenPosition = Camera.main.WorldToScreenPoint(_grounding.Rigidbody.position);
                    avatarScreenPosition.Set(_rotationInput.x - avatarScreenPosition.x, _rotationInput.y - avatarScreenPosition.y, 0);
                    inputAngle = 90 - (Mathf.Atan2(avatarScreenPosition.y, avatarScreenPosition.x) * Mathf.Rad2Deg);
                    break;
                default:
                    return Vector3.zero;
            }

           
            _rotationTarget = AngleToRotation(inputAngle);
            return _rotationInput;
        }
        set
        {
            _rotationInput = value;
            _lastInputTime = Time.time;
        }
    }

    private Quaternion RotationStep
    {
        get
        {

            float rotationAngle = Quaternion.Angle(_grounding.Rigidbody.rotation, _rotationTarget);
            return rotationAngle >= 90
                ? _rotationTarget
                : Quaternion.RotateTowards(_grounding.Rigidbody.rotation, _rotationTarget, rotationAngle * _rotationSpeed);
        }
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {

        if (_grounding.Grounded)
        {
            bool hasRotationInput = RotationInput.sqrMagnitude > float.Epsilon;
            bool hasMovementInput = _movement.MovementInput.sqrMagnitude > float.Epsilon;
            
            if (IgnoreRotationInputWhenIdle 
                && IsIdle)
            {
                if (_grounding.Rigidbody.velocity.sqrMagnitude > float.Epsilon && !hasRotationInput )
                {
                    float movementAngle = Mathf.Atan2(_movement.MovementInput.y, _movement.MovementInput.x) * Mathf.Rad2Deg;
                    _rotationTarget = AngleToRotation(90 - movementAngle);
                }
            }


            if ((!IsIdle && hasRotationInput) 
                || (IsIdle && hasMovementInput))
            {
                _grounding.Rigidbody.MoveRotation(RotationStep);
            }

        }
    }

    protected Quaternion AngleToRotation(float angle)
    {
        Vector3 referenceUp = Vector3.ProjectOnPlane(_grounding.DirectionReference.up, Vector3.up);
        angle += Vector3.SignedAngle(Vector3.forward, referenceUp, Vector3.up);

        //TODO: Account for grounding.NormalAxis
        return Quaternion.AngleAxis(angle, Vector3.up); 
    }


}
