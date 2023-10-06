using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    private LifeBar _lifeBar;

    [SerializeField]
    private float _damage;
    
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
    private List<AMoo> _myBullets   = new ();

    private int _bulletsReady = 100;
    [SerializeField]
    private int _maxBulletsPossible = 100;
    
    [SerializeField] 
    private List<AMoo> _mygranades = new ();
    private int _grenadesReady = 60;
    [SerializeField]
    private int _maxgrebadesPossible = 90;
    
    [SerializeField] 
    private CanvasGroup _lastEnemyKill;

    private Enemy _currentEnemy;
    private int _currentEnemyIndex;

    public Enemy Current => _currentEnemy;

    public void NullIt() => _currentEnemy = null;
    

    [SerializeField]
    private LayerMask _enemyMas;

    private List<Enemy> a = new List<Enemy>();
    List<Transform> nearEnemies= new List<Transform>();

    public List<Enemy> targets = new List<Enemy>();
    
    private bool canFetch = true;

    private float _timerToClean;
    private float _timerToGiveAmmo;

    [Header("States")]
    private bool _canThrow = true;
    private bool _shooting = true;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI _grenadeText, _bulletsText, _ammoTotalText;

    private void Awake()
    {
        _life = _maxLife;

        _rigidbody = GetComponent<Rigidbody>();

        PlayerController controller = new PlayerController(this);
        ArtificialFixedUpdateMethods += controller.ArtificialFixedUpdate;
        ArtificialUpdateMethods += controller.ArtificialUpdate;

        SetUINumbers();
    }

    private void Start()
    {
        _lifeBar.UpdateLifeBar(_life ,_maxLife);
        Debug.Log(_life / _maxLife);
    }

    public float GetLife() => _life;

    public void InstaKill() => Die();

    public void TakeDamage(float dmg)
    {
        _life -= dmg;
        _lifeBar.UpdateLifeBar(_life ,_maxLife);
        
        if (_life <= 0)
            Die();
    }

    private void Die()
    {
        GameManager.instance.GameEnd(false);
    }

    private void Update()
    {
        ArtificialUpdateMethods();
        
        if (_currentEnemy != null)
        {
           Vector3 directionToPlayer = _currentEnemy.transform.position - transform.position;

           directionToPlayer.y = 0;
           
           transform.forward = directionToPlayer.normalized;
           transform.position = new Vector3(transform.position.x, .5f, transform.position.z);
        }
        _timerToClean += Time.deltaTime;

        if (_timerToClean >= 7 && !_currentEnemy)
            targets = new();
        
        if(Input.GetKeyDown(KeyCode.P))
            GameManager.instance.GameEnd(true);
        
        if(Input.GetKeyDown(KeyCode.O))
            GameManager.instance.GameEnd(false);

        _timerToGiveAmmo += Time.deltaTime;

        if (_timerToGiveAmmo > 10)
        {
            _bulletsReady++;
            if (_bulletsReady > _maxBulletsPossible)
                _bulletsReady = _maxBulletsPossible;
            
            _grenadesReady++;
            if (_grenadesReady > _maxgrebadesPossible)
                _grenadesReady = _maxgrebadesPossible;

            SetUINumbers();
            _timerToGiveAmmo = 0;
        }
        CheckAmmo();
    }

   

    void FixedUpdate()
    {
        ArtificialFixedUpdateMethods();
    }


    public void ShowLastEnemy(bool active)
    {
       _lastEnemyKill.alpha = active ? 1 :  0;
    }

    private float CheckAmmo()
    {
        return _myBullets.Concat(_mygranades).Count();
    }

    private void SetUINumbers()
    {
        _grenadeText.text = _grenadesReady.ToString();
        _bulletsText.text = _bulletsReady.ToString();
        _ammoTotalText.text = CheckAmmo().ToString();
    }
    
    #region  Features
        public void GetHealth(float amount)
        {
            _life += amount;
            
            if (_life > _maxLife)
                _life = _maxLife;
            
            _lifeBar.UpdateLifeBar(_life ,_maxLife);
        }

        public void GetAmmo(int howMuch)
        {
            _bulletsReady += howMuch;
            if (_bulletsReady > _maxBulletsPossible)
                _bulletsReady = _maxBulletsPossible;

            _grenadesReady += howMuch;
            if (_grenadesReady > _maxgrebadesPossible)
                _grenadesReady = _maxgrebadesPossible;
            
            
            SetUINumbers();
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
            nearEnemies = a.EnemiesDetecting(transform, 30, 10, _enemyMas).Select(x => x.transform).ToList();
        }
        IEnumerator CanFetchAgain()
        {
            yield return new WaitForSeconds(5f);
            canFetch = true;
        }

        public void TargetSystem()
        {
            if (!targets.Any())
            {
                targets = Physics.OverlapSphere(transform.position, 5, _enemyMas).Select(x=> x.GetComponent<Enemy>()).Where(x=>x.Alive).ToList();

                if (targets.Any())
                    _currentEnemy = targets.First();
            }
            else
            {
                var a = targets.SkipWhile(x => x == _currentEnemy);

                if (a.Any())
                    _currentEnemy = a.First();

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
        
        if(_grenadesReady <= 0) return;

        _grenadesReady--;
        
        var grenade = Instantiate(_grenadePrefab, _grenadeSpawnPoint.position, _grenadeSpawnPoint.rotation);
        grenade.SetDamage(25);
        _myBullets.Add(grenade);
        
        _canThrow = false;
        SetUINumbers();
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
        while (_shooting && _bulletsReady >0)
        {
            _bulletsReady--;
            yield return new WaitForSeconds(.25f);
            
            var bullet = Instantiate(_bulletPrefab, _bulletsSpawnPoint.position, _bulletsSpawnPoint.rotation);
            bullet.SetDamage(_damage);
            _myBullets.Add(bullet);
            SetUINumbers();
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