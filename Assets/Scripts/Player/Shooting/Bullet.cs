using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float _speed;
    public override void FixedUpdateNetwork()
    {
        transform.position += transform.forward * _speed * Runner.DeltaTime;
    }

    private void Start()
    {
        Destroy(gameObject, 3f);
    }
}
