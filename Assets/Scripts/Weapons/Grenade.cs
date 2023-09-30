using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grenade : MonoBehaviour
{
    [SerializeField]
    private LayerMask _groundMask;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _rigidBody.AddForce(transform.forward * 250);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<Grenade>() != null) return;

        Explode();
    }

    public void Explode()
    {
        var inRange = Physics.OverlapSphere(transform.position, 5);
        var idamageablesOnRange = inRange.OfType<IDamageable>();

        if (idamageablesOnRange.Any())
        {
            foreach (var item in idamageablesOnRange)
            {
                item.TakeDamage(17.5f);
            }

            var instaKIlleable = idamageablesOnRange.Where(x => x.GetLife() < 25);

            if (instaKIlleable.Any())
            {
                foreach(var item in instaKIlleable)
                {
                    item.InstaKill();
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5);
    }

}
