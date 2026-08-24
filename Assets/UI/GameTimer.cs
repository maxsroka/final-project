using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    
    void Update()
    {
        text.SetText($"{Mathf.Round(Time.realtimeSinceStartup)}s");
    }
}
