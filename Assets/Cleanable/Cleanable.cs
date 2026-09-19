using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshRenderer), typeof(MeshCollider))]
public class Cleanable : MonoBehaviour
{
    [SerializeField] int textureWidth = 512;
    [SerializeField] int textureHeight = 512;
    [SerializeField, Range(0f, 1f)] float cleanThreshold = 0.7f;
    [SerializeField, Range(0f, 0.1f)] float cleaningPrecision = 0.05f;
    [SerializeField] bool drawDebugTexture;
    
    public float CleanLevel { get; set; }
    public bool IsClean => CleanLevel >= 1f;
    
    MeshRenderer meshRenderer;
    RenderTexture sourceTexture;
    RenderTexture targetTexture;
    Material blitMaterial;
    Vector4[] collisionUVs;
    int collisionCount;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        
        var textureDescriptor = new RenderTextureDescriptor(textureWidth, textureHeight, RenderTextureFormat.R8);
        sourceTexture = RenderTexture.GetTemporary(textureDescriptor);
        targetTexture = RenderTexture.GetTemporary(textureDescriptor);
        blitMaterial = new Material(Shader.Find("CleanableBlit"));
        blitMaterial.SetFloat("_Precision", cleaningPrecision);
        meshRenderer.material.SetTexture("_Render_Texture", targetTexture);
        
        collisionUVs = new Vector4[512];
    }

    void OnDestroy()
    {
        RenderTexture.ReleaseTemporary(sourceTexture);
        RenderTexture.ReleaseTemporary(targetTexture);
    }

    void OnGUI()
    {
        if (drawDebugTexture)
        {
            GUI.DrawTexture(new Rect(0, 0, textureWidth, textureHeight), targetTexture);
        }
    }

    void Update()
    {
        if (IsClean) return;
        
        UpdateRenderTexture();
        UpdateCleanLevel();
    }

    public void OnCollision(Vector2 uv)
    {
        if (IsClean) return;
        
        collisionUVs[collisionCount] = uv;
        collisionCount += 1;
    }

    void UpdateRenderTexture()
    {
        if (collisionCount == 0) return;
        
        blitMaterial.SetInteger("_CollisionCount", collisionCount);
        blitMaterial.SetVectorArray("_CollisionUVs", collisionUVs);
        
        Graphics.Blit(sourceTexture, targetTexture, blitMaterial);
        (sourceTexture, targetTexture) = (targetTexture, sourceTexture);
        
        collisionCount = 0;
    }

    void UpdateCleanLevel()
    {
        CleanLevel = Mathf.Max(Mathf.Min(GetAverageWhiteLevel() / cleanThreshold, 1f), CleanLevel);
        
        if (IsClean)
        {
            Graphics.Blit(Texture2D.whiteTexture, targetTexture);
            meshRenderer.material.SetTexture("_Render_Texture", targetTexture);
        }
        
        GameObject.Find("Clean Percentage Text").GetComponent<TextMeshProUGUI>().SetText($"{Mathf.FloorToInt(CleanLevel * 100f)}% clean");
    }

    float GetAverageWhiteLevel()
    {
        var textureDescriptor = new RenderTextureDescriptor(64, 64, RenderTextureFormat.R8);
        var temp = RenderTexture.GetTemporary(textureDescriptor);
        Graphics.Blit(sourceTexture, temp);

        var req = AsyncGPUReadback.Request(temp);
        req.WaitForCompletion();
        
        var data = req.GetData<Color32>();
        float sum = 0;
        int len = data.Length;
        for (int i = 0; i < len; i++)
        {
            sum += data[i][0];
        }
        var avg = sum / len / 255f;

        RenderTexture.ReleaseTemporary(temp);
        return avg;
    }

    void OnValidate()
    {
        if (!gameObject.CompareTag("Cleanable"))
        {
            Debug.LogWarning($"The object '{name}' has a Cleanable component attached, but its tag isn't set to Cleanable.", this);
        }

        if (GetComponent<MeshCollider>().convex)
        {
            Debug.LogWarning($"The object '{name}' has a Cleanable component attached, but its Mesh Collider is convex.", this);
        }

        if (GetComponent<MeshRenderer>() != null && GetComponent<MeshRenderer>().sharedMaterial != null && GetComponent<MeshRenderer>().sharedMaterial.shader.name != "Shader Graphs/Cleanable")
        {
            Debug.LogWarning($"The object '{name}' has a Cleanable component attached, but its material is not using the Cleanable shader.", this);
        }
    }
}
