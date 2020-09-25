using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    
    /// <summary>
    /// Pause game
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f;
        // Activate pause screen
        pauseScreen.SetActive(true);
    }
    
    /// <summary>
    /// Resume game
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        // Deactivate pause screen
        pauseScreen.SetActive(false);
    }
}
