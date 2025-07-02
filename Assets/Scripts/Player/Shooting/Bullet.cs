using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float _speed;

    private TickTimer _lifeTimer;
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _lifeTime = 2;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            _lifeTimer = TickTimer.CreateFromSeconds(Runner ,_lifeTime);
        }
    }

    public override void FixedUpdateNetwork()
    {
        transform.position += transform.forward * _speed * Runner.DeltaTime;

        if(_lifeTimer.Expired(Runner))
        {
            DespawnObject();
        }
    }

    void DespawnObject()
    {
        _lifeTimer = TickTimer.None;
        Runner.Despawn(Object);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        if (other.TryGetComponent(out LifeHandler lifeHandler))
        {
            lifeHandler.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }
}
