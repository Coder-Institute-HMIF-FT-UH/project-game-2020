using UnityEngine;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    private bool resetTimer = false;
    private DateMaster dateMaster;

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
    }

    private void Start()
    {
        dateMaster = GetComponent<DateMaster>(); // Get DateMaster component
        
        minutes = defaultStartMinutes; // Set minutes
        
        // Get currentSanity
        currentSanity = PlayerPrefs.GetInt("currentSanity");

        if (PlayerPrefs.HasKey("TimeOnExit"))
        {
            milliseconds = PlayerPrefs.GetFloat("TimeOnExit"); // Get time from TimeOnExit

            // TODO: Fix this 
            int additionalSanity = 0;
            float difference = milliseconds - (float) dateMaster.Difference.TotalSeconds;

            Debug.Log("Difference with timer: " + difference);
            
            if (difference < 0)
            {
                additionalSanity += 1;
                difference = minutes * 60 + difference;
                
                while(difference < 0)
                {
                    difference = minutes * 60 + difference;
                    additionalSanity += 1;
                }
                milliseconds = difference;
            } 
            else if (difference >= 0)
            {
                milliseconds -= difference;
            }
            
            Debug.Log("Last Sanity: " + currentSanity);
            Debug.Log("Additional Sanity: " + additionalSanity);
            currentSanity += additionalSanity;
            PlayerPrefs.SetInt("currentSanity", currentSanity);
            Debug.Log("CurrentSanity: " + currentSanity);
            
            minutes = (int) milliseconds / 60; // Get minutes from milliseconds
            milliseconds -= minutes * 60; // Subtract it to get 1-60 seconds

            seconds = (int) milliseconds; // Get the remaining seconds
            milliseconds -= seconds; // Subtract it to get 0-1
            
            PlayerPrefs.DeleteKey("TimeOnExit");
        }
        
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
            UpdateUISanity(); // Update Sanity UI
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

    private void AddSanity()
    {
        // Add Sanity
        currentSanity += 1;
        // Set PlayerPrefs for currentSanity
        PlayerPrefs.SetInt("currentSanity", currentSanity);
        
        // Set Text UI
        sanityText.text = currentSanity + " / " + maxSanity;
        detailSanityText.text = currentSanity + " / " + maxSanity;
    }

    private void UpdateUISanity()
    {
        string fullSanityText;
        
        // Show current time
        oneSanityTimeRemainingText.text = string.Format("{0} : {1}", minutes, seconds);

        // int totalSeconds = minutes * 60 + seconds;
        // int sanityDiff = maxSanity - currentSanity;
        // int fullSanityTime = sanityDiff * totalSeconds;
        //
        // int fullSanityMinutes = fullSanityTime / 60;
        // fullSanityTime -= fullSanityMinutes * 60;
        //
        // int fullSanitySeconds = fullSanityTime;
        //
        // fullSanityText = string.Format("{0} : {1}", fullSanityMinutes, fullSanitySeconds);
        
        if(minutes == 0) // bug in here
            fullSanityText = string.Format("{0} : {1}", (maxSanity - currentSanity) * (minutes + defaultStartMinutes), seconds);
        else
            fullSanityText = string.Format("{0} : {1}", (maxSanity - currentSanity) * (minutes + 1), seconds);

        fullSanityTimeRemainingText.text = fullSanityText;
    }
}
