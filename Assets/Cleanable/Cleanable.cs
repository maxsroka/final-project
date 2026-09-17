using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cleanable : MonoBehaviour
{
    [SerializeField] int textureWidth = 512;
    [SerializeField] int textureHeight = 512;
    
    MeshRenderer meshRenderer;
    RenderTexture sourceTexture;
    RenderTexture targetTexture;
    Material material;
    Vector4[] uvs;
    int uvCount;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        sourceTexture = RenderTexture.GetTemporary(new RenderTextureDescriptor(textureWidth, textureHeight, RenderTextureFormat.R8));
        targetTexture = RenderTexture.GetTemporary(new RenderTextureDescriptor(textureWidth, textureHeight, RenderTextureFormat.R8));
        material = new Material(Shader.Find("CleanableBlit"));
        uvs = new Vector4[512];
        meshRenderer.material.SetTexture("_Render_Texture", targetTexture);
    }

    void OnDestroy()
    {
        RenderTexture.ReleaseTemporary(sourceTexture);
        RenderTexture.ReleaseTemporary(targetTexture);
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, textureWidth, textureHeight), targetTexture);
    }

    void Update()
    {
        if (uvCount == 0) return;
        
        material.SetInteger("_TargetUVsCount", uvCount);
        material.SetVectorArray("_TargetUVs", uvs);
        
        Graphics.Blit(sourceTexture, targetTexture, material);
        (sourceTexture, targetTexture) = (targetTexture, sourceTexture);

        uvCount = 0;
    }

    public void OnCollision(Vector2 uv)
    {
        uvs[uvCount] = uv;
        uvCount += 1;
    }
    
    void OnValidate()
    {
        if (!gameObject.CompareTag("Cleanable"))
        {
            Debug.LogWarning($"The object '{name}' has a Cleanable component attached, but its tag isn't set to Cleanable.", this);
        }
    }
}
