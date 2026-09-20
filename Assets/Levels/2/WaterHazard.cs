using System;
using UnityEngine;

public class WaterHazard : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.OnFail();
        }
    }
}
