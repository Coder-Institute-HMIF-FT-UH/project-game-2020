using UnityEngine;
using UnityEngine.UI;

public class PasswordManager : MonoBehaviour
{
    [SerializeField] private GameObject passwordScreenText;
    [SerializeField] string solution;
    
    string password = "";
    int n = 0;
    private Text screenText;

    void Start()
    {
        screenText = passwordScreenText.GetComponent<Text>();
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
            screenText.text = "Solved";
        }
    }

    private void ShowPassword()
    {
        screenText.text = password;//Changes the text into the current password string
    }
}
