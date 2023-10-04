using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _rigidBody.AddForce(transform.forward * 1000);
        
        Destroy(gameObject, 10);
    }
    private void OnCollisionEnter(Collision collision)
    {
        var col = collision.collider.gameObject.GetComponent<IDamageable>();

        if (col == null) return;

        col.TakeDamage(105);

        Destroy(gameObject);
    }
}