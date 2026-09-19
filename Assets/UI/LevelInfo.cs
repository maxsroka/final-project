using System;
using TMPro;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] TextMeshProUGUI levelCleanables;

    void Start()
    {
        levelName.SetText($"Level {levelManager.LevelNumber}: {levelManager.LevelName}");
    }

    void Update()
    {
        UpdateCleanablesText();
    }

    void UpdateCleanablesText()
    {
        var text = "Clean the following:\n";

        foreach (var cleanable in levelManager.Cleanables)
        {
            var percentage = $"({Mathf.FloorToInt(cleanable.CleanLevel * 100f)}%)";
            if (!cleanable.IsClean)
            {
                text += $"- {cleanable.name} {percentage}\n";
            }
            else
            {
                text += $"- <s>{cleanable.name}</s> {percentage}\n";
            }
        }
        
        levelCleanables.SetText(text);
    }
}
