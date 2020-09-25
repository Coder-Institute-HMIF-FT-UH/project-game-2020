using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public LevelDetails levelDetails;
    [SerializeField] private Text levelName;
    [SerializeField] private DetailsManager detailsManager;
    [SerializeField] private Sprite[] stars;
    [SerializeField] private Image starSides;
    
    private void Awake()
    {
        levelName.text = levelDetails.levelName;
        SetStarSides();
    }

    public void SetDetails()
    {
        detailsManager.levelDetails = levelDetails;
        detailsManager.SeeDetails = true;
        detailsManager.IsDetailsTouch = true;
    }

    private void SetStarSides()
    {
        int starsTaken = PlayerPrefs.GetInt(PlayerPrefsConstant.StarsTaken + levelDetails.levelName);
        
        starSides.sprite = stars[starsTaken];
    }
}
