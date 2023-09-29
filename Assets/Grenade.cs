using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public void Explode()
    {
        var inRange = Physics.OverlapSphere(transform.position, 10);
        var idamageablesOnRange = inRange.OfType<IDamageable>();

        if (idamageablesOnRange.Any())
        {
            foreach (var item in idamageablesOnRange)
            {
                item.TakeDamage(17.5f);
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10);
    }

}
