using TMPro;
using UnityEngine;

public class BuildGUID : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    
    void Start()
    {
        text.SetText($"{Application.buildGUID}");
    }
}
