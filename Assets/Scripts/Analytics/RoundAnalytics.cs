using System;
using UnityEngine;
using Unity.Services.Analytics;

public class RoundAnalytics : MonoBehaviour
{
    public static RoundAnalytics I;

    private float roundStartTime;
    private int todayPlayCount;
    private string savedDateKey = "round_date";
    private string savedCountKey = "round_count";

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

    private void Start()
    {
        ResetDailyCountIfNeeded();
    }

    public void StartRound()
    {
        ResetDailyCountIfNeeded();
        roundStartTime = Time.time;
        Debug.Log("Round started");
    }

    public void EndRound()
    {
        ResetDailyCountIfNeeded();

        float roundDuration = Time.time - roundStartTime;

        todayPlayCount++;
        PlayerPrefs.SetInt(savedCountKey, todayPlayCount);
        PlayerPrefs.Save();

        CustomEvent roundEndEvent = new CustomEvent("round_end")
        {
            { "round_duration", roundDuration },
            { "today_play_count", todayPlayCount }
        };

        AnalyticsService.Instance.RecordEvent(roundEndEvent);

        Debug.Log("Sent round_end");
        Debug.Log("Round duration: " + roundDuration);
        Debug.Log("Today play count: " + todayPlayCount);
    }

    private void ResetDailyCountIfNeeded()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string savedDate = PlayerPrefs.GetString(savedDateKey, "");

        if (savedDate != today)
        {
            PlayerPrefs.SetString(savedDateKey, today);
            PlayerPrefs.SetInt(savedCountKey, 0);
            PlayerPrefs.Save();
        }

        todayPlayCount = PlayerPrefs.GetInt(savedCountKey, 0);
    }
}