using UnityEngine;
using UnityEngine.UI;

public class LoadLevelManager : MonoBehaviour
{
    public LevelDetails levelDetails;
    [SerializeField] private Slider batteryLevel;
    [SerializeField] private WarningUI warningContainer;
    [SerializeField] private WarningScriptableObject lowSanity;
    
    private string levelName;

    public string LevelName => levelName;

    private void Awake()
    {
        levelName = levelDetails.levelName;
        batteryLevel.value = PlayerPrefs.GetFloat(PlayerPrefsConstant.CurrentBattery);
    }
    
    /// <summary>
    /// Restart level
    /// </summary>
    /// <param name="sceneLoader"></param>
    public void RestartLevel(SceneLoader sceneLoader)
    {
        // Check null
        if (!levelDetails) return;
        Time.timeScale = 1f; // Set time to normal
        // Minus sanity
        int currentSanity = PlayerPrefs.GetInt(PlayerPrefsConstant.CurrentSanity);
        
        // If current sanity is greater than requirement, player can play
        if(currentSanity >= levelDetails.sanityRequirement)
        {
            currentSanity -= levelDetails.sanityRequirement;
            PlayerPrefs.SetInt(PlayerPrefsConstant.CurrentSanity, currentSanity);
            Debug.Log("Minus sanity: " + 
                      PlayerPrefs.GetInt(PlayerPrefsConstant.CurrentSanity));

            sceneLoader.LoadScene(levelDetails.sceneName);
        }
        else // If player doesn't have enough sanity, give warning
        {
            SetWarning(lowSanity);
        }
    }
    
    /// <summary>
    /// Set warning
    /// </summary>
    /// <param name="warning"></param>
    private void SetWarning(WarningScriptableObject warning)
    {
        warningContainer.warningScriptableObject = warning;
        warningContainer.gameObject.SetActive(true); 
        warningContainer.SetWarningUI(); // Warning
    }
}
