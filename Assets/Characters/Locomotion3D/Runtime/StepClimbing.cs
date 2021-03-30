using System;
using UnityEngine;

public class StepClimbing : MonoBehaviour
{

#pragma warning disable CS0649
    [SerializeField] protected Grounding grounding;
    [SerializeField] protected GroundMovement movement;


    [SerializeField] protected float climbMaxVelocity = 2f;
    [SerializeField] protected float minStepAngle = 30f;
    [SerializeField] protected float maxStepDistance = 0.5f;
    [SerializeField] protected float maxStepHeight = 0.5f;
    [SerializeField] protected float climbImpulse = 30f;
    [SerializeField] protected float movementImpulse = 5f;

    [SerializeField] protected float offsetFromGround= 0.01f;
#pragma warning restore CS0649

    private Vector3 movementInput;


    protected void FixedUpdate()
    {

        movementInput = 
            movement.MovementInput.x * grounding.Rigidbody.transform.right +
            movement.MovementInput.y * grounding.Rigidbody.transform.forward;

        if (movementInput.sqrMagnitude <= float.Epsilon)
        {
            return;
        }
        
        if(grounding.Rigidbody.velocity.sqrMagnitude > 0.1f)
        {
            Vector3 velRelativeToInput = Vector3.Project(grounding.Rigidbody.velocity, movementInput);
            if (Vector3.Angle(velRelativeToInput, movementInput) > 90)
            {
                //Debug.Log("Climber's rigidbody is not going towards movement input");
                return;
            }
        }

        ClimbStep();
    }




    protected void ClimbStep()
    {

        if (grounding.Rigidbody.velocity.y >= climbMaxVelocity)
        {
            //Debug.Log("Max climb velocity reached ");
            return;
        }


        //Raycast to find steps towards movementDirection
        Vector3 groundPointBelow = grounding.FindPointBelow(5f);
        Vector3 hoverGroundingPosition = groundPointBelow + Vector3.up * offsetFromGround; //position just above the ground

        Vector3 stepCastDirection = Vector3.ProjectOnPlane(movementInput, grounding.ContactNormal).normalized;

        bool blockedPath = Physics.Raycast(
          hoverGroundingPosition,
           stepCastDirection,
          out RaycastHit hitInfo,
          maxDistance: maxStepDistance);


        if(!blockedPath)
        {
            //No potential steps to climb
            //Debug.Log("No Step found");
            return;
        }

        if(hitInfo.collider.attachedRigidbody == grounding.Rigidbody)
        {
            //Blocked by its own colldier
            //Debug.Log("Blocked by climber's collider");
            return;
        }

        Vector3 groundClimbDirection = Vector3.Cross(hitInfo.normal, Vector3.up);
        groundClimbDirection = Vector3.Cross(hitInfo.normal, groundClimbDirection).normalized;
        if(groundClimbDirection.y < 0)
        {
            groundClimbDirection = -groundClimbDirection;
        }


        float stepNormalAngle =  Vector3.Angle(Vector3.up, groundClimbDirection);
        Vector3 tallestStepPossibleTopPosition = 
            hitInfo.point 
            + groundClimbDirection * (1/Mathf.Cos(stepNormalAngle * Mathf.Deg2Rad)) * (maxStepHeight + offsetFromGround)
            + movementInput.normalized * 0.1f; //Adding an offset towards movementInput helps not missing a step

        //Check if not facing wall
        Vector3 tallestPositionFromRigidbody = tallestStepPossibleTopPosition - grounding.Rigidbody.position;
        bool wallHit = Physics.Raycast(
          grounding.Rigidbody.position,
           tallestPositionFromRigidbody,
          out RaycastHit wallHitInfo,
          maxDistance: tallestPositionFromRigidbody.magnitude);
        
        if(wallHit)
        {
            //Debug.Log("Wall hit");
            return;
        }

        //Find the top 'face' of the step
        bool stepHit = Physics.Raycast(
          tallestStepPossibleTopPosition,
           Vector3.down,
          out RaycastHit stepTopHit,
          maxDistance: maxStepHeight * 1.5f /*Magic number*/);

        if(!stepHit)
        {
            //Debug.Log("Cannot find a spot to climb on");
            return;
        }

        //Only consider clibing when the hit obstruction would block the path
        if(Vector3.Angle(hitInfo.normal, grounding.ContactNormal) < minStepAngle)
        {
            //Debug.Log("Obstruction is not too steep to form a step");
            return;
        }

        float stepHeight = stepTopHit.point.y - hoverGroundingPosition.y;
        float groundHeight = grounding.FarthestGroundingPostion.y - groundPointBelow.y;


        grounding.Rigidbody.AddForce(movementInput * movementImpulse, ForceMode.Impulse);

       

        if(stepHeight < -0.1f || stepHeight > maxStepHeight)
        {
            //Debug.Log("Step has negative hight or it is too tall - stepHeight: " + stepHeight);
            return;
        }
        
        if(groundHeight > stepHeight + offsetFromGround)
        {
            //Debug.Log("Climber is above the step");
            return;
        }

        /*
        Debug.DrawLine(
         hoverGroundingPosition,
         hitInfo.point,
         Color.black, 5);
        Debug.DrawLine(
            hitInfo.point,
            stepTopHit.point,
            Color.green, 5);
        Debug.DrawLine(
            tallestStepPossibleTopPosition + groundClimbDirection * 0.05f,
            tallestStepPossibleTopPosition - groundClimbDirection * 0.05f,
            Color.blue, 5);
        
        Debug.Log("Add force!!!");
        */
        grounding.Rigidbody.AddForce(Vector3.up * climbImpulse, ForceMode.Impulse);

    
    }



}
