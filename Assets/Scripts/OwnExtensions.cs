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
                    //lista.Add(posibleEnemy);
                
            }
            else 
                yield return null;
            

        }

       // return lista;
    }
}
