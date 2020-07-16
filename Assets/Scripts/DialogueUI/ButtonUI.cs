using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private List<Button> dialogueButtons = new List<Button>();

    public List<Button> DialogueButtons
    {
        get => dialogueButtons;
        set => dialogueButtons = value;
    }

    public void HideOnClick()
    {
        for (int i = 0; i < dialogueButtons.Count; i++)
        {
            dialogueButtons[i].onClick.AddListener(() =>
            {
                DisplayButtons(false);
            });
        }
    }

    public void DisplayButtons(bool isDisplayed)
    {
        for (int i = 0; i < dialogueButtons.Count; i++)
        {
            dialogueButtons[i].gameObject.SetActive(isDisplayed);
            
            // Removing all listeners on the buttons
            dialogueButtons[i].onClick.RemoveAllListeners();
        }
    }
}
