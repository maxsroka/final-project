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
    [SerializeField] float rotationSpeed;

    bool isGrounded;
    bool doJump;
    
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
        if (!doJump || !isGrounded)
        {
            force.y = isGrounded ? -groundedGravity : -airborneGravity;
        }

        rigidbody.AddForce(force, ForceMode.Acceleration);
        
        isGrounded = false;

        if (doJump)
        {
            var dir = Quaternion.AngleAxis(90f, Vector3.up) * forward.normalized;
            rigidbody.AddTorque(dir * rotationSpeed, ForceMode.VelocityChange);
            doJump = false;
        }
    }

    void Update()
    {
        if (jump.action.WasPerformedThisFrame() && isGrounded)
        {
            doJump = true;
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
