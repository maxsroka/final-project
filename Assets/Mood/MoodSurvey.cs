using System;
using UnityEngine;
using UnityEngine.UI;

public class MoodSurvey : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Slider slider;
    
    public void OnAccept()
    {
        var moodValue = Mathf.Round(slider.value * 1000f) / 1000f;
        PlayerPrefs.SetFloat("last_mood", moodValue);
        
        var moodString = PlayerPrefs.GetString("mood_string", "");
        moodString += " " + moodValue;
        PlayerPrefs.SetString("mood_string", moodString);
        
        Hide();
    }

    void Start()
    {
        Show();
    }

    void Show()
    {
        var lastMood = PlayerPrefs.GetFloat("last_mood", 0f);
        slider.value = lastMood;
        canvas.enabled = true;
    }
    
    void Hide()
    {
        canvas.enabled = false;
    }
}
