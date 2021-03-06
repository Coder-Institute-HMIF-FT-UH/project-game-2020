﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishStage : MonoBehaviour
{
    [SerializeField] private LoadLevelManager loadLevelManager;
    [SerializeField] private InGameTimer inGameTimer;
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject finishUiContainer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Sprite starSprite;
    [SerializeField] private Image[] starsUis;
    [SerializeField] private Animator[] starAnimators;
    [SerializeField] private Text coinValue,
        bestTimeText,
        currentTimeText;
    [SerializeField] private PlayerFPSController playerController;
    [SerializeField] private List<StarScript> starScripts;
    
    private IEnumerator showFinalPanel;
    private bool isDoneFading, isFinished = true;
    private int maxCoin;

    private void Awake()
    {
        maxCoin = loadLevelManager.levelDetails.maxCoin;
        // PlayerPrefs.DeleteKey(PlayerPrefsConstant.StarsTaken + loadLevelManager.LevelName);
        // // Set star is taken prefs
        // foreach (StarScript starScript in starScripts)
        // {
        //     // Catch error if star is already taken
        //     PlayerPrefs.DeleteKey("is" + starScript.name + loadLevelManager.LevelName);
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        inGameTimer.IsFinished = true;
        
        PlayerPrefs.SetInt(PlayerPrefsConstant.IsStageClear + loadLevelManager.LevelName, 1);
        
        // Set star prefs
        PlayerPrefs.SetInt(PlayerPrefsConstant.StarsTaken + loadLevelManager.LevelName,
            playerController.StarsCount);
        // Set star is taken prefs
        foreach (StarScript starScript in starScripts)
        {
            // Catch error if star is already taken
            try
            {
                PlayerPrefs.SetInt("is" + starScript.name + loadLevelManager.LevelName,
                    Convert.ToInt32(starScript.IsTaken));
            }
            catch (MissingReferenceException) { }
            catch (NullReferenceException) { }
        }
        
        finishUiContainer.SetActive(true);
        
        // TIME
        // Get total seconds after finish level
        // CURRENT TIME
        int totalSeconds = inGameTimer.timerManager.hours * 3600
                           + inGameTimer.timerManager.minutes * 6
                           + inGameTimer.timerManager.seconds;
        SetTimeText(currentTimeText,
            inGameTimer.timerManager.hours,
            inGameTimer.timerManager.minutes,
            inGameTimer.timerManager.seconds);
        Debug.Log("Total seconds: " + totalSeconds);
        
        // BEST TIME
        int bestTime = PlayerPrefs.GetInt(PlayerPrefsConstant.BestTime + loadLevelManager.LevelName);
        if(PlayerPrefs.HasKey(PlayerPrefsConstant.BestTime + loadLevelManager.LevelName))
        {
            // Update prefs if total seconds are less than previous one.
            if (totalSeconds < bestTime)
            {
                Debug.Log("Set new record");
                SetTimeText(bestTimeText,
                    inGameTimer.timerManager.hours,
                    inGameTimer.timerManager.minutes,
                    inGameTimer.timerManager.seconds);
                SetTime(totalSeconds);
            }
            else
            {
                int hours = bestTime / 3600;
                bestTime -= hours * 3600;
                int minutes = bestTime / 60;
                bestTime -= minutes * 60;
                int seconds = bestTime;
                
                SetTimeText(bestTimeText, hours, minutes, seconds);
            }
        }
        else
        {
            SetTime(totalSeconds);
            SetTimeText(bestTimeText,
                inGameTimer.timerManager.hours,
                inGameTimer.timerManager.minutes,
                inGameTimer.timerManager.seconds);
        }

        // Show Final Panel slowly
        showFinalPanel = ShowFinalPanel(0.05f, 0.025f, () =>
        {
            controller.SetActive(false);
        });
        StartCoroutine(showFinalPanel);

        // COINS
        // Check coins prefs
        long currentCoins = 0;
        if (PlayerPrefs.HasKey(PlayerPrefsConstant.Coins))
        {
            // Convert string to long
            currentCoins = Convert.ToInt64(
                PlayerPrefs.GetString(PlayerPrefsConstant.Coins));
        }
        // Save long prefs to string
        PlayerPrefs.SetString(PlayerPrefsConstant.Coins, (currentCoins + maxCoin).ToString());
        Debug.Log("Current Coin: " + PlayerPrefs.GetString(PlayerPrefsConstant.Coins));
        
        // STARS
        // Set star UI sprites
        switch (PlayerPrefs.GetInt("stars" + loadLevelManager.LevelName))
        {
            case 1:
                SetStarSprite(1);
                break;
            case 2:
                SetStarSprite(2);
                break;
            case 3:
                SetStarSprite(3);
                break;
        }
    }

    /// <summary>
    /// Set time in prefs and asset
    /// </summary>
    /// <param name="totalSeconds"></param>
    private void SetTime(int totalSeconds)
    {
        // Set prefs
        PlayerPrefs.SetInt(PlayerPrefsConstant.BestTime + loadLevelManager.LevelName, totalSeconds);
    }
    
    /// <summary>
    /// Set time text in finish UI
    /// </summary>
    /// <param name="timeText"></param>
    /// <param name="hours"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    private void SetTimeText(Text timeText, int hours, int minutes, int seconds)
    {
        timeText.text = $"{hours:00}.{minutes:00}.{seconds:00}";
    }

    private void OnTriggerStay(Collider other)
    {
        // If other is Player and isDoneFading and isFinished, ...
        if (!other.CompareTag("Player") || !isDoneFading || !isFinished) return;
        StartCoroutine(StarAnimation()); // Play animation 
        StartCoroutine(CoinAnimation(0.005f));
        isFinished = false; // Set isFinished to false
    }

    /// <summary>
    /// Set Star UI's Sprite
    /// </summary>
    /// <param name="index"></param>
    private void SetStarSprite(int index)
    {
        for (int i = 0; i < index; i++)
            starsUis[i].sprite = starSprite;
    }

    /// <summary>
    /// Coin Animation
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator CoinAnimation(float waitTime)
    {
        // Debug.Log("Coin animation");
        for(int i = 0; i <= maxCoin; i+=5)
        {
            coinValue.text = i.ToString();
            yield return new WaitForSeconds(waitTime);
        }
    }
    
    /// <summary>
    /// Set Active star UI to play animation.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StarAnimation()
    {
        // Debug.Log("Star animation");
        for (int i = 0; i < 3; i++)
        {
            starAnimators[i].gameObject.SetActive(true);
            // Wait animator length to start another animator
            yield return new WaitForSeconds(starAnimators[i].GetCurrentAnimatorStateInfo(0).length);
        }
    }

    /// <summary>
    /// Show Final Panel slowly
    /// </summary>
    /// <param name="fadeAmount"></param>
    /// <param name="fadeSpeed"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator ShowFinalPanel(float fadeAmount, float fadeSpeed, Action callback = null)
    {
        float targetAlpha = canvasGroup.alpha == 1 ? 0 : 1;
        canvasGroup.interactable = false;
        
        while (true)
        {
            switch (targetAlpha)
            {
                case 0:
                {
                    if (canvasGroup.alpha > targetAlpha)
                    {
                        canvasGroup.alpha -= fadeAmount;

                        // Check if we are done fading
                        if (canvasGroup.alpha <= targetAlpha)
                        {
                            StopCoroutine(showFinalPanel);
                            callback?.Invoke();
                        }
                    }

                    break;
                }
                case 1:
                {
                    if (canvasGroup.alpha < targetAlpha)
                    {
                        canvasGroup.alpha += fadeAmount;

                        // Check if we are done fading 
                        if (canvasGroup.alpha >= targetAlpha)
                        {
                            if (canvasGroup.alpha == targetAlpha)
                                isDoneFading = true;
                            canvasGroup.interactable = true;
                            StopCoroutine(showFinalPanel);
                            callback?.Invoke();
                        }
                    }

                    break;
                }
            }
            
            yield return new WaitForSeconds(fadeSpeed);
        }
    }
}
