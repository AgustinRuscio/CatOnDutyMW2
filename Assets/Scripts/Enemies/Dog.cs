using System.Collections;
using UnityEngine;

public class Dog : Enemy
{
    private Player _player;

    [SerializeField]
    private Bullet _bulletsPrefb;

    [SerializeField]
    private Transform _shootPoint;

    private bool _canShoot = true;
    
    protected override void Awake()
    {
        base.Awake();

        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if(!_alive) return;
        
        if (_player != null)
        {
            Vector3 directionToPlayer = _player.transform.position - transform.position;
            transform.forward = directionToPlayer.normalized;
        }
        
        if (Vector3.Distance(_player.transform.position, transform.position) < 10)
        {
            if (_canShoot)
            {
                var bullet = Instantiate(_bulletsPrefb, _shootPoint.position, _shootPoint.rotation);
                bullet.SetDamage(Damage);
                _canShoot = false;
                StartCoroutine(CanShootAgain());
            }
        }
        else
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, _player.transform.position, Time.deltaTime * 15f);
            transform.position = newPosition;
        }
    }

    IEnumerator CanShootAgain()
    {
        yield return new WaitForSeconds(2f);
        _canShoot = true;
    }
}