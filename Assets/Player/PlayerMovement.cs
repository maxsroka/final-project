using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody rigidbody;
    [Header("Settings")]
    [SerializeField] float speed = 5f;
    
    Vector3 input;
    
    void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        input = new Vector3(input.x, 0, input.y);
    }

    void Move()
    {
        var force= input * speed;
        rigidbody.AddForce(force);
    }

    void Update()
    {
        Move();
    }
}
