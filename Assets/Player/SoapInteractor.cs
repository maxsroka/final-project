using System;
using System.Collections.Generic;
using UnityEngine;

public class SoapInteractor : MonoBehaviour
{
    [SerializeField] SoapMesher soapMesher;
    [SerializeField] LayerMask layerMask;
    
    Collider[] overlapSphereResults;

    public HashSet<Collider> Colliders;
    public Dictionary<Vector3Int, Collider> CollisionPoints;
    
    void Awake()
    {
        overlapSphereResults = new Collider[100];
        Colliders = new HashSet<Collider>();
        CollisionPoints = new Dictionary<Vector3Int, Collider>();
    }
    
    void FixedUpdate()
    {
        Colliders.Clear();
        CollisionPoints.Clear();
        
        var voxels = soapMesher.GetVoxels();
        for (int x = 0; x < voxels.Length.x; x++)
        {
            for (int y = 0; y < voxels.Length.y; y++)
            {
                for (int z = 0; z < voxels.Length.z; z++)
                {
                    var voxel = voxels[x, y, z];
                    if (!voxel.isSolid) continue;

                    var voxelPosition = new Vector3Int(x, y, z);
                    var worldPosition = soapMesher.VoxelToWorldPosition(voxelPosition);
                    var colliderCount = Physics.OverlapSphereNonAlloc(worldPosition, SoapMesher.VOXEL_SCALE * 0.75f, overlapSphereResults, layerMask, QueryTriggerInteraction.Ignore);
                    for (int i = 0; i < colliderCount; i++)
                    {
                        var collider = overlapSphereResults[i];
                        Colliders.Add(collider);
                        CollisionPoints.TryAdd(voxelPosition, collider);
                    }
                }
            }
        }
    }
}
