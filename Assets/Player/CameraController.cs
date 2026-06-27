using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float sensitivity;
    [SerializeField] Transform pivot;
    
    Vector2 input;
    Vector3 rotation;
    
    void OnLook(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rotation = pivot.rotation.eulerAngles;
    }

    void LateUpdate()
    {
        rotation += new Vector3(-input.y, input.x, 0f) * sensitivity;
        rotation.x = Mathf.Clamp(rotation.x, 0, 180f);
        pivot.rotation = Quaternion.Euler(rotation);
        
        pivot.position = transform.position;
    }
}
