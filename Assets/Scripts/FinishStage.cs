using System;
using System.Collections;
using UnityEngine;

public class FinishStage : MonoBehaviour
{
    [SerializeField] private GameObject controller;
    [SerializeField] private CanvasGroup canvasGroup;

    private IEnumerator showFinalPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            showFinalPanel = ShowFinalPanel(0.05f, 0.025f, () =>
            {
                controller.SetActive(false);
            });
            StartCoroutine(showFinalPanel);
        }
    }

    private IEnumerator ShowFinalPanel(float fadeAmount, float fadeSpeed, Action callback = null)
    {
        float targetAlpha = canvasGroup.alpha == 1 ? 0 : 1;
        canvasGroup.interactable = false;
        
        while (true)
        {
            if (targetAlpha == 0)
            {
                if (canvasGroup.alpha > targetAlpha)
                {
                    canvasGroup.alpha -= fadeAmount;

                    // Check if we are done fading
                    if (canvasGroup.alpha <= targetAlpha)
                    {
                        StopCoroutine(showFinalPanel);
                        if (callback != null)
                        {
                            callback();
                        }
                    }
                }
            }
            else if (targetAlpha == 1)
            {
                if (canvasGroup.alpha < targetAlpha)
                {
                    canvasGroup.alpha += fadeAmount;

                    // Check if we are don fading 
                    if (canvasGroup.alpha >= targetAlpha)
                    {
                        canvasGroup.interactable = true;
                        StopCoroutine(showFinalPanel);
                        if (callback != null)
                        {
                            callback();
                        }
                    }
                }
            }
            yield return new WaitForSeconds(fadeSpeed);
        }
    }
}
