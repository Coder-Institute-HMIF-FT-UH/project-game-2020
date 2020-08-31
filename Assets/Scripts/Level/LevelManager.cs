using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public LevelDetails levelDetails;
    [SerializeField] private Text levelName;
    [SerializeField] private DetailsManager detailsManager;

    private void Awake()
    {
        levelName.text = levelDetails.levelName;
    }

    public void SetDetails()
    {
        detailsManager.levelDetails = levelDetails;
        detailsManager.SeeDetails = true;
        detailsManager.IsDetailsTouch = true;
    }
}
