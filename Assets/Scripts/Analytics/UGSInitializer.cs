using System;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class UGSInitializer : MonoBehaviour
{
    private async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("UGS + Analytics ready");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}