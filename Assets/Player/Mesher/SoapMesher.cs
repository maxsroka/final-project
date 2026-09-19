using System;
using Unity.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SoapMesher : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;
    [SerializeField] SoapInteractor soapInteractor;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float voxelTime;
    [SerializeField] Color baseColor;
    [SerializeField] Color destroyedColor;

    NativeList<float3> vertices;
    NativeList<int> indices;
    NativeList<Color32> colors;

    VoxelArray voxels;
    float3 meshOffset;

    public const float VOXEL_SCALE = 0.2f;

    public VoxelArray GetVoxels()
    {
        return voxels;
    }

    public Vector3 VoxelToLocalPosition(Vector3 voxelPosition)
    {
        return voxelPosition * VOXEL_SCALE - (Vector3)meshOffset + Vector3.one * VOXEL_SCALE / 2;
    }

    public Vector3 VoxelToWorldPosition(Vector3 voxelPosition)
    {
        return transform.TransformPoint(VoxelToLocalPosition(voxelPosition));
    }
    
    void FixedUpdate()
    {
        bool anyVoxelsRemoved = false;
        for (int x = 0; x < voxels.Length.x; x++)
        {
            for (int y = 0; y < voxels.Length.y; y++)
            {
                for (int z = 0; z < voxels.Length.z; z++)
                {
                    var voxel = voxels[x, y, z];
                    if (!voxel.isSolid) continue;

                    var voxelPosition = new Vector3Int(x, y, z);
                    var doesCollide = soapInteractor.CollisionPoints.ContainsKey(voxelPosition);
                    if (!doesCollide) continue;

                    if (voxel.time <= 0)
                    {
                        voxels[x, y, z] = new Voxel();
                        anyVoxelsRemoved = true;
                    }
                    else
                    {
                        voxel.time -= Time.deltaTime;
                        voxels[x, y, z] = voxel;
                    }
                }
            }
        }
        
        MeshSoap(anyVoxelsRemoved);
    }

    void Start()
    {
        voxels = new VoxelArray(8, 4, 16, Allocator.Persistent);
        meshOffset = (float3)voxels.Length * VOXEL_SCALE / 2f;
        
        for (int x = 0; x < voxels.Length.x; x++)
        {
            for (int y = 0; y < voxels.Length.y; y++)
            {
                for (int z = 0; z < voxels.Length.z; z++)
                {
                    var voxel = voxels[x, y, z];
                    if (!voxel.isSolid) continue;
                    Debug.Log(new Vector3(x, y, z));
                }
            }
        }

        for (int x = 0; x < voxels.Length.x; x++)
        {
            for (int y = 0; y < voxels.Length.y; y++)
            {
                for (int z = 0; z < voxels.Length.z; z++)
                {
                    voxels[x, y, z] = new Voxel(true, voxelTime);
                }
            }
        }
        
        MeshSoap(true);
    }

    void OnDestroy()
    {
        voxels.Dispose();
    }

    void MeshSoap(bool updateCollider)
    {
        vertices = new NativeList<float3>(Allocator.Temp);
        indices = new NativeList<int>(Allocator.Temp);
        colors = new NativeList<Color32>(Allocator.Temp);
        
        for (int x = 0; x < voxels.Length.x; x++)
        {
            for (int y = 0; y < voxels.Length.y; y++)
            {
                for (int z = 0; z < voxels.Length.z; z++)
                {
                    MeshVoxel(x, y, z);
                }
            }
        }
        
        var mesh = new Mesh();
        mesh.SetVertices(vertices.AsArray());
        // mesh.indexFormat = IndexFormat.UInt32
        mesh.SetIndices(indices.AsArray(), MeshTopology.Triangles, 0);
        mesh.SetColors(colors.AsArray());
        mesh.RecalculateNormals();
        // mesh.Optimize();
        
        meshFilter.mesh = mesh;

        if (updateCollider)
        {
            meshCollider.sharedMesh = mesh;
        }

        bool areVerticesEmpty = vertices.IsEmpty;
        vertices.Dispose();
        indices.Dispose();
        colors.Dispose();
        
        if (areVerticesEmpty)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void MeshVoxel(int x, int y, int z)
    {
        var position = new int3(x, y, z);
        var voxel = voxels[x, y, z];
        if (!voxel.isSolid) return;
        var color = Color32.Lerp(destroyedColor, baseColor, voxel.time / voxelTime);
        
        // left
        
        MeshFace(
            position,
            direction: new int3(-1, 0, 0),
            indices: new int3x2(new int3(3, 2, 1), new int3(3, 1, 0)),
            vertices: new int3x4(new int3(0, 0, 0), new int3(0, 1, 0), new int3(0, 1, 1), new int3(0, 0, 1)),
            color
        );
        
        // forward
        
        MeshFace(
            position,
            direction: new int3(0, 0, 1),
            indices: new int3x2(new int3(3, 2, 1), new int3(3, 1, 0)),
            vertices: new int3x4(new int3(0, 0, 1), new int3(0, 1, 1), new int3(1, 1, 1), new int3(1, 0, 1)),
            color
        );

        // right

        MeshFace(
            position,
            direction: new int3(1, 0, 0),
            indices: new int3x2(new int3(0, 1, 2), new int3(0, 2, 3)),
            vertices: new int3x4(new int3(1, 0, 0), new int3(1, 1, 0), new int3(1, 1, 1), new int3(1, 0, 1)),
            color
        );
        
        // back
      
        MeshFace(
            position,
            direction: new int3(0, 0, -1),
            indices: new int3x2(new int3(0, 1, 2), new int3(0, 2, 3)),
            vertices: new int3x4(new int3(0, 0, 0), new int3(0, 1, 0), new int3(1, 1, 0), new int3(1, 0, 0)),
            color
        );

        // up
      
        MeshFace(
            position,
            direction: new int3(0, 1, 0),
            indices: new int3x2(new int3(0, 1, 2), new int3(0, 2, 3)),
            vertices: new int3x4(new int3(0, 1, 0), new int3(0, 1, 1), new int3(1, 1, 1), new int3(1, 1, 0)),
            color
        );

        // down
        
        MeshFace(
            position,
            direction: new int3(0, -1, 0), 
            indices: new int3x2(new int3(3, 2, 1), new int3(3, 1, 0)),
            vertices: new int3x4(new int3(0, 0, 0), new int3(0, 0, 1), new int3(1, 0, 1), new int3(1, 0, 0)), 
            color
        );
    }

    void MeshFace(int3 position, int3 direction, int3x2 indices, int3x4 vertices, Color32 color)
    {
        var neighborPosition = position + direction;
        if (IsInBounds(neighborPosition.x, neighborPosition.y, neighborPosition.z) && voxels[neighborPosition.x, neighborPosition.y, neighborPosition.z].isSolid) return;
        
        var verticesLength = this.vertices.Count;
        this.indices.Add(verticesLength + indices.c0[0]);
        this.indices.Add(verticesLength + indices.c0[1]);
        this.indices.Add(verticesLength + indices.c0[2]);
        this.indices.Add(verticesLength + indices.c1[0]);
        this.indices.Add(verticesLength + indices.c1[1]);
        this.indices.Add(verticesLength + indices.c1[2]);

        var vertexOffset = (float3)position * VOXEL_SCALE - meshOffset;
        this.vertices.Add((float3)vertices.c0 * VOXEL_SCALE + vertexOffset);
        this.vertices.Add((float3)vertices.c1 * VOXEL_SCALE + vertexOffset);
        this.vertices.Add((float3)vertices.c2 * VOXEL_SCALE + vertexOffset);
        this.vertices.Add((float3)vertices.c3 * VOXEL_SCALE + vertexOffset);
        
        colors.AddReplicate(color, 4);
    }

    bool IsInBounds(int x, int y, int z)
    {
        return x >= 0 && y >= 0 && z >= 0 
               && x < voxels.Length.x && y < voxels.Length.y && z < voxels.Length.z;
    }
}
