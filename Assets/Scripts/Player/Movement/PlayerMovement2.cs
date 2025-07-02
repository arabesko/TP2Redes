using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;

    [SerializeField] private NetworkRigidbody3D _netRB;
    private float _xAxis;
    private float _zAxis;

    void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");
    }

    public override void FixedUpdateNetwork()
    {
        MovementPlayer();
        RotationPlayer();
    }

    private void MovementPlayer()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 vel = transform.forward * _zAxis * _speed * Runner.DeltaTime;
        _netRB.Rigidbody.velocity = Vector3.ClampMagnitude(vel, _speed);
    }

    private void RotationPlayer()
    {
        if (_xAxis == 0) return;
        float angle = _xAxis * _speedRotation * Runner.DeltaTime;
        _netRB.Rigidbody.MoveRotation(_netRB.Rigidbody.rotation * Quaternion.Euler(0, angle, 0));
    }
}
