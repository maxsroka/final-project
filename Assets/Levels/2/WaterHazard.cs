using System;
using UnityEngine;

public class WaterHazard : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] AudioSource waterSource;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waterSource.Play();
            levelManager.OnFail();
        }
    }
}
