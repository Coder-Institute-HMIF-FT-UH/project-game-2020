using UnityEngine;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    private bool resetTimer = false;

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
    [SerializeField] private Text oneSanityTimeRemainingText;
    [SerializeField] private Text fullSanityTimeRemainingText;

    private void Awake()
    {
        minutes = defaultStartMinutes; // Set minutes
        
        if (PlayerPrefs.HasKey("TimeOnExit"))
        {
            milliseconds = PlayerPrefs.GetFloat("TimeOnExit"); // Get time from TimeOnExit

            minutes = (int) milliseconds / 60; // Get minutes from milliseconds
            milliseconds -= minutes * 60; // Subtract it to get 1-60 seconds

            seconds = (int) milliseconds; // Get the remaining seconds
            milliseconds -= seconds; // Subtract it to get 0-1
            
            PlayerPrefs.DeleteKey("TimeOnExit");
        }
    }

    private void Start()
    {
        // Get currentSanity
        currentSanity = PlayerPrefs.GetInt("currentSanity");
        // Set Text UI
        sanityText.text = currentSanity + " / " + maxSanity;
        detailSanityText.text = currentSanity + " / " + maxSanity;
    }

    private void Update()
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
                    // Add Sanity
                    currentSanity += 1;
                    // Set PlayerPrefs for currentSanity
                    PlayerPrefs.SetInt("currentSanity", currentSanity);
                    // Set Text UI
                    sanityText.text = currentSanity + " / " + maxSanity;
                    detailSanityText.text = currentSanity + " / " + maxSanity;
                    
                    seconds = 59; // Repeat seconds
                    minutes--; // Decrease minutes
                }
            }
            else // If time is up, ...
            {
                // Add extra code here (may also need to add a flag so that this is not called continually)
                resetTimer = allowTimerRestart;
            }
            
            
        }

        // If seconds is not equals with savedSeconds, ...
        if (seconds != savedSeconds)
        {
            // Show current time
            oneSanityTimeRemainingText.text = string.Format("{0} : {1}", minutes, seconds);
            
            if(minutes == 0)
                fullSanityTimeRemainingText.text = string.Format("{0} : {1}", (maxSanity - currentSanity) * (minutes + 1), seconds);
            else
                fullSanityTimeRemainingText.text = string.Format("{0} : {1}", (maxSanity - currentSanity) * minutes, seconds);
            
            savedSeconds = seconds;
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

    private void OnApplicationQuit()
    {
        int numSeconds = minutes * 60 + seconds; // Get all minutes and seconds remaining
        if (numSeconds > 0)
        {
            milliseconds += numSeconds;
            PlayerPrefs.SetFloat("TimeOnExit", milliseconds);
        }
    }
}
