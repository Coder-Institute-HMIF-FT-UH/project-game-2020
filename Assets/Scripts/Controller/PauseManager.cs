using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    // public SceneLoader sceneLoader;
    [SerializeField] private LoadLevelManager loadLevelManager;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Text stageName,
        starsTaken,
        batteryText,
        hintsLeft;

    /// <summary>
    /// Set status in pause screen
    /// </summary>
    public void SetStatus()
    {
        stageName.text = $"Stage {loadLevelManager.LevelName}";
        starsTaken.text = $"Star(s) taken: {PlayerPrefs.GetInt("stars" + loadLevelManager.LevelName)}";
        batteryText.text = $"Battery: {Math.Round(PlayerPrefs.GetFloat("currentBattery") * 100, 0)}%";
        hintsLeft.text = "Hint(s) left: 3";
    }
    
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
