using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    public GameObject batteryDetails,
        shopPanel;

    public Button batteryText;

    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject == batteryText.gameObject)
            batteryDetails.SetActive(false);
    }

    /// <summary>
    /// ShowBatteryDetails: Show battery details UI when battery text is clicked
    /// </summary>
    public void ShowBatteryDetails()
    {
        // Activate batteryDetails when it is not active
        // Deactivate batteryDetails when it is active
        batteryDetails.SetActive(!batteryDetails.activeInHierarchy);
    }

    /// <summary>
    /// ShowAddBattery: Show Shop when Add Sanity Button is clicked
    /// </summary>
    public void ShowAddBattery()
    {
        if(batteryDetails.activeInHierarchy)
            batteryDetails.SetActive(false); // Deactivate battery Details
        
        // Activate shopPanel
        shopPanel.SetActive(true);
    }
}
