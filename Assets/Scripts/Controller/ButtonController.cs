using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject gpsPanel;

    /// <summary>
    /// Open Gps if battery is greater than 0
    /// </summary>
    /// <param name="battery"></param>
    public void OpenGps(BatteryController battery)
    {
        gpsPanel.SetActive(battery.batteryLevel.value > 0);
    }
}
