using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("Room/Room (Baked Lighting)", LoadSceneMode.Additive);
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OnChooseLevel()
    {
        
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnCopySurveyData()
    {
        var moodString = PlayerPrefs.GetString("mood_string", "");
        GUIUtility.systemCopyBuffer = $"level_number:mood_value{moodString}";
    }
}
