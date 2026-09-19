using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoapCleaner : MonoBehaviour
{
    [SerializeField] SoapInteractor soapInteractor;
    [SerializeField] SoapMesher soapMesher;
    [SerializeField] LayerMask layerMask;

    void Update()
    {
        foreach (var (point, collider) in soapInteractor.CollisionPoints)
        {
            var worldPosition = soapMesher.VoxelToWorldPosition(point);
            
            if (collider.CompareTag("Cleanable"))
            {
                // var direction = (worldPosition - transform.position).normalized;
                // if (Physics.Raycast(worldPosition, direction, out var hit, SoapMesher.VOXEL_SCALE, layerMask, QueryTriggerInteraction.Ignore))
                // {
                //     var uv = hit.textureCoord;
                //     var cleanable = collider.GetComponent<Cleanable>();
                //
                //     cleanable.OnCollision(uv);
                // }

                var directions = new Vector3[] { Vector3.up, Vector3.left, Vector3.back, Vector3.right, Vector3.forward, Vector3.down };
                foreach (var direction in directions)
                {
                    if (Physics.Raycast(worldPosition, direction, out var hit, SoapMesher.VOXEL_SCALE, layerMask, QueryTriggerInteraction.Ignore))
                    {
                        var uv = hit.textureCoord;
                        var cleanable = collider.GetComponent<Cleanable>();
                    
                        cleanable.OnCollision(uv);
                        break;
                    }
                }
            }
        }
        
        // if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit))
        // {
        //     if (!hit.transform.CompareTag("Cleanable")) return;
        //     Debug.Log(hit.textureCoord);
        //     
        //     var cleanable = hit.transform.GetComponent<Cleanable>();
        //     cleanable.OnCollision(hit.textureCoord);
        // }
    }
}
