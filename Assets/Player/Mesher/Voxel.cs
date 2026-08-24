using UnityEngine;

public struct Voxel
{
    public bool isSolid;
    public float time;
    
    public Voxel(bool isSolid, float time)
    {
        this.isSolid = isSolid;
        this.time = time;
    }
}