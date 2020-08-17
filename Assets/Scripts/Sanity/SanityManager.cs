using UnityEngine;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    public TimerManager timerManager;

    [SerializeField] private DateMaster dateMaster;
    
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
        // If currentSanity >= maxSanity, IsSanityFull = true
        // Else, IsSanityFull = false
        return currentSanity >= maxSanity;
    }
    
    private void Start()
    {
        if(PlayerPrefs.HasKey(PlayerPrefsConstant.CurrentSanity))
        // Get currentSanity
        currentSanity = PlayerPrefs.GetInt(PlayerPrefsConstant.CurrentSanity);
        
        // Set currentSanity after refilling when app quit
        currentSanity = timerManager.CountDownInBackground(dateMaster, IsSanityFull, 
            PlayerPrefsConstant.TimeOnExitSanity, PlayerPrefsConstant.CurrentSanity, 
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
            if (currentSanity != PlayerPrefs.GetInt(PlayerPrefsConstant.CurrentSanity))
            {
                // Set new currentSanity to PlayerPrefs
                PlayerPrefs.SetInt(PlayerPrefsConstant.CurrentSanity, currentSanity);
            }
            UpdateSanityUi(); // Update UI
        }
        else // If currentSanity is full, deactivate UI
        {
            TimeRemainingSanityUi(false);
        }
    }
    
    /// <summary>
    /// When destroy (move scene, quit)
    /// </summary>
    private void OnDestroy()
    {
        int numSeconds = timerManager.minutes * 60 + timerManager.seconds; // Get all minutes and seconds remaining
        if (numSeconds > 0)
        {
            timerManager.milliseconds += numSeconds;
            PlayerPrefs.SetFloat(PlayerPrefsConstant.TimeOnExitSanity, timerManager.milliseconds);
        }
    }

    /// <summary>
    /// Add 1 sanity
    /// </summary>
    private void AddSanity()
    {
        Debug.Log("Add Sanity");
        // Add Sanity
        currentSanity += 1;
        // Set PlayerPrefs for currentSanity
        PlayerPrefs.SetInt(PlayerPrefsConstant.CurrentSanity, currentSanity);
    }

    /// <summary>
    /// Update Sanity UI when sanity is refilling
    /// </summary>
    private void UpdateSanityUi()
    {
        TimeRemainingSanityUi(true);
        
        sanityText.text = detailSanityText.text = addSanityText.text = $"{currentSanity} / {maxSanity}";
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
        
        string fullSanityText = hours > 0  
            ? $"Full Sanity in {hours} : {totalMinutes - hours * 60} : " + $"{timerManager.seconds}"  
            : $"Full Sanity in {totalMinutes} : {timerManager.seconds}";
        
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
