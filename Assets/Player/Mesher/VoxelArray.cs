using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public struct VoxelArray : IDisposable
{
    public int3 Length { get; }
    
    NativeArray<Voxel> nativeArray;
    
    public VoxelArray(int x, int y, int z, Allocator allocator)
    {
        Length = new int3(x, y, z);
        nativeArray = new NativeArray<Voxel>(x * y * z, allocator);
    }

    public Voxel this[int x, int y, int z]
    {
        get => nativeArray[x + y * Length.x + z * Length.x * Length.y];
        set => nativeArray[x + y * Length.x + z * Length.x * Length.y] = value;
    }
    
    public NativeArray<Voxel> AsArray() => nativeArray;

    public void Dispose() => nativeArray.Dispose();
}
