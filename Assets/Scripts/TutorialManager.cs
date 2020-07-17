using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private BoxCollider fakeDoor;
    
    // Controller
    [Header("Controller")] 
    [SerializeField] private GameObject jumpButton;
    [SerializeField] private GameObject gpsButton;
    
    // Tutorial Dialogue
    [Header("Tutorial Dialogue")]
    [SerializeField] private GameObject[] tutorials;
    
    
    private int tutorialIndex;

    private void Start()
    {
        jumpButton.SetActive(false);
        gpsButton.SetActive(false);
    }

    private void Update()
    {
        for (int i = 0; i < tutorials.Length; i++)
        {
            if(i == tutorialIndex)
                tutorials[i].SetActive(true);
            else
                tutorials[i].SetActive(false);
        }
        
        // Check tutorialsIndex
    }
}
