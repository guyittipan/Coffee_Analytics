using UnityEngine;
using Unity.Services.Analytics;

public class AnalyticsSender : MonoBehaviour
{
    public static AnalyticsSender I;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SendGameResult(bool isWin, string rankName)
    {
        CustomEvent gameResultEvent = new CustomEvent("game_result")
        {
            { "is_win", isWin },
            { "rank_name", rankName }
        };

        AnalyticsService.Instance.RecordEvent(gameResultEvent);
        Debug.Log("Sent game_result");
    }
}