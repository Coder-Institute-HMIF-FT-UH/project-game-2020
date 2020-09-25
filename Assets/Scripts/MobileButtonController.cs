using UnityEngine;

public class MobileButtonController : MonoBehaviour
{
    private enum BackTo { Home, Exit }
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameObject exitContainer;
    [SerializeField] private BackTo backTo;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (backTo)
            {
                case BackTo.Home:
                    sceneLoader.gameObject.SetActive(true);
                    sceneLoader.LoadScene("HomeScene");
                    break;
                case BackTo.Exit:
                    exitContainer.SetActive(true);
                    break;
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
