using UnityEngine;
using UnityEngine.UI;

public class PasswordManager : MonoBehaviour
{
    [SerializeField] private GameObject linkedDoor; //Put the doors that want to get linked with the UI here 
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

    void Start()
    {
        screenText = passwordScreenText.GetComponent<Text>();

        doorCollider = linkedDoor.GetComponent<Collider>();
        doorTransform = linkedDoor.GetComponent<Transform>();
        doorCloseState = doorTransform.transform.position;
        doorOpenState = doorTransform.transform.position + new Vector3(0, 3);
    }

    public void Update()
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
    public void Number(string input)
    {
        if (password.Length < solution.Length)
        {
            password = password.Insert(password.Length, input);//Adds a character to the password string
            ShowPassword();
        }
    }

    public void Delete()
    {
        if(password.Length > 0)
        {
            password = password.Remove(password.Length - 1);//Removes a character in the password string
            ShowPassword();
        }
    }

    public void Enter()
    {
        if (password == solution)
        {
            isSolved = true;
            passwordPanel.SetActive(false);
        }
    }

    public void Back()
    {
        passwordPanel.SetActive(false);
    }

    private void ShowPassword()
    {
        screenText.text = password;//Changes the text into the current password string
    }
}
