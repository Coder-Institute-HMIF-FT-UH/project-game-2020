using System;
using UnityEngine;
using UnityEngine.UI;

public class BatteryManager : MonoBehaviour
{
    public TimerManager timerManager;

    [SerializeField] private DateMaster dateMaster;
    
    [Header("Battery")] 
    [Range(0, 1)] [SerializeField] private float currentBattery;
    [SerializeField] private float maxBattery = 1;

    [Header("UI Elements")]
    [SerializeField] private Text batteryText;
    [SerializeField] private Text detailBatteryText;
    [SerializeField] private Text oneBatteryTimeRemainingText;
    [SerializeField] private Text fullBatteriesTimeRemainingText;

    private bool IsBatteryFull()
    {
        // If currentBattery >= maxBattery, IsBatteryFull = true
        // Else, IsBatteryFull = false
        return currentBattery >= maxBattery;
    }

    private void Start()
    {
        if(PlayerPrefs.HasKey("currentBattery"))
        {
            // Get current Battery
            currentBattery = PlayerPrefs.GetInt("currentBattery");
        }
    }
}
