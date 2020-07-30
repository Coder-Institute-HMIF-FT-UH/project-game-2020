﻿using UnityEngine;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    [Header("Timer")] 
    [SerializeField] private int minutes = 1;
    [SerializeField] private int seconds = 0;
    [SerializeField] private int savedSeconds;
    [SerializeField] private float milliseconds = 0;

    [Range(1, 59)]
    public int defaultStartMinutes = 1;
    public bool allowTimerRestart = true;

    [Header("Sanity")]
    [SerializeField] private int currentSanity;
    [SerializeField] private int maxSanity = 50;
    
    [Header("UI Elements")] 
    [SerializeField] private Text sanityText;
    [SerializeField] private Text detailSanityText;
    [SerializeField] private Text addSanityText;
    [SerializeField] private Text oneSanityTimeRemainingText;
    [SerializeField] private Text fullSanityTimeRemainingText;
    
    private bool resetTimer;

    private bool IsSanityFull()
    {
        bool isSanityFull;

        if (currentSanity == maxSanity || currentSanity > maxSanity)
            isSanityFull = true;
        else
            isSanityFull = false;

        return isSanityFull;
    }
    
    private DateMaster dateMaster;
    
    private void Start()
    {
        // Get currentSanity
        currentSanity = PlayerPrefs.GetInt("currentSanity");
        dateMaster = GetComponent<DateMaster>(); // Get DateMaster component
        minutes = defaultStartMinutes; // Set minutes
        
        // If currentSanity is not full, ...
        if(!IsSanityFull())
        {
            if (PlayerPrefs.HasKey("TimeOnExit"))
            {
                milliseconds = PlayerPrefs.GetFloat("TimeOnExit"); // Get time from TimeOnExit

                int additionalSanity = 0;
                float difference = milliseconds - (float) dateMaster.Difference.TotalSeconds;

                // Debug.Log("Difference with timer: " + difference);

                while (difference < 0)
                {
                    additionalSanity += 1;
                    difference = minutes * 60 + difference;
                }

                milliseconds = difference;
                currentSanity += additionalSanity; // Add currentSanity with additionalSanity

                if (currentSanity > maxSanity)
                    currentSanity = maxSanity;
                
                PlayerPrefs.SetInt("currentSanity", currentSanity); // Set to playerPrefs

                // If currentSanity is not full, ...
                if(!IsSanityFull())
                {
                    minutes = (int) milliseconds / 60; // Get minutes from milliseconds
                    milliseconds -= minutes * 60; // Subtract it to get 1-60 seconds

                    seconds = (int) milliseconds; // Get the remaining seconds
                    milliseconds -= seconds; // Subtract it to get 0-1
                }
                else // If currentSanity is full, Reset Time
                    ResetTime();

                PlayerPrefs.DeleteKey("TimeOnExit");
            }
        }
        
        UpdateSanityUi(); // Set Text UI
    }

    private void Update()
    {
        if(!IsSanityFull()) // If currentSanity isn't full, ...
        {
            // Count down in seconds
            milliseconds += Time.deltaTime;

            if (resetTimer)
                ResetTime();

            // If milliseconds are greater or equal to 1, ... 
            if (milliseconds >= 1.0f)
            {
                milliseconds -= 1.0f; // Subtract by 1
                // If time is not up, ...
                if (seconds > 0 || minutes > 0)
                {
                    seconds--; // Decrease seconds
                    if (seconds < 0)
                    {
                        seconds = 59; // Repeat seconds
                        minutes--; // Decrease minutes
                    }
                }
                else // If time is up, ...
                {
                    AddSanity(); // Add Sanity
                    // Add extra code here (may also need to add a flag so that this is not called continually)
                    resetTimer = allowTimerRestart;
                }
            }

            // If seconds is not equals with savedSeconds, ...
            if (seconds != savedSeconds)
            {
                UpdateSanityTimeRemaining(); // Update Sanity UI
                savedSeconds = seconds;
            }

            // If currentSanity is not equal to PlayerPrefs, ...
            if (currentSanity != PlayerPrefs.GetInt("currentSanity"))
            {
                // Set new currentSanity to PlayerPrefs
                PlayerPrefs.SetInt("currentSanity", currentSanity);
            }
            UpdateSanityUi(); // Update UI
        }
        else // If currentSanity is full, deactivate UI
        {
            TimeRemainingSanityUi(false);
        }
    }

    /// <summary>
    /// ResetTime: Restart countdown when timer is out
    /// </summary>
    private void ResetTime()
    {
        minutes = defaultStartMinutes;
        seconds = 0;
        savedSeconds = 0;
        milliseconds = 1.0f - Time.deltaTime;
        resetTimer = false;
    }

    /// <summary>
    /// When user quits application
    /// </summary>
    private void OnApplicationQuit()
    {
        int numSeconds = minutes * 60 + seconds; // Get all minutes and seconds remaining
        if (numSeconds > 0)
        {
            milliseconds += numSeconds;
            PlayerPrefs.SetFloat("TimeOnExit", milliseconds);
        }
    }

    /// <summary>
    /// Add 1 sanity
    /// </summary>
    private void AddSanity()
    {
        // Add Sanity
        currentSanity += 1;
        // Set PlayerPrefs for currentSanity
        PlayerPrefs.SetInt("currentSanity", currentSanity);
    }

    /// <summary>
    /// Update Sanity UI when sanity is refilling
    /// </summary>
    private void UpdateSanityUi()
    {
        TimeRemainingSanityUi(true);
        
        sanityText.text = currentSanity + " / " + maxSanity;
        detailSanityText.text = currentSanity + " / " + maxSanity;
        addSanityText.text = currentSanity + " / " + maxSanity;
    }

    /// <summary>
    /// Activate or deactivate Time Remaining Sanity UI
    /// </summary>
    /// <param name="isActivate"></param>
    private void TimeRemainingSanityUi(bool isActivate)
    {
        oneSanityTimeRemainingText.gameObject.SetActive(isActivate);
        fullSanityTimeRemainingText.gameObject.SetActive(isActivate);
    }

    /// <summary>
    /// Update Sanity Time Remaining when sanity is not full
    /// </summary>
    private void UpdateSanityTimeRemaining()
    {
        string fullSanityText;
        
        // Show current time
        oneSanityTimeRemainingText.text = string.Format("1 Sanity in {0} : {1}", minutes, seconds);

        int totalMinutes = (maxSanity - currentSanity) * defaultStartMinutes - (defaultStartMinutes - minutes);
        int hours = totalMinutes / 60;
        
        if(hours > 0)
            fullSanityText = string.Format("Full Sanity in {0} : {1} : {2}", hours, totalMinutes - hours*60, seconds);
        else
            fullSanityText = string.Format("Full Sanity in {0} : {1}", totalMinutes, seconds);
        
        fullSanityTimeRemainingText.text = fullSanityText;
    }

    /// <summary>
    /// Just for testing
    /// </summary>
    public void MinusSanity()
    {
        currentSanity -= 49;
    }
}
