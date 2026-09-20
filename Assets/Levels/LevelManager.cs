using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [field: SerializeField] public int LevelNumber { get; private set; }
    [field: SerializeField] public string LevelName { get; private set; }
    [field: SerializeField] public Color LevelColor { get; private set; }
    [field: SerializeField] public Cleanable[] Cleanables { get; private set; }
    [field: SerializeField] public float Timer { get; private set; }
    [SerializeField] LevelEnd levelEnd;
    [SerializeField] string nextLevelName;

    bool isComplete;
    
    void Update()
    {
        if (isComplete) return;

        Timer += Time.deltaTime;
        
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
            levelEnd.Show(true);
            isComplete = true;
        }
    }

    public void OnTimeOut()
    {
        levelEnd.Show(false);
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnProceed()
    {
        if (isComplete)
        {
            LoadNextLevel();
        }
        else
        {
            ReloadLevel();
        }
    }

    void OnValidate()
    {
        if (Cleanables.Length == 0)
        {
            Debug.LogWarning("There are no Cleanables assigned to the Level Manager.");
        }
    }
}
