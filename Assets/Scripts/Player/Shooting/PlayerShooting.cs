using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [SerializeField] private Transform _firePoint;

    private void Start()
    {
        //Destroy(this.gameObject, 5);
    }

    public void Shoot()
    {
        if (!_bulletPrefab.IsValid || _firePoint == null) return;
        Runner.Spawn(_bulletPrefab, _firePoint.position, _firePoint.rotation);
    }
}
