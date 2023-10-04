using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if(posibleEnemy.DetectonChanceCalculate() > 25) continue;
                
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
        string[] names = {"Bartolo", "Apolo", "Carlito", "", "Pichico", "Pepiña", "Cochito", "Pololo", "Max", "Chicho" };
        
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

}
