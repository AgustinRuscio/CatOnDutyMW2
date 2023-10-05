using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private Player _player;
    
    private string names;
    private string[] _allNames;
    
    private int lvls;
    private int[] _allLvls;

    private List<Enemy> _enemies = new();
    private List<Enemy> _enemiesKilled = new();

    [SerializeField]
    private TextMeshProUGUI _lastKilled, _enemyKilledCount, _mostPowerful, _objectCollected, _enemuesKilledFinal;

    private float _mostPowerfulDMg;

    private IEnumerable<Pickables> _allPickablesColledted;

    [SerializeField] 
    private GameObject _losePanel, _winPanel, _finalStats;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return null;
        SetEnemiesData();
    }
    private void SetEnemiesData()
    {
        _allNames = names.EnemyNaming(_enemies.Count).ToArray();
            
        string[] nickName = {"The viking", "El barbaro", "El sucio", "The beast", "El loco", "Crazy 8", "The gangster", "El malenate", "El gigante", "El bajito" };
        int[] dinasty = {4,6,8,9,13, 12, 11,5,7};
        
        var completeName = _allNames.Zip(nickName, (n, N) => n + " " + N).Zip(dinasty, (n, N) => n + " " + N + "th").ToArray();;

        _allLvls = lvls.EnemyLeveling(_enemies.Count).ToArray();

        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].SetStats(completeName[i], _allLvls[i]);
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        if(_enemies.Contains(enemy)) return;
        
        _enemies.Add(enemy);

            
    }
    public void RemoveEnemy(Enemy enemy)
    {
        if(!_enemies.Contains(enemy)) return;
        
        _enemies.Remove(enemy);
        
        _enemiesKilled.Add(enemy);
 
        Debug.Log("Update");
        k = _enemiesKilled.Last().MyStats();
        
        _lastKilled.text = k.Item1 + " - " + k.Item2 + " - " + k.Item3;
        
        _mostPowerfulDMg = _enemiesKilled.Count == 1 ? _enemiesKilled.Last().Damage : _enemiesKilled.Select(x=> x.Damage).OrderByDescending(x=>x).First();
        _mostPowerful.text = _mostPowerfulDMg.ToString();
        
        _enemyKilledCount.text = _enemiesKilled.Count.ToString();
    }
    
    public void GameEnd(bool matchWin)
    {
        if(matchWin) _winPanel.SetActive(true);
        else _losePanel.SetActive(true);

        _finalStats.SetActive(true);
        _allPickablesColledted = _enemiesKilled.SelectMany(x => x._myItems);
        
        _objectCollected.text = _allPickablesColledted.Count().ToString();
        _enemuesKilledFinal.text = _enemiesKilled.Count.ToString();
    }
    
    private Tuple<String, int, EnemyType> k;
}