using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemiesDetecter : MonoBehaviour
{
    [SerializeField]
    private LayerMask _enemyLayer;

    [SerializeField]
    private List<Transform> _enemyList;

    IEnumerable<Enemy> EnemiesDetector(float radius, float amountOfEnemies)
    {
        List<Enemy> list = new ();

        var enemies = Physics.OverlapSphere(transform.position, radius, _enemyLayer);

        while(amountOfEnemies > 0)
        {
            foreach (var enemy in enemies)
            {
                var posibleEnemy = enemy.gameObject.GetComponent<Enemy>();

                if (posibleEnemy)
                {
                    if(posibleEnemy.DetectonChanceCalculate() < 25)
                    {
                        posibleEnemy.Moan();
                        posibleEnemy.Detected();
                        list.Add(posibleEnemy);
                    }
                } 
            }

            amountOfEnemies --;
        }

        return list;
    }

    public void Plays() => StartCoroutine(UpdateRadar(_enemyList, 10));
   

    IEnumerator UpdateRadar(List<Transform> targetList, float amoutEnemies) 
    {
        yield return new WaitForSeconds(.2f);

        Debug.Log("Corutina");

        _enemyList = EnemiesDetector(10, amoutEnemies).OfType<Enemy>().Select(x => x.transform).ToList();
    } 


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 10);
    }

}
