using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarDash : MonoBehaviour
{
    [SerializeField] private Character2D _character;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private VelocityMovement _movement;
    
    [SerializeField] private float _speed;
    [SerializeField] private float _staminaCost;


    public void StartDash()
    {
        _character.SpendStamina(_staminaCost);
    }

    private void OnDash()
    {
        Vector2 dashDirection = _movement.MovementInput;

        if (dashDirection.sqrMagnitude == 0)
        {
            dashDirection = _rigidbody.transform.right * _rigidbody.transform.localScale.x;
        }
        _rigidbody.velocity = dashDirection * _speed;

    }

}
