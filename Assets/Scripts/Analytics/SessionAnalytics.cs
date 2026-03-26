using UnityEngine;
using Unity.Services.Analytics;

public class SessionAnalytics : MonoBehaviour
{
    private float sessionStartTime;

    private void Start()
    {
        sessionStartTime = Time.time;
    }

    private void OnApplicationQuit()
    {
        SendSession();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SendSession();
        }
    }

    private void SendSession()
    {
        float duration = Time.time - sessionStartTime;

        CustomEvent sessionEvent = new CustomEvent("session_end")
        {
            { "session_duration", duration }
        };

        AnalyticsService.Instance.RecordEvent(sessionEvent);

        Debug.Log("Session duration: " + duration);
    }
}