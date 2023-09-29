using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float _life;

    public void TakeDamage(float dmg)
    {
        _life -= dmg;

        if (_life <= 0)
        {
            Died();
        }
    }

    private void Died() => Destroy(gameObject);
    
}
