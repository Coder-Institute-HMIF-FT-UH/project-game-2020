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
    [SerializeField] private DialogueUI[] dialogueUI;
    [SerializeField] private DialogueData[] tutorials;
    [SerializeField] private int tutorialIndex = 0;
    
    private bool isPlayerMoved = false,
        showDialogue = true;
    
    private void Start()
    {
        // Don't make player use jumpBtn and gpsBtn for the first time
        jumpButton.SetActive(false);
        gpsButton.SetActive(false);
    }

    private void Update()
    {
        for (int i = 0; i < tutorials.Length; i++)
        {
            if(i == tutorialIndex)
            {
                tutorials[i].gameObject.SetActive(true); // Activate DialogueData
                if(showDialogue)
                {
                    dialogueUI[i].gameObject.SetActive(true); // Activate DialogueUI
                    showDialogue = false;
                }
            }
            else
            {
                tutorials[i].gameObject.SetActive(false); // Deactivate DialogueData
                dialogueUI[i].gameObject.SetActive(false); // Deactivate DialogueUI
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
                // Debug.Log("All Tutorials done");
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
}
