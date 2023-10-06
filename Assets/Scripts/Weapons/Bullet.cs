using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : AMoo
{
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _rigidBody.AddForce(transform.forward * 1000);

        Destroy(gameObject, 7.5f);
    }


    private void OnTriggerEnter(Collider other)
    {
        var col = other.gameObject.GetComponent<IDamageable>();

        if (col == null) return;

        col.TakeDamage(_damage);

        Destroy(gameObject);
    }

}