using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject pickUpButton;
    [SerializeField] private GameObject gpsBackButton;
    [SerializeField] private PickUpObject pickUpObject;
    
    // Tutorial Dialogue
    [Header("Tutorial Dialogue")]
    [SerializeField] private int tutorialIndex = 0;
    private Queue<DialogueUI> dialogueUIs;
    private Queue<DialogueData> tutorials;
    
    private bool isPlayerMoved = false,
        showDialogue = true;
    
    private void Start()
    {
        dialogueUIs = new Queue<DialogueUI>();
        tutorials = new Queue<DialogueData>();
        
        EnqueueAll();
        
        // Don't make player use jumpBtn and gpsBtn for the first time
        jumpButton.SetActive(false);
        gpsButton.SetActive(false);
        pickUpButton.SetActive(false);
    }

    private void Update()
    {
        for (int i = 0; i < tutorials.Count; i++)
        {
            if(i == tutorialIndex)
            {
                tutorials.Dequeue();
                // tutorials[i].gameObject.SetActive(true); // Activate DialogueData
                if(showDialogue)
                {
                    dialogueUIs.Dequeue();
                    // dialogueUIs[i].gameObject.SetActive(true); // Activate DialogueUI
                    showDialogue = false;
                }
            }
            else
            {
                // tutorials[i].gameObject.SetActive(false); // Deactivate DialogueData
                // dialogueUIs[i].gameObject.SetActive(false); // Deactivate DialogueUI
            }
        }

        switch (tutorialIndex)
        {
            // Check tutorialsIndex
            case 0:
            {
                // Move the joystick
                if (isPlayerMoved)
                {
                    // Destroy(dialogueUI[tutorialIndex].gameObject);
                    tutorialIndex++;
                    showDialogue = true;
                    jumpButton.SetActive(true);
                    // Debug.Log("Tutorial 1 done");
                }
                break;
            }
            case 1:
            {
                // Jump
                if (EventSystem.current.currentSelectedGameObject == jumpButton)
                {
                    // Destroy(dialogueUI[tutorialIndex].gameObject);
                    tutorialIndex++;
                    showDialogue = true;
                    fakeDoor.enabled = false;
                    pickUpButton.SetActive(true);
                    // Debug.Log("Tutorial 2 done");
                }
                break;
            }
            case 2:
            {
                // Pick up object
                if (pickUpObject.IsPickingUp)
                {
                    // Destroy(dialogueUI[tutorialIndex].gameObject);
                    tutorialIndex++;
                    showDialogue = true;
                    // Debug.Log("Tutorial 3 done");
                }
                break;
            }
            case 3:
            {
                // Release object
                if (!pickUpObject.IsPickingUp)
                {
                    // Destroy(dialogueUI[tutorialIndex].gameObject);
                    tutorialIndex++;
                    showDialogue = true;
                    // Debug.Log("Tutorial 4 done");
                }
                break;
            }
            case 4:
            {
                // Collect stars
                if (playerController.StarsCount > 0)
                {
                    // Destroy(dialogueUI[tutorialIndex].gameObject);
                    tutorialIndex++;
                    showDialogue = true;
                    gpsButton.SetActive(true);
                    // Debug.Log("Tutorial 5 done");
                }
                break;
            }
            case 5:
            {
                // Use GPS
                if (EventSystem.current.currentSelectedGameObject == gpsBackButton)
                {
                    // Destroy(dialogueUI[tutorialIndex].gameObject);
                    tutorialIndex++;
                    showDialogue = true;
                    // Debug.Log("Tutorial 6 done");
                }
                break;
            }
            case 6:
            {
                // Debug.Log("All Tutorials done");
                // Destroy(dialogueUI[tutorialIndex].transform.parent);
                break;
            }
        }
    }

    private void EnqueueAll()
    {
        foreach (DialogueData tutorial in tutorials)
            tutorials.Enqueue(tutorial);

        foreach (DialogueUI dialogueUi in dialogueUIs)
            dialogueUIs.Enqueue(dialogueUi);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerMoved = true;
        }
    }
}
