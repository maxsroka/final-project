using System;
using UnityEngine;
using UnityEngine.UI;

public class MoodSurvey : MonoBehaviour
{
    [SerializeField] PauseManager pauseManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] Canvas canvas;
    [SerializeField] Slider slider;
    [SerializeField] AudioSource clickSource;

    Action callback;
    
    public void OnAccept()
    {
        clickSource.Play();
        
        var moodValue = Mathf.Round(slider.value * 1000f) / 1000f;
        PlayerPrefs.SetFloat("last_mood", moodValue);
        
        var moodString = PlayerPrefs.GetString("mood_string", "");
        moodString += $" {levelManager.LevelNumber}:{moodValue}";
        PlayerPrefs.SetString("mood_string", moodString);
        
        Hide();
    }

    public void Show(Action callback = null)
    {
        this.callback = callback;
        pauseManager.Pause();
        var lastMood = PlayerPrefs.GetFloat("last_mood", 0f);
        slider.value = lastMood;
        canvas.enabled = true;
    }
    
    public void Hide()
    {
        pauseManager.Resume();
        canvas.enabled = false;
        callback?.Invoke();
    }
}
