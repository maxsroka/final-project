using System;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<SoapController>(out var soapController))
            {
                soapController.ResetPositionAndRotation();
            }
        }
    }
}
