using System;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    MeshFilter meshFilter;
    Mesh mesh;
    Rigidbody soap;
    
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        var colors = new Color[mesh.vertexCount];
        mesh.colors = colors;
        meshFilter.mesh = mesh;
    }

    void OnCollisionEnter(Collision collision)
    {
        soap = collision.rigidbody;
    }

    void OnCollisionExit(Collision collision)
    {
        soap = null;
    }

    void FixedUpdate()
    {
        if (soap == null) return;
        
        var colors = mesh.colors;
        var pos = transform.InverseTransformPoint(soap.position);

        var minDist = Mathf.Infinity;
        var minIndex = 0;
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            var vertex = mesh.vertices[i];
            var dist = Vector3.SqrMagnitude(pos - vertex);

            if (dist < minDist)
            {
                minDist = dist;
                minIndex = i;
            }
        }

        colors[minIndex] = Color.white;
        
        mesh.colors = colors;
        meshFilter.mesh = mesh;
    }
}
