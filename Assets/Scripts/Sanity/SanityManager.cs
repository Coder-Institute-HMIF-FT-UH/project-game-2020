using UnityEngine;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    public TimerManager timerManager;

    [Header("Sanity")]
    [SerializeField] private int currentSanity;
    [SerializeField] private int maxSanity = 50;
    
    [Header("UI Elements")] 
    [SerializeField] private Text sanityText;
    [SerializeField] private Text detailSanityText;
    [SerializeField] private Text addSanityText;
    [SerializeField] private Text oneSanityTimeRemainingText;
    [SerializeField] private Text fullSanityTimeRemainingText;

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
        // Get DateMaster component
        dateMaster = GetComponent<DateMaster>(); 
        
        // Set currentSanity after refilling when app quit
        currentSanity = timerManager.CountDownInBackground(dateMaster, IsSanityFull, 
            "TimeOnExit", "currentSanity", 
            currentSanity, maxSanity);
        
        UpdateSanityUi(); // Set Text UI
    }

    private void Update()
    {
        if(!IsSanityFull()) // If currentSanity isn't full, ...
        {
            // Count down 
            timerManager.CountDown(AddSanity, UpdateSanityTimeRemaining);

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
    /// When user quits application
    /// </summary>
    private void OnApplicationQuit()
    {
        int numSeconds = timerManager.minutes * 60 + timerManager.seconds; // Get all minutes and seconds remaining
        if (numSeconds > 0)
        {
            timerManager.milliseconds += numSeconds;
            PlayerPrefs.SetFloat("TimeOnExit", timerManager.milliseconds);
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
        // Show current time
        oneSanityTimeRemainingText.text = $"1 Sanity in {timerManager.minutes} : {timerManager.seconds}";

        int totalMinutes = (maxSanity - currentSanity) * timerManager.defaultStartMinutes - 
                           (timerManager.defaultStartMinutes - timerManager.minutes);
        int hours = totalMinutes / 60;
        
        string fullSanityText = hours > 0 ? 
            $"Full Sanity in {hours} : {totalMinutes - hours * 60} : " +
            $"{timerManager.seconds}" : $"Full Sanity in {totalMinutes} : {timerManager.seconds}";
        
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
