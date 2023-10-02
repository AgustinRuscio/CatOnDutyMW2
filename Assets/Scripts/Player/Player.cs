using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float _maxLife;

    private float _life;

    [SerializeField]
    private Grenade _grenadePrefab;

    [SerializeField]
    private Bullet _bulletPrefab;

    [SerializeField]
    private Transform _grenadeSpawnPoint, _bulletsSpawnPoint;

    [SerializeField]
    private float _speed, _rotationSpeed;

    private Rigidbody _rigidbody;

    private event Action ArtificialFixedUpdateMethods = delegate { };
    private event Action ArtificialUpdateMethods = delegate { };

    
    [SerializeField]
    private LayerMask _enemyMas;

    private List<Enemy> a = new List<Enemy>();
    List<Transform> nearEnemies= new List<Transform>();

    private bool canFetch = true; 


    [Header("States")]
    private bool _canThrow = true;
    private bool _shooting = true;


    private void Awake()
    {
        _life = _maxLife;

        _rigidbody = GetComponent<Rigidbody>();

        PlayerController controller = new PlayerController(this);
        ArtificialFixedUpdateMethods += controller.ArtificialFixedUpdate;
        ArtificialUpdateMethods += controller.ArtificialUpdate;
    }

    private List<Enemy> enemies = new ();

    public float GetLife() => _life;

    public void InstaKill() => Die();

    public void TakeDamage(float dmg)
    {
        _life -= dmg;

        if (_life <= 0)
            Die();
    }

    public void Die()
    {
        
    }

    private void Update()
    {
        ArtificialUpdateMethods();

        if (Input.GetKey(KeyCode.I) && canFetch)
        {
            Debug.Log("Try");
            canFetch = false;
            StartCoroutine(CanFetchAgain());
            nearEnemies = a.EnemiesDetecting(transform, 10, 10, _enemyMas).Select(x => x.transform).ToList();
            
        }
        
        Debug.Log(nearEnemies.Count + "a");
    }

    IEnumerator CanFetchAgain()
    {
        yield return new WaitForSeconds(5f);
        canFetch = true;

    }

    void FixedUpdate()
    {
        ArtificialFixedUpdateMethods();
    }

    #region Attacking

    public void ThrowingGrenade()
    {
        if (!_canThrow) return;
        
        Instantiate(_grenadePrefab, _grenadeSpawnPoint.position, _grenadeSpawnPoint.rotation);
        _canThrow = false;
        StartCoroutine(CDCorutine( _canThrow));
    }

    public void StartShooting()
    {
         _shooting = true;
        StartCoroutine(Shooting());
    }

    public void StopShooting() => _shooting = false;
    
    IEnumerator Shooting()
    {
        while (_shooting)
        {
            yield return new WaitForSeconds(.1f);
            Instantiate(_bulletPrefab, _bulletsSpawnPoint.position, _bulletsSpawnPoint.rotation);
        }
    }

    private IEnumerator CDCorutine(bool state)
    {
        yield return new WaitForSeconds(1);
        _canThrow = true;
    }

    #endregion


    #region Movement
    public void Rotate(float z)
    {
        Vector3 rotation = new Vector3(0, z * _rotationSpeed * Time.fixedDeltaTime, 0);
        Quaternion deltaRotation = Quaternion.Euler(rotation);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }

    public void Move(float x)
    {
        Vector3 pos = transform.forward * x;

        pos *= _speed * Time.deltaTime;
        pos += transform.up * _rigidbody.velocity.y;

        _rigidbody.velocity = pos;
    }
    #endregion
}