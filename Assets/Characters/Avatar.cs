using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : MonoBehaviour
{
    [SerializeField] private GroundMovement movement = default;

    public void Input(InputAction.CallbackContext context)
    {
        movement.MovementInput = context.ReadValue<Vector2>();

    }
}
