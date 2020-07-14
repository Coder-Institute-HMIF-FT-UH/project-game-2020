using UnityEngine;
using UnityEngine.UI;

public class BatteryController : MonoBehaviour
{
    public Slider batteryLevel;

    [Space(10)] public Image batteryFill;

    public Color currentBatteryColor,
        highBatteryColor,
        mediumBatteryColor,
        lowBatteryColor;

    private void LateUpdate()
    {
        if (batteryLevel.value < 0.3f)
        {
            currentBatteryColor = Color.Lerp(batteryFill.color, lowBatteryColor, Time.deltaTime);
        }
        else if (batteryLevel.value < 0.6f)
        {
            currentBatteryColor = Color.Lerp(batteryFill.color, mediumBatteryColor, Time.deltaTime);
        }
        else
        {
            currentBatteryColor = Color.Lerp(batteryFill.color, highBatteryColor, Time.deltaTime);
        }

        batteryFill.color = currentBatteryColor;
    }
}
