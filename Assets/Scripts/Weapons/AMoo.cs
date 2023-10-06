using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMoo : MonoBehaviour
{
    protected Rigidbody _rigidBody;

    protected float _damage;
    
    public void SetDamage(float newDmg)
    {
        _damage = newDmg;
    }
}
