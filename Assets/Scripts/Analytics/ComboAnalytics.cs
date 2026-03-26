using UnityEngine;
using Unity.Services.Analytics;

public class ComboAnalytics : MonoBehaviour
{
    public static ComboAnalytics I;

    private int comboCountThisRound = 0;

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

    public void StartRound()
    {
        comboCountThisRound = 0;
        Debug.Log("ComboAnalytics: Start Round");
    }

    public void RecordCombo(string comboName)
    {
        comboCountThisRound++;

        CustomEvent comboEvent = new CustomEvent("combo_used")
        {
            { "combo_name", comboName }
        };

        AnalyticsService.Instance.RecordEvent(comboEvent);

        Debug.Log("Sent combo_used: " + comboName);
    }

    public void EndRound()
    {
        CustomEvent summaryEvent = new CustomEvent("round_combo_summary")
        {
            { "combo_count", comboCountThisRound }
        };

        AnalyticsService.Instance.RecordEvent(summaryEvent);

        Debug.Log("Sent round_combo_summary: " + comboCountThisRound);
    }
}