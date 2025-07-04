using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private List<Transform> _firePointPower;
    
    public bool _isPowerActivate;
    [Networked] private TickTimer TimerPower { get; set; }
    [SerializeField] private float _timePower = 10;

    private void Start()
    {
        //Destroy(this.gameObject, 5);
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority) // Solo el host inicia el ciclo
        {
            _isPowerActivate = false;
        }
    }

    public void ActivatePower()
    {
        _isPowerActivate = true;
        TimerPower = TickTimer.CreateFromSeconds(Runner, _timePower);
    }

    public override void FixedUpdateNetwork()
    {
        if (TimerPower.Expired(Runner))
        {
            _isPowerActivate = false;
            TimerPower = TickTimer.None;
        }

       
    }

    public void Shoot()
    {
        if (!_bulletPrefab.IsValid || _firePoint == null) return;
        
        if (!_isPowerActivate)
        {
           Runner.Spawn(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        }
        else
        {
            foreach (Transform points in _firePointPower)
            {
                Runner.Spawn(_bulletPrefab, points.position, _firePoint.rotation);
            }
        }
    }
}
