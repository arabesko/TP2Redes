using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : NetworkCharacterController
{
    
    public override void Move(Vector3 direction)
    {
        var deltaTime = Runner.DeltaTime;
        var previousPos = transform.position;
        var moveVelocity = Velocity;

        direction = direction.normalized;

        moveVelocity.y = 0f;

        Vector3 vel = transform.forward * direction.z * acceleration * deltaTime;
        float angle = direction.x * rotationSpeed * Runner.DeltaTime;
        transform.rotation = transform.rotation * Quaternion.Euler(0, angle, 0);

        _controller.Move(vel * deltaTime);

        // --- Bloquear posición Y ---
        Vector3 fixedPosition = transform.position;
        fixedPosition.y = 0f; // Fuerza Y=0
        transform.position = fixedPosition; // Sobrescribe cualquier cambio en Y

        transform.position = GameManager.Instance.AjustPositionToBounds(transform.position);

        Velocity = (transform.position - previousPos) * Runner.TickRate;
        Grounded = _controller.isGrounded;
    }
}
