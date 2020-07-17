﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;
    
    // UI Elements
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Text dialogueText,
        characterName;
    [SerializeField] private ButtonUI buttonUI;
    [SerializeField] private Image skipButton;
    [SerializeField] private string characterPortraitName;
    
    // IEnumerator
    private IEnumerator readDialogue,
        displayDialogueUI;
    
    // Boolean
    private bool dialogueIsRunning = false,
        dialogueDisplayed = false;

    private void Start()
    {
        // Hiding all buttons on default
        buttonUI.DisplayButtons(false);
        
        // Show to first dialogue line
        displayDialogueUI = DisplayDialogueUI(0.05f, 0.025f, () =>
        {
            dialogueData.GetNextDialogue();
            AdvanceDialogue();
        });

        StartCoroutine(displayDialogueUI);
    }

    private void Update()
    {
        // Set the next dialogue
        if (Input.GetMouseButtonDown(0))
        {
            if (!dialogueData.IsLocked && !dialogueIsRunning && dialogueDisplayed)
            {
                // Fetching the data of the next dialogue
                dialogueData.GetNextDialogue();
                
                // Setting our new dialogue
                AdvanceDialogue();
            }
            else if (dialogueIsRunning)
            {
                SkipDialogue();
            }
        }
    }

    public void AdvanceDialogue()
    {
        characterName.text = dialogueData.DialogueName;
        
        // Set the coroutine
        readDialogue = ReadDialogue(DialogueConstants.DialogueReadSpeed);
        StartCoroutine(readDialogue);
    }

    private IEnumerator ReadDialogue(float typeSpeed)
    {
        skipButton.enabled = false;
        dialogueText.text = "";
        dialogueIsRunning = true;

        foreach (char letter in dialogueData.DialogueLine.ToCharArray())
        {
            dialogueText.text += letter;
            
            yield return new WaitForSeconds(typeSpeed);
        }
        skipButton.enabled = true;
        
        // Check if we need to display options buttons
        if (dialogueData.IsQuestion)
        {
            skipButton.enabled = false;
            buttonUI.DialogueButtons = dialogueData.SetButtons(buttonUI.DialogueButtons, AdvanceDialogue);
            buttonUI.HideOnClick();
        }

        dialogueIsRunning = false;
        StopCoroutine(readDialogue);
    }

    private void SkipDialogue()
    {
        StopCoroutine(readDialogue);
        dialogueText.text = dialogueData.DialogueLine;
        dialogueIsRunning = false;
        skipButton.enabled = true;
        
        // Check if we need to display options buttons.
        if (dialogueData.IsQuestion)
        {
            buttonUI.DialogueButtons = dialogueData.SetButtons(buttonUI.DialogueButtons, AdvanceDialogue);
            buttonUI.HideOnClick();
        }
    }

    /// <summary>
    /// Showing and hiding the whole dialogue.Showing and hiding the whole dialogue.
    /// </summary>
    /// <param name="fadeAmount"></param>
    /// <param name="fadeSpeed"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator DisplayDialogueUI(float fadeAmount, float fadeSpeed, Action callback = null)
    {
        float targetAlpha = canvasGroup.alpha == 1 ? 0 : 1;
        canvasGroup.interactable = false;
        dialogueDisplayed = false;

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
                        StopCoroutine(displayDialogueUI);

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
                        dialogueDisplayed = true;
                        canvasGroup.interactable = true;
                        StopCoroutine(displayDialogueUI);

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