using System;
using TMPro;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] LevelManager levelManager;

    public void Show(bool isComplete)
    {
        canvas.enabled = true;
        title.color = levelManager.LevelColor;

        if (isComplete)
        {
            title.SetText($"Level {levelManager.LevelNumber}: Complete");
        }
        else
        {
            title.SetText($"Time's out: you've melted!");
        }
        
        var cleanablesLength = levelManager.Cleanables.Length;
        var time = TimeSpan.FromSeconds(levelManager.Timer).ToString("mm\\:ss");
        description.SetText($"Objects Cleaned: {cleanablesLength}/{cleanablesLength}\nTime spent cleaning: {time}");
    }
    
    public void OnProceed()
    {
        canvas.enabled = false;
        levelManager.OnProceed();
    }
}
