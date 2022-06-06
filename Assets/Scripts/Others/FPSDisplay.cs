using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI FpsText;

    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    private void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if (time >= pollingTime)
        {
            int fpsCount = Mathf.RoundToInt(frameCount / time);
            FpsText.text = fpsCount.ToString() + " FPS";
            time -= pollingTime;
            frameCount = 0;
        }
    }
}
