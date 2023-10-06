using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class OwnExtensions 
{
    public static IEnumerable<Enemy> EnemiesDetecting(this IEnumerable<Enemy> lista, Transform transform ,float radius, float amountOfEnemies, LayerMask _enemyLayer)
    {   
        var enemies = Physics.OverlapSphere(transform.position, radius, _enemyLayer);

        foreach (var enemy in enemies)
        {
            var posibleEnemy = enemy.gameObject.GetComponent<Enemy>();

            if (posibleEnemy != null)
            {
                //if(posibleEnemy.DetectonChanceCalculate() > 25) continue;
                
                Debug.Log("qiee");
                    posibleEnemy.Moan();
                    posibleEnemy.Detected();
                    yield return posibleEnemy;
            }
            else 
                yield return null;
        }
    }

    public static IEnumerable<string> EnemyNaming(this string name, int numberOfEnemies)
    {
        string[] names = {"Bartolo", "Apolo", "Carlito", "Gay Tony", "Pichico", "Pepiña", "Cochito", "Pololo", "Max", "Chicho" };
        //var a = completeName.Zip(dinasty, (n, N) => n + " " + N + "th").ToArray();
        
        while (numberOfEnemies > 0)
        {
            numberOfEnemies--;
            
            int random = Random.Range(0, names.Length);
            name = names[random];
            
            yield return name;
        }
    }
    public static IEnumerable<int> EnemyLeveling(this int lvls, int numberOfEnemies)
    {
        int[] levels = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        while (numberOfEnemies > 0)
        {
            numberOfEnemies--;

            int random = Random.Range(0, levels.Length);
            lvls = levels[random];

            yield return lvls;
        }
    }

    public static IEnumerable<Enemy> EnemyGenre(this IEnumerable<Enemy> enemies)
    {
        foreach (var enemy in enemies)
        {
            var value = Random.Range(0, 101);
            
            if(value % 2 == 0)
                yield return enemy;
            
        }
    }

    public static IEnumerable<Tuple<int, Dificulty>> DifficultySetter(this int wavesAmount)
    {
        Dificulty _dif;
        int enemiesPerWave;
        
        
        for (int i = 0; i < wavesAmount; i++)
        {
            var randomizer = Random.Range(0, 101);

            if (randomizer < 25)
            {
                _dif = Dificulty.Easy;
                enemiesPerWave = 5;
            }
            else if (randomizer >= 25 && randomizer < 75)
            {
                _dif = Dificulty.Medium;
                enemiesPerWave = 7;
            }
            else
            {
                _dif = Dificulty.Hard;
                enemiesPerWave = 10;
            }
            
            yield return new Tuple<int, Dificulty>(enemiesPerWave, _dif);
        }
    }
}