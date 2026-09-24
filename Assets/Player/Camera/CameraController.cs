using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float sensitivity;
    [SerializeField] Transform player;
    [SerializeField] InputActionReference look;
    
    Vector3 rotation;
    
    void Awake()
    {
        rotation = transform.rotation.eulerAngles;
    }

    void LateUpdate()
    {
        var input = look.action.ReadValue<Vector2>();
        rotation += new Vector3(-input.y, input.x, 0f) * sensitivity;
        rotation.x = Mathf.Clamp(rotation.x, 1f, 89f);
        transform.rotation = Quaternion.Euler(rotation);
        transform.position = player.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0f, 0f, 0.1f);
        Gizmos.DrawSphere(transform.position, Vector3.Distance(transform.position, transform.GetChild(0).position));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Vector3.Distance(transform.position, transform.GetChild(0).position));
    }
}
