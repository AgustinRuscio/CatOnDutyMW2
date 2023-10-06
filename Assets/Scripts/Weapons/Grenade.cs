using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grenade : AMoo
{
    [SerializeField]
    private LayerMask  _enemyMask;


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _rigidBody.AddForce(transform.forward * 250);
        
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Grenade>() != null) return;

        Explode();
    }


    public void Explode()
    {
        var inRange = Physics.OverlapSphere(transform.position, 15, _enemyMask);
        Debug.Log(inRange.Count() + " aaa in range");

        List<GameObject> a = new List<GameObject>();
        
        
        var idamageablesOnRange = inRange.Select(x=>x.GetComponent<Enemy>()).OfType<IDamageable>().ToArray();
        
        Debug.Log(idamageablesOnRange.Length + " Damageable in range");
        
        Debug.Log("Entro");
        
        if (idamageablesOnRange.Any())
        {
        Debug.Log("Algo habia");
            foreach (var item in idamageablesOnRange)
            {
                item.TakeDamage(_damage);
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
