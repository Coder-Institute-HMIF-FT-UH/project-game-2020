using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DialogueData : MonoBehaviour
{
    [SerializeField] private string dialogueStoryName,
        xmlPath;
    
    private XmlLoader xmlLoader = new XmlLoader();
    private XmlNodeList dialogueData;

    private bool isFinished;
    public bool IsFinished => isFinished;

    private bool isQuestion;
    public bool IsQuestion => isQuestion;

    private bool isLocked;
    public bool IsLocked => isLocked;
    
    // XML Data
    private int dialogueId = 0,
        destinationId = 0;
    
    private string dialogueType;
    
    private string dialogueName;
    public string DialogueName => dialogueName;

    private string dialogueMood;
    public string DialogueMood => dialogueMood;

    private string dialogueLine;
    public string DialogueLine => dialogueLine;

    private void Awake()
    {
        dialogueData = xmlLoader.LoadXmlData(xmlPath);
        dialogueData = dialogueData[1].SelectSingleNode(dialogueStoryName).SelectNodes(DialogueConstants.Dialogue);
    }

    public void GetNextDialogue()
    {
        try
        {
            isFinished = false;
            // Getting dialogueId and parsing it to an int value
            string stringDialogueId = dialogueData[dialogueId]
                .SelectSingleNode(DialogueConstants.XmlDialogueId)
                .Value;
            dialogueId = int.Parse(stringDialogueId);
            Debug.Log("DialogueID: " + dialogueId);

            // Getting dialogueType
            string stringDialogueType = dialogueData[dialogueId]
                .SelectSingleNode(DialogueConstants.XmlDialogueType)
                .Value;
            dialogueType = stringDialogueType;
            Debug.Log("Type: " + stringDialogueType);

            // Getting portrait of the character
            string stringDialoguePortrait = dialogueData[dialogueId]
                .SelectSingleNode(DialogueConstants.Line)
                .SelectSingleNode(DialogueConstants.XmlDialoguePortrait)
                .Value;
            dialogueName = stringDialoguePortrait;
            Debug.Log("Character: " + dialogueName);

            // Getting mood
            string stringDialogueMood = dialogueData[dialogueId]
                .SelectSingleNode(DialogueConstants.Line)
                .SelectSingleNode(DialogueConstants.XmlDialogueMood)
                .Value;
            dialogueMood = stringDialogueMood;
            Debug.Log("Mood: " + dialogueMood);

            // Getting the next dialogueID if it's not a question and parsing it to an int value
            if (dialogueType == DialogueConstants.Normal)
            {
                string stringDestinationId = dialogueData[dialogueId].FirstChild
                    .SelectSingleNode(DialogueConstants.XmlDialogueDestination)
                    .Value;
                destinationId = int.Parse(stringDestinationId);
                Debug.Log("DestinationID: " + destinationId);
                isQuestion = false;
            }
            else if (dialogueType == DialogueConstants.Question)
            {
                int choiceAmount = dialogueData[dialogueId]
                    .SelectSingleNode(DialogueConstants.Options)
                    .ChildNodes.Count;
                for (int i = 0; i < choiceAmount; i++)
                {
                    Debug.Log("Option: " + dialogueData[dialogueId]
                        .SelectSingleNode(DialogueConstants.Options)
                        .SelectNodes(DialogueConstants.Option)[i]
                        .InnerText);
                    Debug.Log("Choice: " + dialogueData[dialogueId]
                        .SelectSingleNode(DialogueConstants.Options)
                        .SelectNodes(DialogueConstants.Option)[i]
                        .SelectSingleNode(DialogueConstants.XmlDialogueChoice)
                        .Value);
                }

                isQuestion = true;
                isLocked = true;
            }

            // Setting the dialogue line
            dialogueLine = dialogueData[dialogueId]
                .SelectSingleNode(DialogueConstants.Line)
                .InnerText;

            Debug.Log(dialogueLine);

            dialogueId = destinationId;

            Debug.Log("===========================");
        }
        catch (NullReferenceException)
        {
            isFinished = true;
        }
    }

    public List<Button> SetButtons(List<Button> buttons, Action callback)
    {
        // Get the amount of options we have
        int choiceAmount = dialogueData[dialogueId]
            .SelectSingleNode(DialogueConstants.Options)
            .ChildNodes
            .Count;

        for (int i = 0; i < choiceAmount; i++)
        {
            // Set the button text
            string buttonText = dialogueData[dialogueId]
                .SelectSingleNode(DialogueConstants.Options)
                .SelectNodes(DialogueConstants.Options)[i]
                .SelectSingleNode(DialogueConstants.XmlDialogueChoice)
                .Value;

            buttons[i].gameObject.GetComponentInChildren<Text>().text = buttonText;
            
                // Display the correct buttons
                buttons[i].gameObject.SetActive(true);

                XmlNode buttonNode = dialogueData[dialogueId]
                    .SelectSingleNode(DialogueConstants.Options)
                    .SelectNodes(DialogueConstants.Option)[i];
                buttons[i].onClick.AddListener(() =>
                {
                    SetButtonDestination(buttonNode);
                    callback();
                });
        }
        
        // After setting the buttons make sure the next line won't be a question
        isQuestion = false;
        
        return buttons;
    }

    private void SetButtonDestination(XmlNode node)
    {
        string stringNextDialogueId = node
            .SelectSingleNode(DialogueConstants.XmlDialogueDestination)
            .Value;

        dialogueId = int.Parse(stringNextDialogueId);
        dialogueType = node.SelectSingleNode(DialogueConstants.XmlDialogueType).Value;
        dialogueLine = node.InnerText;
        dialogueName = node.SelectSingleNode(DialogueConstants.XmlDialoguePortrait).Value;
        dialogueMood = node.SelectSingleNode(DialogueConstants.XmlDialogueMood).Value;
        
        // Lock the dialogue
        isLocked = false;
    }
}
