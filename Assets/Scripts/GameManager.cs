using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private Enemy _enemyPrefab;
    
    [SerializeField]
    private TextMeshProUGUI _lastKilled, _enemyKilledCount, _mostPowerful, _objectCollected, _enemuesKilledFinal, _difficulty;

    private float _mostPowerfulDMg;

    private IEnumerable<Pickables> _allPickablesColledted;

    [SerializeField] 
    private GameObject _losePanel, _winPanel, _finalStats;

    private List<Tuple<int, Dificulty, GameObject[]>> wavesData = new List<Tuple<int, Dificulty, GameObject[]>>();
    
    [SerializeField] 
    private GameObject[] wavesSpawnPoints;

    private int _enemyCounter;
    private int _waveCounter;
    
    
    private Tuple<String, int, EnemyType> k;

    private List<Enemy> _enemiesInInstance = new List<Enemy>();
    private bool _waveReady = false;
    private bool _readyToSubstract;
    
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        Time.timeScale = 1;
        
        var roundsSelected = Random.Range(2, 5);
        
        var ammountAndDifficulty = roundsSelected.DifficultySetter().ToArray();
        _waveCounter = 0;
        
        //Debug.Log(ammountAndDifficulty.Length +"aaaaa ");
        
        for (int i = 0; i < ammountAndDifficulty.Count(); i++)
        {
            wavesData.Add(new Tuple<int, Dificulty, GameObject[]>(ammountAndDifficulty[i].Item1, ammountAndDifficulty[i].Item2, wavesSpawnPoints));
        }
        
        Debug.Log(wavesData.Count() +" wave data ");
        _player = FindObjectOfType<Player>();
        
    }

    private void Start()
    {
        StartCoroutine(wait());
    }

    private void Update()
    {
        WaveLoop();
    }

    IEnumerator wait()
    {
        yield return null;
        SetEnemiesData();
        
        StartWave();
    }

    private void StartWave()
    {
        var enemies = wavesData[_waveCounter].Item1;
        
        _difficulty.text = wavesData[_waveCounter].Item2.ToString();
        var spawns = wavesData[_waveCounter].Item3;
        
        //Debug.Log("Wave number: " + _waveCounter);
        
        StartCoroutine(wavesInstanciate(enemies,spawns));
    }

    IEnumerator wavesInstanciate(int amountOfEnemies, GameObject[] spawns)
    {
        yield return new WaitForSeconds(7.5f);
        
        while (amountOfEnemies > 0)
        {
            int randomPoint = Random.Range(0, spawns.Length);

            var enemy = Instantiate(_enemyPrefab, spawns[randomPoint].transform.position,spawns[randomPoint].transform.rotation);
            yield return new WaitForSeconds(.75f);

            amountOfEnemies--;
            _waveReady = true;
        }
    }

    private void WaveLoop()
    {
        if(!_waveReady) return;
        
        if (_enemiesInInstance.Count <= 0)
        {
            _waveReady = false;
            _readyToSubstract = true;
            WaveEnd();
        }
    }
    
    private void WaveEnd()
    {
        if (_readyToSubstract)
        {
            _readyToSubstract = false;
            _waveCounter++;
            
            if (_waveCounter >= wavesData.Count)
                GameEnd(true);
            else
                StartWave();
        }
    }
    
    private void SetEnemiesData()
    {
        _allNames = names.EnemyNaming(_enemies.Count).ToArray();
            
        string[] nickName = {"The viking", "El barbaro", "El sucio", "The beast", "El loco", "Crazy 8", "The gangster", "El malenate", "El gigante", "El bajito" };
        int[] dinasty = {4,6,8,9,13, 12, 11,5,7};
        
        var completeName = _allNames.Zip(nickName, (n, N) => n + " " + N).Zip(dinasty, (n, N) => n + " " + N + "th").ToArray();;

        _allLvls = lvls.EnemyLeveling(_enemies.Count).ToArray();

        var femaleEnemies = _enemies.EnemyGenre();
        
        //Debug.Log(femaleEnemies.Count());
        
        foreach (var female in femaleEnemies)
        {
            female.SetFemale();
        }
        
        for (int i = 0; i < _enemies.Count-1; i++)
        {
            _enemies[i].SetStats(completeName[i], _allLvls[i]);
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        if(_enemies.Contains(enemy)) return;
        
        _enemiesInInstance.Add(enemy);
        _enemies.Add(enemy);
        
        SetEnemiesData();
    }
    public void RemoveEnemy(Enemy enemy)
    {
        if (_enemiesInInstance.Contains(enemy))
            _enemiesInInstance.Remove(enemy);
            
        if(!_enemies.Contains(enemy)) return;

        enemy._alive = false;
        
        _enemiesKilled.Add(enemy);
        _enemies.Remove(enemy);
        
        if(_player.Current == enemy)
            _player.NullIt();

        if (_player.targets.Contains(enemy))
            _player.targets.Remove(enemy);
        
 
        Debug.Log("Update");
        k = _enemiesKilled?.Last()?.MyStats();
        
        _lastKilled.text = k?.Item1 + " - " + k?.Item2 + " - " + k?.Item3;
        
        _mostPowerfulDMg = _enemiesKilled.Count == 1 ? _enemiesKilled.Last().Damage : _enemiesKilled.Select(x=> x.Damage).OrderByDescending(x=>x).First();
        _mostPowerful.text = _mostPowerfulDMg.ToString();
        
        _enemyKilledCount.text = _enemiesKilled.Count.ToString();
    }
    
    public void GameEnd(bool matchWin)
    {
        Time.timeScale = 0;
        
        if(matchWin) _winPanel.SetActive(true);
        else _losePanel.SetActive(true);

        _finalStats.SetActive(true);
        _allPickablesColledted = _enemiesKilled.SelectMany(x => x._myItems);
        
        _objectCollected.text = _allPickablesColledted.Count().ToString();
        _enemuesKilledFinal.text = _enemiesKilled.Count.ToString();
    }
    
}

public enum Dificulty
{
    Easy, Medium, Hard
}