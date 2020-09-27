using UnityEngine;
using UnityEngine.UI;

public class PasswordManager : MonoBehaviour
{
    // Put the doors that want to get linked with the UI here 
    [SerializeField] private GameObject linkedDoor; 
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private GameObject passwordScreenText;
    [SerializeField] string solution;
    
    private string password = "";
    private int n = 0;
    private Text screenText;
    private bool isSolved;
    private Transform doorTransform;
    private Collider doorCollider;
    private Vector3 doorOpenState, doorCloseState;
    private float distanceTraveled = 0f;

    private void Start()
    {
        screenText = passwordScreenText.GetComponent<Text>();

        doorCollider = linkedDoor.GetComponent<Collider>();
        doorTransform = linkedDoor.GetComponent<Transform>();
        doorCloseState = doorTransform.transform.position;
        doorOpenState = doorTransform.transform.position + new Vector3(0, 3);
    }

    private void Update()
    {
        if (isSolved)
        {
            doorCollider.enabled = true;
            if (distanceTraveled < 1)
            {
                doorTransform.transform.position = Vector3.LerpUnclamped(doorCloseState, doorOpenState, distanceTraveled); //Moves the door up
                distanceTraveled += 0.02f;
            }
        }
    }
    
    /// <summary>
    /// Add input number to password
    /// </summary>
    /// <param name="input"></param>
    public void Number(string input)
    {
        if (password.Length < solution.Length)
        {
            // Add a character to the password string
            password = password.Insert(password.Length, input);
            ShowPassword();
        }
    }

    /// <summary>
    /// Delete password
    /// </summary>
    public void Delete()
    {
        if(password.Length > 0)
        {
            // Remove a character in the password string
            password = password.Remove(password.Length - 1);
            ShowPassword();
        }
    }

    /// <summary>
    /// Enter the password and check it
    /// </summary>
    public void Enter()
    {
        if (password == solution)
        {
            isSolved = true;
            passwordPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Show password
    /// </summary>
    private void ShowPassword()
    {
        // Change the text into the current password string
        screenText.text = password;
    }
}
