using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    private float _life;

    private float _totalDamage;
    
    [SerializeField]
    private float _baseDamage;
    
    [SerializeField]
    private EnemyType _enemyType;

    [SerializeField]
    private float _maxLife;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private Material _detectedMat, _normalMat;
    
    private Tuple<String, int, EnemyType> _stats;

    [HideInInspector]
    public List<Pickables> _myItems = new ();

    [SerializeField]
    private Pickables[] _pickablesPrefabs;
    
    private int pickablesNum;
    
    [SerializeField] 
    private GameObject[] _pickablesSpawnPoints;
    
    public float Damage => _totalDamage;


    private void Awake()
    {
        _life = _maxLife;
        _meshRenderer = GetComponent<MeshRenderer>();
        _totalDamage = _baseDamage;

        StartCoroutine(wait());

    }
        
    IEnumerator wait()
    {
        yield return null;
        GameManager.instance.AddEnemy(this);
    }

    public Tuple<String, int, EnemyType>  MyStats()
    {
        return _stats;
    }

    public void SetStats(string name, int level)
    {
        _stats = new Tuple<string, int, EnemyType>(name, level, _enemyType);

        _totalDamage += (0.5f * _stats.Item2);

        switch (_enemyType)
        {
            case EnemyType.thief:
                _totalDamage += .5f;
                break;
            
            case EnemyType.runner:
                _totalDamage += 1f;
                break;
            
            case EnemyType.tank:
                _totalDamage += .25f;
                break;
        }
        
        
        
        Debug.Log(_stats.Item1 + " : " + _stats.Item2 + " : " + _stats.Item3 + " : " + "Total Damage " + _totalDamage);
    }
    
    public void TakeDamage(float dmg)
    {
        _life -= dmg;
    Debug.Log("Auch");
        if (_life <= 0)
        {
            Died();
            Debug.Log("Mori");
        }
    }

    private void Died()
    {
        pickablesNum = Random.Range(0, 3);
        
        for (int i = 0; i < pickablesNum; i++)
        {
            var randomizer = Random.Range(0, 3);

            var toIntanciate = _pickablesPrefabs[randomizer];
            
            var item =Instantiate(toIntanciate, _pickablesSpawnPoints[i].transform.position,
                _pickablesSpawnPoints[i].transform.rotation);
            
            item.SetType(PickableType.Legendary);
            
            _myItems.Add(item);
        }
        
        GameManager.instance.RemoveEnemy(this);
        Destroy(gameObject);
    }


    public float DetectonChanceCalculate()
    {
        switch (_enemyType)
        {
            case EnemyType.runner:
                return Random.Range(0, 37);
            case EnemyType.tank:
                return Random.Range(0, 101);
            case EnemyType.thief:
                return Random.Range(0, 251);
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

public enum EnemyType
{
    runner, 
    tank,
    thief
}