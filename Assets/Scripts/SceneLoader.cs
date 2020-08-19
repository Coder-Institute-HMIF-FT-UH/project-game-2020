﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Scene currentScene;

    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    /// <summary>
    /// Load sceneName
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // Set time to normal
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Restart Scene
    /// </summary>
    public void RestartScene()
    {
        Time.timeScale = 1f; // Set time to normal
        SceneManager.LoadScene(currentScene.name);
    }
}
