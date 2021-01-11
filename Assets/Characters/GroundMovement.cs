using UnityEngine;

public class GroundMovement : MonoBehaviour
{

    [Tooltip("Movement is relative to the reference transform. This is usually a Camera or the root transform.")]
    [SerializeField] protected Transform directionReference = default;
    
    [Tooltip("Ground component that checks wether the player is grounded and can move.")]
    [SerializeField] protected Grounding grounding = default;

    [Header("Speed")]
    [Tooltip("Force applied on the grounding rigidbody in the direction of the movement input. This force is only applied if movement has not reached it's target speed, and it does not overlap the target speed.")]
    [SerializeField] protected float motorForce = 4f;

    [Tooltip("Target speed before considering speed differences in direction.")]
    [SerializeField] protected float baseSpeed = 8.0f;


    [Tooltip("Factor increase in the final velocity if Run is set to true.")]
    [SerializeField] protected float runMultiplier = 1.5f;

    [Tooltip("Factor applied on Base Speed to access maximum speed in each different input direction. This function domain is input angle and its image is the ratio of the force applied.")]
    [SerializeField]protected AnimationCurve directionSpeedFactor= new AnimationCurve(
        new Keyframe(0, 0.5f),
        new Keyframe(90, 1f),
        new Keyframe(180, 0.5f),
        new Keyframe(270, 0.5f),
        new Keyframe(360, 0.5f));

    [Tooltip("Factor applied on Base Speed do slowdown movement when climbing slopes. It receives the slope angle and it returns the raito of the force that is applied.")]
    [SerializeField] protected AnimationCurve slopeCurveModifier = new AnimationCurve(
        new Keyframe(-90.0f, 1.0f),
        new Keyframe(20.0f, 1.0f),
        new Keyframe(40.0f, 0.6f),
        new Keyframe(70.0f, 0.0f));
    
    public enum MovementPlane { XY,XZ, YZ}
    [SerializeField] private MovementPlane movementPlane;

    private Vector2 movementInput;
    public Vector2 MovementInput
    {
        get { return movementInput; }
        set
        {
            movementInput = value;
            if (movementInput.magnitude > 1)
            {
                movementInput.Normalize();
            }
        }
    }

    public bool Run { get; set; }

    private Vector3 DesiredMovementDirection
    {
        get
        {
            switch (movementPlane)
            {
                case MovementPlane.XY:
                    return directionReference.transform.right * MovementInput.x 
                        + directionReference.transform.up * MovementInput.y;
                case MovementPlane.XZ:
                    return directionReference.transform.right * MovementInput.x + directionReference.transform.forward * MovementInput.y;
                default:
                    return directionReference.transform.up * MovementInput.x 
                        + directionReference.transform.forward * MovementInput.y;
            }
        }
    }


    protected float SlopeMultiplier
    {
        get
        {
            float slopeAngle = Vector3.Angle(grounding.ContactNormal, Vector3.up);
            return slopeCurveModifier.Evaluate(slopeAngle);
        }
    }


    protected float currentTargetSpeed;

    protected virtual void UpdateDesiredTargetSpeed()
    {
        if (MovementInput == Vector2.zero)
            return;

        //float inputAngle = Mathf.Atan2(MovementInput.y, MovementInput.x) * Mathf.Rad2Deg;

        Vector3 movDir = DesiredMovementDirection.normalized;
        Vector3 cross = Vector3.Cross(movDir, transform.forward);
        float inputAngle = Vector3.SignedAngle(movDir, transform.forward, cross);

        currentTargetSpeed = 
            baseSpeed
            * directionSpeedFactor.Evaluate(inputAngle)
            * (Run ? runMultiplier : 1);

    }

    public Vector3 Velocity
    {
        get { return grounding.Rigidbody.velocity; }
    }


    protected void FixedUpdate()
    {
        if (Mathf.Abs(MovementInput.x) > float.Epsilon || Mathf.Abs(MovementInput.y) > float.Epsilon)
        {
            if (grounding.Grounded)
            {
                float velocityMagnitude = grounding.Rigidbody.velocity.magnitude;
                UpdateDesiredTargetSpeed();
                Vector3 desiredMove = DesiredMovementDirection;
                
                desiredMove = Vector3.ProjectOnPlane(desiredMove, grounding.ContactNormal);
                desiredMove = desiredMove.normalized;

                if (velocityMagnitude < currentTargetSpeed)
                {
                    Vector3 rawImpulse = desiredMove * SlopeMultiplier * motorForce * MovementInput.magnitude;
                    
                    Vector3 clampedImpulse = 
                        rawImpulse.normalized 
                        * Mathf.Clamp(
                            rawImpulse.magnitude,
                            0,
                            currentTargetSpeed * grounding.Rigidbody.mass / Time.fixedDeltaTime);

                    grounding.Rigidbody.AddForce(clampedImpulse);
                }
                grounding.DinamicDrag = true;
            }

        }
        else
        {
            grounding.DinamicDrag = false;
        }
        
    }


}
