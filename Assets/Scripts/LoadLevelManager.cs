using UnityEngine;
using UnityEngine.UI;

public class LoadLevelManager : MonoBehaviour
{
    [SerializeField] private Slider batteryLevel;

    private void Awake()
    {
        batteryLevel.value = PlayerPrefs.GetFloat(PlayerPrefsConstant.CurrentBattery);
    }
}
