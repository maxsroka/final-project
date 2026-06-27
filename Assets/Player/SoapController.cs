

using UnityEngine;
using UnityEngine.InputSystem;

public class SoapController : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Transform camera;
    [SerializeField] float speed = 5f;
    [SerializeField] float gravity = 1f;
        
    Vector3 input;
    
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
        var velocity = direction.normalized * (speed * Time.fixedDeltaTime);

        velocity.y -= gravity * Time.fixedDeltaTime;

        rigidbody.linearVelocity = velocity;
    }

    void FixedUpdate()
    {
        Move();
    }
}
