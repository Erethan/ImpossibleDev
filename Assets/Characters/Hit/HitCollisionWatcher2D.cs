using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitCollisionWatcher2D : MonoBehaviour
{

    [SerializeField] private UnityEvent<Collider2D> _triggerEnter;
    [SerializeField] private UnityEvent<Collider2D> _triggerExit;

    [SerializeField] private UnityEvent<Collision2D> _collisionEnter;
    [SerializeField] private UnityEvent<Collision2D> _collisionExit;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _triggerEnter.Invoke(collider);

    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        _triggerExit.Invoke(collider);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collisionEnter.Invoke(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _collisionExit.Invoke(collision);

    }
}
