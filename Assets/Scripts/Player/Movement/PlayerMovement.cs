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

        if (Grounded && moveVelocity.y < 0)
        {
            moveVelocity.y = 0f;
        }

        moveVelocity.y += gravity * Runner.DeltaTime;

        //var horizontalVel = default(Vector3);
        //horizontalVel.x = moveVelocity.x;
        //horizontalVel.z = moveVelocity.z;

        //if (direction == default)
        //{
        //    horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
        //}
        //else
        //{
        //    horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Runner.DeltaTime);
        //}

        //moveVelocity.x = horizontalVel.x;
        //moveVelocity.z = horizontalVel.z;

        Vector3 vel = transform.forward * direction.z * acceleration * deltaTime;
        float angle = direction.x * rotationSpeed * Runner.DeltaTime;
        transform.rotation = transform.rotation * Quaternion.Euler(0, angle, 0);

        _controller.Move(vel * deltaTime);

        Velocity = (transform.position - previousPos) * Runner.TickRate;
        Grounded = _controller.isGrounded;
    }
}
