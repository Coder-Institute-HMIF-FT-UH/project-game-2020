using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinishStage : MonoBehaviour
{
    public SceneLoader sceneLoader;
    [SerializeField] private GameObject controller;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Sprite starSprite;
    [SerializeField] private Image[] starsUis;
    [SerializeField] private Animator[] starAnimators;

    private PlayerFPSController playerController;
    private IEnumerator showFinalPanel;
    private bool isDoneFading, isFinished = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show Final Panel slowly
            showFinalPanel = ShowFinalPanel(0.05f, 0.025f, () =>
            {
                controller.SetActive(false);
            });
            StartCoroutine(showFinalPanel);

            // Set star UI sprites
            switch (PlayerPrefs.GetInt("stars" + sceneLoader.CurrentScene.name))
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
    }

    private void OnTriggerStay(Collider other)
    {
        // If other is Player and isDoneFading and isFinished, ...
        if (!other.CompareTag("Player") || !isDoneFading || !isFinished) return;
        StartCoroutine(StarAnimation()); // Play animation 
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
    /// Set Active star UI to play animation.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StarAnimation()
    {
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
