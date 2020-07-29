using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    public DialogueQueueData dialogueQueueData;
    
    [SerializeField] private BoxCollider fakeDoor;
    
    // Controller
    [Header("Controller")]
    [SerializeField] private PlayerFPSController playerController;
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject jumpButton;
    [SerializeField] private GameObject gpsButton;
    [SerializeField] private GameObject pickUpButton;
    [SerializeField] private GameObject gpsBackButton;
    [SerializeField] private PickUpObject pickUpObject;
    
    // Tutorial Dialogue
    [Header("Tutorial Dialogue")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private GameObject dialogueData;
    [SerializeField] private bool isTutorialDone = false;
    [SerializeField] private int tutorialIndex = 0;
    private Queue<DialogueUI> dialogueUIs;
    private Queue<DialogueData> tutorials;
    private int tutorialsCount = 0;
    
    private bool isPlayerMoved = false,
        showDialogue = true;
    
    private void Start()
    {
        dialogueUIs = new Queue<DialogueUI>();
        tutorials = new Queue<DialogueData>();
        
        EnqueueAll();
        
        tutorialsCount = tutorials.Count;
        
        // Don't make player use jumpBtn and gpsBtn for the first time
        jumpButton.SetActive(false);
        gpsButton.SetActive(false);
        pickUpButton.SetActive(false);
    }

    private void Update()
    {
        if(!isTutorialDone)
        {
            for (int i = 0; i < tutorialsCount; i++)
            {
                if (i == tutorialIndex)
                {
                    if (showDialogue)
                    {
                        tutorials.Dequeue();
                        dialogueUIs.Dequeue();

                        playerController.enabled = false;

                        dialogueQueueData.tutorials[i].gameObject.SetActive(true); // Activate DialogueData
                        dialogueQueueData.dialogueUis[i].gameObject.SetActive(true); // Activate DialogueUI
                        showDialogue = false;
                    }
                }
                else
                {
                    dialogueQueueData.tutorials[i].gameObject.SetActive(false); // Deactivate DialogueData
                    dialogueQueueData.dialogueUis[i].gameObject.SetActive(false); // Deactivate DialogueUI
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
                        tutorialIndex++;
                        showDialogue = true;
                        // Debug.Log("Tutorial 6 done");
                    }

                    break;
                }
                case 6:
                {
                    Debug.Log("All Tutorials done");
                    isTutorialDone = true;
                    break;
                }
            }
        } 
        else if (isTutorialDone && (dialogueData.activeInHierarchy || dialogueUI.activeInHierarchy))
        {
            Debug.Log("Done");
            MakeControllersNormal();
            Destroy(dialogueData);
            Destroy(dialogueUI);
            Destroy(gameObject);
        }
    }

    private void MakeControllersNormal()
    {
        controller.SetActive(true);
        jumpButton.SetActive(true);
        fakeDoor.enabled = false;
        pickUpButton.SetActive(true);
        gpsButton.SetActive(true);
    }

    private void EnqueueAll()
    {
        foreach (DialogueData tutorial in dialogueQueueData.tutorials)
            tutorials.Enqueue(tutorial);

        foreach (DialogueUI dialogueUi in dialogueQueueData.dialogueUis)
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
