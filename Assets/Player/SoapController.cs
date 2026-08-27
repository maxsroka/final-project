using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoapController : MonoBehaviour
{
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Transform camera;
    [SerializeField] float speed;
    [SerializeField] float airborneGravity;
    [SerializeField] float groundedGravity;
    [SerializeField] float flipAngularForce;
    [SerializeField] float flipLinearForce;

    bool isGrounded;
    
    void Start()
    {
        InputSystem.actions.Enable();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        var input = move.action.ReadValue<Vector2>();
        
        var forward = camera.forward;
        forward.y = 0;

        var right = camera.right;
        right.y = 0;

        var direction = (input.x * right + input.y * forward).normalized;
        var force = direction * speed;
        force.y = isGrounded ? -groundedGravity : -airborneGravity;

        rigidbody.AddForce(force, ForceMode.Acceleration);
        
        isGrounded = false;
    }

    void Flip()
    {
        var forward = camera.forward;
        forward.y = 0;
        var rotationDirection = Quaternion.AngleAxis(90f, Vector3.up) * forward.normalized;
        rigidbody.AddTorque(rotationDirection * flipAngularForce, ForceMode.VelocityChange);
        rigidbody.AddForce(Vector3.up * flipLinearForce, ForceMode.VelocityChange);
    }

    void Update()
    {
        if (jump.action.WasPerformedThisFrame() && isGrounded)
        {
            Flip();
        }
    }

    void OnCollisionStay(Collision other)
    {
        isGrounded = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(rigidbody.position, rigidbody.position + rigidbody.linearVelocity);
    }
}
