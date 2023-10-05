using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
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

    [SerializeField]
    private Light _pointLight;

    private event Action ArtificialFixedUpdateMethods = delegate { };
    private event Action ArtificialUpdateMethods = delegate { };

    [SerializeField] 
    private CanvasGroup _lastEnemyKill;

    private Enemy _currentEnemy;
    private int _currentEnemyIndex;
    
    [SerializeField]
    private LayerMask _enemyMas;

    private List<Enemy> a = new List<Enemy>();
    List<Transform> nearEnemies= new List<Transform>();

    private bool canFetch = true;

    private float _timerToClean;

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

    public float GetLife() => _life;

    public void InstaKill() => Die();

    public void TakeDamage(float dmg)
    {
        _life -= dmg;

        if (_life <= 0)
            Die();
    }

    private void Die()
    {
        
    }

    private void Update()
    {
        ArtificialUpdateMethods();
        
        if (_currentEnemy != null)
            transform.LookAt(_currentEnemy.transform.position);

        _timerToClean += Time.deltaTime;

        if (_timerToClean >= 7 && !_currentEnemy)
            targets = new();
        
        if(Input.GetKeyDown(KeyCode.A))
            GameManager.instance.GameEnd(true);
        
        if(Input.GetKeyDown(KeyCode.B))
            GameManager.instance.GameEnd(false);
    }

   

    void FixedUpdate()
    {
        ArtificialFixedUpdateMethods();
    }


    public void ShowLastEnemy(bool active)
    {
       _lastEnemyKill.alpha = active ? 1 :  0;
    }

    #region  Features
        public void GetHealth(float amount)
    {
        _life += amount;
        
        if (_life > _maxLife)
            _life = _maxLife;
    }

        public void IncreaseSpeed(float multiplayer, float time)
        {
            var normalSpeed = _speed;
            _speed *= multiplayer;

            StartCoroutine(ReturnToNormalSpeed(normalSpeed, time));
        }

        IEnumerator ReturnToNormalSpeed(float normalSpeed, float time )
        {
            yield return new WaitForSeconds(time);
            _speed = normalSpeed;
        }

        public void TurnLightsOn(float time)
        {
            Debug.Log("Lights On");
            _pointLight.gameObject.SetActive(true);
            
            StartCoroutine(TurnLightOff(time));
        }

        IEnumerator TurnLightOff(float time)
        {
            yield return new WaitForSeconds(time);
            _pointLight.gameObject.SetActive(false);
        }
        
        public void Featch()
        {
            if(!canFetch) return;
            
            Debug.Log("Try");
            canFetch = false;
            StartCoroutine(CanFetchAgain());
            nearEnemies = a.EnemiesDetecting(transform, 10, 10, _enemyMas).Select(x => x.transform).ToList();
        }
        IEnumerator CanFetchAgain()
        {
            yield return new WaitForSeconds(5f);
            canFetch = true;
        }

        private List<Enemy> targets = new List<Enemy>();
        public void TargetSystem()
        {
            if (!targets.Any())
            {
                var near = Physics.OverlapSphere(transform.position, 5, _enemyMas);

                foreach (var VARIABLE in near)
                {
                    var current = VARIABLE.GetComponent<Enemy>();
                    
                    if(current == null) continue;
                    
                    targets.Add(current);
                }
                
                if (targets.Any())
                    _currentEnemy = targets.First();
            }
            else
            {
                _currentEnemy = targets.SkipWhile(x => x == _currentEnemy).First();
            }
        }

        public void ReleasTarget()
        {
            _currentEnemy = null;
            targets = new();
        }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5);
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
            yield return new WaitForSeconds(.25f);
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