using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public SceneLoader sceneLoader;
    
    [SerializeField] private GameObject pauseScreen;

    [SerializeField] private Text stageName,
        starsTaken,
        batteryText,
        hintsLeft;

    public void SetStatus()
    {
        stageName.text = "Stage 1-1";
        starsTaken.text = $"Star(s) taken: {PlayerPrefs.GetInt("stars" + sceneLoader.CurrentScene.name)}";
        batteryText.text = $"Battery: {Math.Round(PlayerPrefs.GetFloat("currentBattery") * 100, 0)}%";
        hintsLeft.text = "Hint(s) left: 3";
    }
    
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
