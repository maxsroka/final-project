using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] AudioSource clickSource;
    
    void Start()
    {
        SceneManager.LoadScene("Room/Room (Baked Lighting)", LoadSceneMode.Additive);
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OnQuit()
    {
        clickSource.Play();
        Application.Quit();
    }

    public void OnCopySurveyData()
    {
        clickSource.Play();
        
        var moodString = PlayerPrefs.GetString("mood_string", "");

        var avgFrameRate = Mathf.RoundToInt(PlayerPrefs.GetFloat("frame_rate_avg"));
        var minFrameRate = Mathf.RoundToInt(PlayerPrefs.GetFloat("frame_rate_min"));
        var maxFrameRate = Mathf.RoundToInt(PlayerPrefs.GetFloat("frame_rate_max"));
        
        GUIUtility.systemCopyBuffer = $"level_number:mood_value{moodString} | avg_fps:{avgFrameRate} min_fps:{minFrameRate} max_fps:{maxFrameRate}";
    }
}
