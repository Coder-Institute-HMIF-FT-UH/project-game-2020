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
    
    private bool IsBatteryFull()
    {
        // If currentBattery >= maxBattery, IsBatteryFull = true
        // Else, IsBatteryFull = false
        return currentBattery >= maxBattery;
    }

    private void Start()
    {
        if(PlayerPrefs.HasKey(PlayerPrefsConstant.CurrentBattery))
        {
            // Get current Battery
            currentBattery = PlayerPrefs.GetFloat(PlayerPrefsConstant.CurrentBattery);
            
            currentBattery = timerManager.CountDownInScene(IsBatteryFull, PlayerPrefsConstant.TimeOnExitBattery, currentBattery);
        }
        else
        {
            // If first time, then currentBattery equals to maxBattery
            currentBattery = maxBattery;
        }
        
        PlayerPrefs.SetFloat(PlayerPrefsConstant.CurrentBattery, currentBattery);
        UpdateBatteryUi(); // Set Text UI
    }

    private void Update()
    {
        if (!IsBatteryFull()) // If currentBattery isn't full, ...
        {
            // Count Down
            timerManager.CountDown(AddBattery, UpdateBatteryTimeRemaining);
            
            // If currentBattery isn't equal to PlayerPrefs, ...
            if (currentBattery != PlayerPrefs.GetFloat(PlayerPrefsConstant.CurrentBattery))
            {
                currentBattery = PlayerPrefs.GetFloat(PlayerPrefsConstant.CurrentBattery);
                // Set new currentBattery to PlayerPrefs
                // Debug.Log("Change");
                // PlayerPrefs.SetFloat(PlayerPrefsConstant.CurrentBattery, currentBattery);
            }
            
            UpdateBatteryUi(); // Update UI
        }
        else // If currentBattery is full, deactivate UI
        {
            TimeRemainingBatteryUi(false);
        }
    }
    
    /// <summary>
    /// When destroy (move scene, quit)
    /// </summary>
    // private void OnDestroy()
    // {
    //     int numSeconds = timerManager.minutes * 60 + timerManager.seconds; // Get all minutes and seconds remaining
    //     if (numSeconds > 0)
    //     {
    //         timerManager.milliseconds += numSeconds;
    //         PlayerPrefs.SetFloat(PlayerPrefsConstant.TimeOnExitBattery, timerManager.milliseconds);
    //     }
    // }

    private void OnDisable()
    {
        Debug.Log(gameObject.name + " disable");
        int numSeconds = timerManager.minutes * 60 + timerManager.seconds; // Get all minutes and seconds remaining
        if (numSeconds > 0)
        {
            timerManager.milliseconds += numSeconds;
            PlayerPrefs.SetFloat(PlayerPrefsConstant.TimeOnExitBattery, timerManager.milliseconds);
        }
    }

    private void AddBattery()
    {
        Debug.Log("Add Battery");
        // Add Battery
        currentBattery += 0.1f;
        // If current battery are greater than max battery, ...
        if (currentBattery > maxBattery)
            currentBattery = maxBattery; // Set currentBattery to maxBattery
        
        // Set PlayerPrefs for currentSanity
        PlayerPrefs.SetFloat(PlayerPrefsConstant.CurrentBattery, currentBattery);
    }

    private void UpdateBatteryUi()
    {
        TimeRemainingBatteryUi(true);
        
        string text = $"{Math.Round(currentBattery * 100, 0)} / {maxBattery * 100}";
        
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
        oneBatteryTimeRemainingText.text = $"1 Battery in {timerManager.minutes} : {timerManager.seconds:00}";

        int totalMinutes = (int) ((maxBattery - currentBattery) * timerManager.defaultStartMinutes * 10 -
                                  (timerManager.defaultStartMinutes - timerManager.minutes));
        int hours = totalMinutes / 60;
        
        string fullBatteriesText = hours > 0
            ? $"Full Batteries in {hours} : {totalMinutes - hours * 60:00} : " + $"{timerManager.seconds:00}"
            : $"Full Batteries in {totalMinutes} : {timerManager.seconds:00}";

        fullBatteriesTimeRemainingText.text = fullBatteriesText;
    }
    
    /// <summary>
    /// Just for testing
    /// </summary>
    public void MinusBattery()
    {
        currentBattery -= 0.5f;
        PlayerPrefs.SetFloat(PlayerPrefsConstant.CurrentBattery, currentBattery);
    }
}
