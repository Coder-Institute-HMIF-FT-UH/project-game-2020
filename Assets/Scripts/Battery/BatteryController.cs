using UnityEngine;
using UnityEngine.UI;

public class BatteryController : MonoBehaviour
{
    [SerializeField] private float speed = 0.007f;
    [SerializeField] private GameObject gpsPanel;
    
    public Slider batteryLevel;
    [Space(10)] public Image batteryFill;
    public Color currentBatteryColor,
        highBatteryColor,
        mediumBatteryColor,
        lowBatteryColor;

    private void Awake()
    {
        // Get battery prefs
        batteryLevel.value = PlayerPrefs.GetFloat(PlayerPrefsConstant.CurrentBattery);
    }

    private void Update()
    {
        // Decrease the battery
        var value = batteryLevel.value;
        if(value > 0f)
        {
            value -= Time.deltaTime * speed;
            batteryLevel.value = value;
            PlayerPrefs.SetFloat(PlayerPrefsConstant.CurrentBattery, value);
            // Debug.Log("Battery = " + batteryLevel.value);
        }
        else
        {
            gpsPanel.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        // Change battery color
        if (batteryLevel.value < 0.3f)
            currentBatteryColor = Color.Lerp(batteryFill.color, lowBatteryColor, Time.deltaTime);
        else if (batteryLevel.value < 0.6f)
            currentBatteryColor = Color.Lerp(batteryFill.color, mediumBatteryColor, Time.deltaTime);
        else
            currentBatteryColor = Color.Lerp(batteryFill.color, highBatteryColor, Time.deltaTime);

        batteryFill.color = currentBatteryColor;
    }
}
