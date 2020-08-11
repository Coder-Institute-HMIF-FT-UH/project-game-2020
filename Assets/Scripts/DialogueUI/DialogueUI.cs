using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;
    public DialogueData DialogueData
    {
        get => dialogueData;
        set => dialogueData = value;
    }

    [SerializeField] private PlayerFPSController playerController;
    
    // UI Elements
    private CanvasGroup canvasGroup;
    [SerializeField] private GameObject controller;
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
        canvasGroup = GetComponent<CanvasGroup>();
        
        // Hiding all buttons on default
        buttonUI.DisplayButtons(false);
        
        // Show to first dialogue line
        displayDialogueUI = DisplayDialogueUI(0.05f, 0.025f, () =>
        {
            dialogueData.GetNextDialogue();
            AdvanceDialogue();
        });
        
        // Deactivate Controller if it's active
        if (controller.activeInHierarchy)
            StartCoroutine(DeactivateController());
                    
        // Activate Dialogue if it's not active
        if(!gameObject.activeInHierarchy)
            gameObject.SetActive(true);

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

                // If dialogue is finished, ...
                if (dialogueData.IsFinished)
                {
                    dialogueText.text = "";
                    characterName.text = "";
                    gameObject.SetActive(false); // Deactivate Dialogue
                    playerController.enabled = true; 
                    controller.SetActive(true); // Activate Controller
                }
                else // If dialogue is not finished ...
                {
                    // Deactivate Controller if it's active
                    if (controller.activeInHierarchy)
                        controller.SetActive(false);
                    
                    // Activate Dialogue if it's not active
                    if(!gameObject.activeInHierarchy)
                        gameObject.SetActive(true);
                    
                    playerController.enabled = false;
                    
                    // Setting our new dialogue
                    AdvanceDialogue();
                }
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

    private IEnumerator DeactivateController()
    {
        yield return new WaitForSeconds(0.5f);
        controller.SetActive(false);
    }

    private IEnumerator ReadDialogue(float typeSpeed)
    {
        skipButton.gameObject.SetActive(false);
        dialogueText.text = "";
        dialogueIsRunning = true;

        foreach (char letter in dialogueData.DialogueLine.ToCharArray())
        {
            dialogueText.text += letter;
            
            yield return new WaitForSeconds(typeSpeed);
        }
        skipButton.gameObject.SetActive(true);
        
        // Check if we need to display options buttons
        if (dialogueData.IsQuestion)
        {
            skipButton.gameObject.SetActive(false);
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
        skipButton.gameObject.SetActive(true);
        
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
                            StopCoroutine(displayDialogueUI);

                            if (callback != null)
                            {
                                callback();
                            }
                        }
                    }

                    break;
                }
                case 1:
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

                    break;
                }
            }

            yield return new WaitForSeconds(fadeSpeed);
        }
    }
}
