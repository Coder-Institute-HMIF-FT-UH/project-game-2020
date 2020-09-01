using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    
    private Scene currentScene;
    private SceneLoader instance;
    
    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();

        // if (instance == null)
        // {
        //     instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }

    /// <summary>
    /// Load sceneName
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        loadingScreen.SetActive(true);
        Time.timeScale = 1f; // Set time to normal
        StartCoroutine(LoadNewScene(sceneName));
    }

    private IEnumerator LoadNewScene(string sceneName)
    {
        yield return new WaitForSeconds(3f);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            yield return null;
        }
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
