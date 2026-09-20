using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [field: SerializeField] public int LevelNumber { get; private set; }
    [field: SerializeField] public string LevelName { get; private set; }
    [field: SerializeField] public Color LevelColor { get; private set; }
    [field: SerializeField] public Cleanable[] Cleanables { get; private set; }
    [field: SerializeField] public float Timer { get; private set; }
    public LevelStatus Status { get; private set; }

    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] string nextLevelName;

    public enum LevelStatus
    {
        Playing,
        Failed,
        Completed
    }
    
    void Update()
    {
        if (Status != LevelStatus.Playing) return;

        Timer += Time.deltaTime;

        if (Cleanables.All(c => c.IsClean))
        {
            OnComplete();
        }
    }

    public void OnFail()
    {
        Status = LevelStatus.Failed;
        ShowUI();
    }

    public void OnComplete()
    {
        Status = LevelStatus.Completed;
        ShowUI();
    }

    public void ShowUI()
    {
        canvas.enabled = true;
        title.color = LevelColor;
        title.SetText(Status == LevelStatus.Completed ? $"Level {LevelNumber}: Complete" : "Time's out: you've melted!");

        var cleanablesLength = Cleanables.Length;
        var time = TimeSpan.FromSeconds(Timer).ToString("mm\\:ss");
        var cleanCount = Cleanables.Count(c => c.IsClean);
        description.SetText($"Objects Cleaned: {cleanCount}/{cleanablesLength}\nTime spent cleaning: {time}");
    }

    public void OnProceed()
    {
        SceneManager.LoadScene(Status == LevelStatus.Completed ? nextLevelName : SceneManager.GetActiveScene().name);
    }

    void OnValidate()
    {
        if (Cleanables.Length == 0)
        {
            Debug.LogWarning("There are no Cleanables assigned to the Level Manager.");
        }
    }
}
