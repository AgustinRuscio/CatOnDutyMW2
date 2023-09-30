using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    private float _life;

    [SerializeField]
    private enemyType _enemyType;

    [SerializeField]
    private float _maxLife;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private Material _detectedMat, _normalMat;

    private void Awake()
    {
        _life = _maxLife;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void TakeDamage(float dmg)
    {
        _life -= dmg;

        if (_life <= 0)
        {
            Died();
        }
    }

    private void Died()
    { 
        //Destroy(gameObject);
    }

    private void OnDestroy()
    {
        
    }

    public float DetectonChanceCalculate()
    {
        switch (_enemyType)
        {
            case enemyType.runner:
                return UnityEngine.Random.Range(0, 37);
                break;
            case enemyType.tank:
                return UnityEngine.Random.Range(0, 101);
                break;
            case enemyType.thief:
                return UnityEngine.Random.Range(0, 251);
                break;

            default: 
                return 0;

        }
    }

    public void Moan()
    {
        _audioSource.Play();
    }

    public void Detected()
    {
        StartCoroutine(ChangeMat());
    }

    private IEnumerator ChangeMat()
    {
        _meshRenderer.material = _detectedMat;
        yield return new WaitForSeconds(.76f);
        _meshRenderer.material = _normalMat;
    }

    public float GetLife() => _life;

    public void InstaKill() => Died();
}

enum enemyType
{
    runner, 
    tank,
    thief
}