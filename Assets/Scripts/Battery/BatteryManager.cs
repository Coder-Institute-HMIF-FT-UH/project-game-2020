using System;
using UnityEngine;
using UnityEngine.UI;

public class BatteryManager : MonoBehaviour
{
    public TimerManager timerManager;

    [SerializeField] private DateMaster dateMaster;
    
    [Header("Battery")] 
    [SerializeField] private float currentBattery;
    [SerializeField] private float maxBattery = 1;

    [Header("UI Elements")]
    [SerializeField] private Text batteryText;
    [SerializeField] private Text detailBatteryText;
    [SerializeField] private Text oneBatteryTimeRemainingText;
    [SerializeField] private Text fullBatteriesTimeRemainingText;

    private const string CURRENT_BATTERY = "currentBattery";
    private const string TIME_ON_EXIT = "TimeOnExitBattery";
    
    private bool IsBatteryFull()
    {
        // If currentBattery >= maxBattery, IsBatteryFull = true
        // Else, IsBatteryFull = false
        return currentBattery >= maxBattery;
    }

    private void Start()
    {
        if(PlayerPrefs.HasKey(CURRENT_BATTERY))
        {
            // Get current Battery
            Debug.Log("Current prefs: " + PlayerPrefs.GetFloat(CURRENT_BATTERY));
            currentBattery = PlayerPrefs.GetFloat(CURRENT_BATTERY);
            Debug.Log("Current bat: " + currentBattery);
            
            currentBattery = timerManager.CountDownInScene(IsBatteryFull, TIME_ON_EXIT, currentBattery);
            Debug.Log(timerManager.minutes);
            
            PlayerPrefs.SetFloat(CURRENT_BATTERY, currentBattery);
        }
        UpdateBatteryUi(); // Set Text UI
    }

    private void Update()
    {
        if (!IsBatteryFull()) // If currentBattery isn't full, ...
        {
            // Count Down
            timerManager.CountDown(AddBattery, UpdateBatteryTimeRemaining);
            
            // If currentBattery isn't equal to PlayerPrefs, ...
            if (currentBattery != PlayerPrefs.GetFloat(CURRENT_BATTERY))
            {
                Debug.Log("Set prefs");
                // Set new currentBattery to PlayerPrefs
                PlayerPrefs.SetFloat(CURRENT_BATTERY, currentBattery);
            }
            
            UpdateBatteryUi(); // Update UI
        }
        else // If currentBattery is full, deactivate UI
        {
            TimeRemainingBatteryUi(false);
        }
    }
    
    /// <summary>
    /// When user quits application
    /// </summary>
    private void OnApplicationQuit()
    {
        int numSeconds = timerManager.minutes * 60 + timerManager.seconds; // Get all minutes and seconds remaining
        if (numSeconds > 0)
        {
            timerManager.milliseconds += numSeconds;
            PlayerPrefs.SetFloat(TIME_ON_EXIT, timerManager.milliseconds);
        }
    }

    private void AddBattery()
    {
        // Add Battery
        currentBattery += 0.1f;
        // Set PlayerPrefs for currentSanity
        PlayerPrefs.SetFloat(CURRENT_BATTERY, currentBattery);
        Debug.Log(PlayerPrefs.GetFloat(CURRENT_BATTERY));
    }

    private void UpdateBatteryUi()
    {
        TimeRemainingBatteryUi(true);
        
        string text = $"{currentBattery * 100} / {maxBattery * 100}";
        
        batteryText.text = detailBatteryText.text = text;
    }

    private void TimeRemainingBatteryUi(bool isActivate)
    {
        oneBatteryTimeRemainingText.gameObject.SetActive(isActivate);
        fullBatteriesTimeRemainingText.gameObject.SetActive(isActivate);
    }

    private void UpdateBatteryTimeRemaining()
    {
        // Show current time
        oneBatteryTimeRemainingText.text = $"1 Battery in {timerManager.minutes} : {timerManager.seconds}";

        int totalMinutes = (int) ((maxBattery - currentBattery) * timerManager.defaultStartMinutes * 10 -
                                  (timerManager.defaultStartMinutes - timerManager.minutes));
        int hours = totalMinutes / 60;
        
        string fullBatteriesText = hours > 0 ?
            $"Full Batteries in {hours} : {totalMinutes - hours * 60} : " + $"{timerManager.seconds}" : 
            $"Full Batteries in {totalMinutes} : {timerManager.seconds}";

        fullBatteriesTimeRemainingText.text = fullBatteriesText;
    }
}
