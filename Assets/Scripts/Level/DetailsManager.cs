using UnityEngine;
using UnityEngine.UI;

public class DetailsManager : MonoBehaviour
{
    public LevelDetails levelDetails;
    
    [SerializeField] private Text levelName;
    [SerializeField] private Text status;
    [SerializeField] private Text bestTime;
    [SerializeField] private Text sanityRequirement;
    [SerializeField] private Sprite[] starSprites;
    [SerializeField] private Image[] stars;

    private bool seeDetails;
    private int hours, minutes, seconds;

    public bool SeeDetails
    {
        get => seeDetails;
        set => seeDetails = value;
    }

    private void Update()
    {
        if(levelDetails && seeDetails)
        {
            seeDetails = false;
            levelName.text = levelDetails.levelName;
            
            // Status
            status.text = PlayerPrefs.HasKey(PlayerPrefsConstant.IsStageClear + levelDetails.levelName) 
                ? "Status: Clear" // If there is prefs, then it's clear 
                : "Status: Unclear"; // else, it's unclear

            // Best Time
            if (PlayerPrefs.HasKey(PlayerPrefsConstant.BestTime + levelDetails.levelName))
            {
                seconds = PlayerPrefs.GetInt(PlayerPrefsConstant.BestTime + levelDetails.levelName);
                SetBestTime();
                bestTime.text = "Best time: " + (hours > 0
                    ? $"{hours:00} : {minutes:00} : {seconds:00}"
                    : $"{minutes:00} : {seconds:00}");
            }
            else
            {
                bestTime.text = "Best time: -";
            }

            // Sanity Requirement
            sanityRequirement.text = $"-{levelDetails.sanityRequirement}";

            // Star
            if(PlayerPrefs.HasKey(PlayerPrefsConstant.StarsTaken + levelDetails.levelName))
            {
                for (int i = 0; i < PlayerPrefs.GetInt(PlayerPrefsConstant.StarsTaken + levelDetails.levelName); i++)
                {
                    stars[i].sprite = starSprites[1];
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    stars[i].sprite = starSprites[0];
                }
            }
        }
    }

    private void SetBestTime()
    {
        // Hours
        hours = seconds / 3600;
        seconds -= hours * 3600;
        
        // Minutes
        minutes = seconds / 60;
        // Seconds
        seconds -= minutes * 60;
    }

    public void StartLevel(SceneLoader sceneLoader)
    {
        if (levelDetails)
            sceneLoader.LoadScene(levelDetails.sceneName);
    }
}
