using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoapController : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Transform camera;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float gravity;
    [SerializeField] float maxSpeed;
    [SerializeField] float meltSpeed;
        
    Vector3 input;
    Vector3 velocity;
    
    void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }
    
    void Move()
    {
        var forward = camera.forward;
        forward.y = 0;

        var right = camera.right;
        right.y = 0;

        var direction = input.x * right + input.y * forward;
        velocity += direction.normalized * (acceleration * Time.fixedDeltaTime);
        velocity.y = -gravity;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        rigidbody.linearVelocity = velocity;
        velocity = Vector3.MoveTowards(velocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
    }

    void Melt()
    {
        if (rigidbody.linearVelocity.sqrMagnitude < 0.1f) return;
        
        var scale = transform.localScale;
        scale = Vector3.MoveTowards(scale, Vector3.one * 0.1f, meltSpeed * Time.deltaTime);
        transform.localScale = scale;
    }

    void Update()
    {
        Melt();
    }

    void FixedUpdate()
    {
        Move();
    }
}
