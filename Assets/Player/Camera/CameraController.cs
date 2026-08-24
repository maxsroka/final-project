using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float sensitivity;
    [SerializeField] Transform player;
    [SerializeField] InputActionReference look;
    
    Vector3 rotation;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
}
