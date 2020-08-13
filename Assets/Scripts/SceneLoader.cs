﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Load sceneName
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Restart Scene
    /// </summary>
    public void RestartScene()
    {
        Time.timeScale = 1f; // Set time to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
