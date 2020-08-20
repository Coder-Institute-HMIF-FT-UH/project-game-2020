using UnityEngine;
using UnityEngine.UI;

public class DetailsManager : MonoBehaviour
{
    public LevelDetails levelDetails;
    
    [SerializeField] private Text levelName;
    [SerializeField] private Text status;
    [SerializeField] private Text bestTime;
    [SerializeField] private Text sanityRequirement;
    [SerializeField] private Sprite starSprite;
    [SerializeField] private Image[] stars;

    private bool seeDetails;

    public bool SeeDetails
    {
        get => seeDetails;
        set => seeDetails = value;
    }

    private void Update()
    {
        if(levelDetails && seeDetails)
        {
            seeDetails = false;
            levelName.text = levelDetails.levelName;
            status.text = "Status: " + (levelDetails.isClear ? "Clear" : "Unclear");

            // Best Time
            bestTime.text = "Best time: " + (levelDetails.bestHours > 0 
                ? $"{levelDetails.bestHours:00} : {levelDetails.bestMinutes:00} : {levelDetails.bestSeconds:00}" 
                : $"{levelDetails.bestMinutes:00} : {levelDetails.bestSeconds:00}");

            // Sanity Requirement
            sanityRequirement.text = $"-{levelDetails.sanityRequirement}";

            // Star
            for (int i = 0; i < levelDetails.starsTaken; i++)
            {
                stars[i].sprite = starSprite;
            }
        }
    }

    public void StartLevel(SceneLoader sceneLoader)
    {
        if (levelDetails)
            sceneLoader.LoadScene(levelDetails.sceneName);
    }
}
