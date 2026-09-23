using System;
using UnityEngine;

public class Burner : MonoBehaviour
{
    FireHazard fireHazard;
    SoapMesher mesher;
    MeshRenderer meshRenderer;
    ParticleSystem particleSystem;
    bool isBurning;
    bool isPlayerInTrigger;
    Material material;
    
    void Awake()
    {
        fireHazard = GetComponentInParent<FireHazard>();
        meshRenderer = GetComponentInParent<MeshRenderer>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        meshRenderer.materials = new[] { meshRenderer.sharedMaterial, material };
    }

    void Update()
    {
        var emission = particleSystem.emission;
        emission.enabled = isBurning;
    }

    public void EnableBurn()
    {
        isBurning = true;
        material.color = fireHazard.BurnerActiveColor;
    }

    public void DisableBurn()
    {
        isBurning = false;
        material.color = fireHazard.BurnerInactiveColor;
        
        if (isPlayerInTrigger)
        {
            mesher?.SetBurnMultiplier(1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isBurning) return;
        
        if (other.CompareTag("Player"))
        {
            mesher = other.GetComponent<SoapMesher>();
            mesher.SetBurnMultiplier(fireHazard.BurnMultiplier);
            isPlayerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isBurning) return;
        
        if (other.CompareTag("Player"))
        {
            mesher?.SetBurnMultiplier(1f);
            isPlayerInTrigger = false;
        }
    }
}
