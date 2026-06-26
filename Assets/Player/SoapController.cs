

using UnityEngine;
using UnityEngine.InputSystem;

public class SoapController : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Transform camera;
    [SerializeField] float speed = 5f;
        
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
        var force = direction * speed;
        
        rigidbody.AddForce(force);
    }

    void Update()
    {
        Move();
    }
}
