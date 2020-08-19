using UnityEngine;
using UnityEngine.UI;

public class LoadLevelManager : MonoBehaviour
{
    public LevelDetails levelDetails;
    [SerializeField] private Slider batteryLevel;

    private string levelName;

    public string LevelName => levelName;

    private void Awake()
    {
        levelName = levelDetails.levelName;
        batteryLevel.value = PlayerPrefs.GetFloat(PlayerPrefsConstant.CurrentBattery);
    }
}
