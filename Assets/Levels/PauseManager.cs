using System;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] SoapController soapController;
    [SerializeField] CameraController cameraController;

    void Awake()
    {
        Resume();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        soapController.enabled = false;
        cameraController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        soapController.enabled = true;
        cameraController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
