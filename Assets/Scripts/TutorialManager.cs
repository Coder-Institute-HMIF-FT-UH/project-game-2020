using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private BoxCollider fakeDoor;
    
    // Controller
    [Header("Controller")]
    [SerializeField] private PlayerFPSController playerController;
    [SerializeField] private GameObject jumpButton;
    [SerializeField] private GameObject gpsButton;
    [SerializeField] private GameObject gpsBackButton;
    [SerializeField] private PickUpObject pickUpObject;
    
    // Tutorial Dialogue
    [Header("Tutorial Dialogue")] 
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueData[] tutorials;
    [SerializeField] private int tutorialIndex = 0;
    
    private bool isPlayerMoved = false,
        showDialogue = true;
    
    private void Start()
    {
        jumpButton.SetActive(false);
        gpsButton.SetActive(false);
    }

    private void Update()
    {
        for (int i = 0; i < tutorials.Length; i++)
        {
            if(i == tutorialIndex)
            {
                tutorials[i].gameObject.SetActive(true);
                dialogueUI.DialogueData = tutorials[i];
            }
            else
                tutorials[i].gameObject.SetActive(false);
        }

        switch (tutorialIndex)
        {
            // Check tutorialsIndex
            case 0:
            {
                // Move the joystick
                if (isPlayerMoved)
                {
                    tutorialIndex++;
                    jumpButton.SetActive(true);
                    Debug.Log("Tutorial 1 done");
                }
                // ChangeDialogueData(tutorialIndex + 1);
                break;
            }
            case 1:
            {
                ActivateDialogue();
                // Jump
                if (EventSystem.current.currentSelectedGameObject == jumpButton)
                {
                    showDialogue = true;
                    tutorialIndex++;
                    Debug.Log("Tutorial 2 done");
                }
                // ChangeDialogueData(tutorialIndex + 1);
                break;
            }
            case 2:
            {
                ActivateDialogue();
                // Pick up object
                if (pickUpObject.IsPickingUp)
                {
                    showDialogue = true;
                    tutorialIndex++;
                    Debug.Log("Tutorial 3 done");
                }
                // ChangeDialogueData(tutorialIndex + 1);
                break;
            }
            case 3:
            {
                ActivateDialogue();
                // Release object
                if (!pickUpObject.IsPickingUp)
                {
                    showDialogue = true;
                    tutorialIndex++;
                    fakeDoor.enabled = false;
                    Debug.Log("Tutorial 4 done");
                }
                // ChangeDialogueData(tutorialIndex + 1);
                break;
            }
            case 4:
            {
                ActivateDialogue();
                // Collect stars
                if (playerController.StarsCount > 0)
                {
                    showDialogue = true;
                    tutorialIndex++;
                    gpsButton.SetActive(true);
                    Debug.Log("Tutorial 5 done");
                }
                // ChangeDialogueData(tutorialIndex + 1);
                break;
            }
            case 5:
            {
                ActivateDialogue();
                // Use GPS
                if (EventSystem.current.currentSelectedGameObject == gpsBackButton)
                {
                    showDialogue = true;
                    tutorialIndex++;
                    Debug.Log("Tutorial 6 done");
                }
                // ChangeDialogueData(tutorialIndex + 1);
                break;
            }
            case 6:
            {
                ActivateDialogue();
                Debug.Log("Tutorial selesai");
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerMoved = true;
        }
    }

    private void ActivateDialogue()
    {
        if (showDialogue)
        {
            showDialogue = false;
            dialogueUI.gameObject.SetActive(true);
        }
    }

    private void ChangeDialogueData(int index)
    {
        if (dialogueUI.DialogueData.IsFinished)
        {
            dialogueUI.DialogueData = tutorials[index];
        }
    }
}
