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

    void FixedUpdate()
    {
        Move();
    }
}
