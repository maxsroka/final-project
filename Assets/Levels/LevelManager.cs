using System;
using System.Collections;
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
    [SerializeField] MoodSurvey moodSurvey;
    [SerializeField] PauseManager pauseManager;

    static int attemptCount = 0;

    void Awake()
    {
        SceneManager.LoadScene("Room/Room (Baked Lighting)", LoadSceneMode.Additive);
    }

    IEnumerator Start()
    {
        yield return null;
        if (attemptCount == 0)
        {
            moodSurvey.Show();
            attemptCount++;
        }
    }

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
            StartCoroutine(OnCompleteDelayed());
        }
    }

    public void OnFail()
    {
        attemptCount++;
        Status = LevelStatus.Failed;
        ShowUI();
    }

    public IEnumerator OnCompleteDelayed()
    {
        yield return new WaitForSeconds(1f);
        OnComplete();
    }

    public void OnComplete()
    {
        pauseManager.Pause();
        attemptCount = 0;
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
