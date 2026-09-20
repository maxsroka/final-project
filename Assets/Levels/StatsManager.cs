using System;
using System.Collections;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    float maxFrameRate = float.MinValue;
    float minFrameRate = float.MaxValue;
    float avgFrameRate = 0f;

    bool isMeasuring;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        isMeasuring = true;
    }

    void Update()
    {
        if (!isMeasuring) return;
        
        var frameRate = 1f / Time.unscaledDeltaTime;

        if (frameRate > maxFrameRate)
        {
            maxFrameRate = frameRate;
        }

        if (frameRate < minFrameRate)
        {
            minFrameRate = frameRate;
        }

        avgFrameRate = (avgFrameRate + frameRate) / 2f;
    }

    void OnDestroy()
    {
        if (isMeasuring)
        {
            PlayerPrefs.SetFloat("frame_rate_avg", avgFrameRate);
            PlayerPrefs.SetFloat("frame_rate_max", maxFrameRate);
            PlayerPrefs.SetFloat("frame_rate_min", minFrameRate);
        }
    }
}
