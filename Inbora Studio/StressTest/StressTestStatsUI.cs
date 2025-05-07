using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class StressTestStatsUI : MonoBehaviour
{
    public TextMeshProUGUI statsText;

    private float deltaTime = 0.0f;
    private int frameCount = 0;
    private float timePassed = 0.0f;
    private float fps = 0.0f;

    // For average, min, and max FPS over 10 seconds
    private Queue<float> frameTimes = new Queue<float>();
    private List<float> fpsHistory = new List<float>();
    private float frameTimeSum = 0f;
    private float avgFps = 0f;
    private float minFps = float.MaxValue;
    private float maxFps = float.MinValue;
    private float avgWindow = 10f;

    void Update()
    {
        // FPS Calculation
        deltaTime += Time.unscaledDeltaTime;
        frameCount++;
        timePassed += Time.unscaledDeltaTime;
        if (timePassed > 0.5f)
        {
            fps = frameCount / timePassed;
            frameCount = 0;
            timePassed = 0;
        }

        // Average, min, and max FPS over 10 seconds
        float thisFrame = Time.unscaledDeltaTime;
        float thisFps = 1f / thisFrame;
        frameTimes.Enqueue(thisFrame);
        fpsHistory.Add(thisFps);
        frameTimeSum += thisFrame;
        while (frameTimeSum > avgWindow && frameTimes.Count > 0)
        {
            frameTimeSum -= frameTimes.Dequeue();
            fpsHistory.RemoveAt(0);
        }
        if (frameTimeSum > 0 && fpsHistory.Count > 0)
        {
            avgFps = fpsHistory.Count / frameTimeSum;
            minFps = float.MaxValue;
            maxFps = float.MinValue;
            foreach (var f in fpsHistory)
            {
                if (f < minFps) minFps = f;
                if (f > maxFps) maxFps = f;
            }
        }
        else
        {
            avgFps = 0f;
            minFps = 0f;
            maxFps = 0f;
        }

        // Build stats string with colors
        string stats = $"<color=#FF6F61>FPS:</color> <b>{fps:F1}</b>\n" +
                       $"<color=#FFD561>Avg FPS (10s):</color> <b>{avgFps:F1}</b>\n" +
                       $"<color=#61FFB2>Min FPS (10s):</color> <b>{minFps:F1}</b>\n" +
                       $"<color=#FF616B>Max FPS (10s):</color> <b>{maxFps:F1}</b>\n" +
                       $"<color=#6BFF61>Resolution:</color> <b>{Screen.width}x{Screen.height}</b>\n" +
                       $"<color=#61B2FF>GPU:</color> <b>{SystemInfo.graphicsDeviceName}</b>\n" +
                       $"<color=#FF61E7>CPU:</color> <b>{SystemInfo.processorType}</b>";

        if (statsText)
            statsText.text = stats;
    }
} 