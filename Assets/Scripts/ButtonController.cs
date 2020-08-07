using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject gps,
        pauseScreen;
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        // Activate pause screen
        pauseScreen.SetActive(true);
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        // Deactivate pause screen
        pauseScreen.SetActive(false);
    }
}
