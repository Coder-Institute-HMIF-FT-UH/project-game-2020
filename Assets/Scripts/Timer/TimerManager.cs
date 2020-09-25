using System;
using UnityEngine;

[Serializable]
public class TimerManager
{
    [Range(1,59)]
    public int defaultStartMinutes;
    public int hours = 0;
    public int minutes = 1;
    public int seconds = 0;
    public int savedSeconds;
    public float milliseconds;
    public bool allowTimerRestart = true;

    private bool resetTimer;

    public void SetMinutes()
    {
        minutes = defaultStartMinutes;
    }
    
    /// <summary>
    /// Reset to default time
    /// </summary>
    public void ResetTime()
    {
        SetMinutes();
        seconds = 0;
        savedSeconds = 0;
        milliseconds = 1.0f - Time.deltaTime;
        resetTimer = false;
    }

    public void Timer(Action updateTimer)
    {
        milliseconds += Time.deltaTime;

        if (milliseconds >= 1.0f)
        {
            milliseconds -= 1.0f;

            if (seconds >= 0 || minutes >= 0)
            {
                seconds++;
                if (seconds > 59)
                {
                    seconds = 0;
                    minutes++;
                    if (minutes > 59)
                    {
                        minutes = 0;
                        hours++;
                    }
                }
            }
        }
        
        if (seconds != savedSeconds)
        {
            updateTimer();
            savedSeconds = seconds;
        }
    }
    
    /// <summary>
    /// Count time Down 
    /// </summary>
    /// <param name="addSomething"></param>
    /// <param name="updateTimeRemaining"></param>
    public void CountDown(Action addSomething, Action updateTimeRemaining)
    {
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
                addSomething(); // Do Something when time is up
                resetTimer = allowTimerRestart;
            }
        }

        if (seconds != savedSeconds)
        {
            updateTimeRemaining();
            savedSeconds = seconds;
        }
    }

    /// <summary>
    /// Calculate count down when app is closed
    /// </summary>
    /// <param name="dateMaster"></param>
    /// <param name="isFull"></param>
    /// <param name="timeOnExit"></param>
    /// <param name="currentValuePrefs"></param>
    /// <param name="currentValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public int CountDownInBackground(DateMaster dateMaster, Func<bool> isFull, 
        string timeOnExit, string currentValuePrefs, 
        int currentValue, int maxValue)
    {
        SetMinutes();
        
        // If currentValue is not full, ...
        if (!isFull())
        {
            if (PlayerPrefs.HasKey(timeOnExit))
            {
                // Get time from TimeOnExit
                milliseconds = PlayerPrefs.GetFloat(timeOnExit);

                int additionalValue = 0;
                float difference = milliseconds - (float) dateMaster.Difference.TotalSeconds;

                while (difference < 0)
                {
                    additionalValue += 1;
                    difference = minutes * 60 + difference;
                }

                milliseconds = difference;
                currentValue += additionalValue; // Add currentValue with additionalValue

                if (currentValue > maxValue)
                    currentValue = maxValue;
                
                PlayerPrefs.SetInt(currentValuePrefs, currentValue); // Set to playerPrefs
                
                // Check currentValue and maxValue again
                isFull = () => currentValue == maxValue;

                // If currentValue is not full, ...
                if (!isFull())
                {
                    minutes = (int) milliseconds / 60; // Get minutes from milliseconds
                    milliseconds -= minutes * 60; // Subtract it to get 1-60 seconds

                    seconds = (int) milliseconds; // Get the remaining seconds
                    milliseconds -= seconds; // Subtract it to get 0-1
                }
                else // If currentValue is full, ResetTime
                {
                    ResetTime();
                }
                
                PlayerPrefs.DeleteKey(timeOnExit);
            }
        }

        return currentValue;
    }

    public float CountDownInScene(Func<bool> isFull, 
        string timeOnExit, float currentValue)
    {
        SetMinutes();
        
        if (PlayerPrefs.HasKey(timeOnExit))
        {
            milliseconds = PlayerPrefs.GetFloat(timeOnExit);

            if (!isFull())
            {
                minutes = (int) milliseconds / 60; // Get minutes from milliseconds
                milliseconds -= minutes * 60; // Subtract it to get 1-60 seconds

                seconds = (int) milliseconds; // Get the remaining seconds
                milliseconds -= seconds; // Subtract it to get 0-1
            }
            else // If currentValue is full, ResetTime
            {
                ResetTime();
            }
            
            PlayerPrefs.DeleteKey(timeOnExit);
        }

        return currentValue;
    }
}
