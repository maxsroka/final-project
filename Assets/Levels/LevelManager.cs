using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [field: SerializeField] public int LevelNumber { get; private set; }
    [field: SerializeField] public string LevelName { get; private set; }
    [field: SerializeField] public Color LevelColor { get; private set; }
    [field: SerializeField] public Cleanable[] Cleanables { get; private set; }

    void Update()
    {
        bool areAllClean = true;
        foreach (var cleanable in Cleanables)
        {
            if (!cleanable.IsClean)
            {
                areAllClean = false;
                break;
            }
        }

        if (areAllClean)
        {
            Debug.Log("End");
        }
    }
}
