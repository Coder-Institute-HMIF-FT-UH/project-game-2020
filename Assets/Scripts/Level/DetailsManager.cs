﻿using UnityEngine;
using UnityEngine.UI;

public class DetailsManager : MonoBehaviour
{
    public LevelDetails levelDetails;

    [SerializeField] private GameObject stageHolder;
    [SerializeField] private Text levelName;
    [SerializeField] private Text status;
    [SerializeField] private Text bestTime;
    [SerializeField] private Text sanityRequirement;
    [SerializeField] private Sprite[] starSprites;
    [SerializeField] private Image[] stars;
    [SerializeField] private WarningUI warningContainer;
    [SerializeField] private WarningScriptableObject lowSanity;

    private Animator detailsAnimator;
    private bool seeDetails, 
        isDetailsTouch;
    private int hours, minutes, seconds;

    public bool IsDetailsTouch
    {
        get => isDetailsTouch;
        set => isDetailsTouch = value;
    }

    public bool SeeDetails
    {
        get => seeDetails;
        set => seeDetails = value;
    }

    private void Start()
    {
        detailsAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(levelDetails && seeDetails)
        {
            seeDetails = false;
            stageHolder.SetActive(false); // Deactivate stage Holder 
            detailsAnimator.SetBool("isAppear", true); // Show details
            
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

        // If player touch objects other than detailsContainer, ...
        if (!isDetailsTouch)
        {
            stageHolder.SetActive(true); // Deactivate stage Holder
            // Hide details
            detailsAnimator.SetBool("isAppear", false); 
        }
    }

    /// <summary>
    /// Set best time on details UI
    /// </summary>
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

    /// <summary>
    /// Start level
    /// </summary>
    /// <param name="sceneLoader"></param>
    public void StartLevel(SceneLoader sceneLoader)
    {
        // Check null
        if (!levelDetails) return;
        // Minus sanity
        int currentSanity = PlayerPrefs.GetInt(PlayerPrefsConstant.CurrentSanity);
        
        // If current sanity is greater than requirement, player can play
        if(currentSanity >= levelDetails.sanityRequirement)
        {
            currentSanity -= levelDetails.sanityRequirement;
            PlayerPrefs.SetInt(PlayerPrefsConstant.CurrentSanity, currentSanity);
            Debug.Log("Minus sanity: " + PlayerPrefs.GetInt(PlayerPrefsConstant.CurrentSanity));

            sceneLoader.LoadScene(levelDetails.sceneName);
        }
        else // If player doesn't have enough sanity, give warning
        {
            sceneLoader.gameObject.SetActive(false);
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
