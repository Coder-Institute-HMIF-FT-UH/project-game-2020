using System;
using UnityEngine;

public class DateMaster : MonoBehaviour
{
    private DateTime currentDate,
        oldDate;

    private TimeSpan difference;
    public TimeSpan Difference => difference;

    private void Awake()
    {
        // Store the current time when it starts
        currentDate = DateTime.Now;
        
        // Grab the old time from the player prefs as a long
        long temp = Convert.ToInt64(PlayerPrefs.GetString("sysString"));
        
        // Convert the old time from binary to a DateTime
        DateTime oldDate = DateTime.FromBinary(temp);
        Debug.Log("oldDate: " + oldDate);
        
        // Use the Subtract method and store the result as a timespan variable
        difference = currentDate.Subtract(oldDate);
        Debug.Log("Difference: " + difference);
    }

    private void OnApplicationQuit()
    {
        // Save the current system time as a string
        PlayerPrefs.SetString("sysString", DateTime.Now.ToBinary().ToString());
        
        Debug.Log("Saving this date to prefs: " + DateTime.Now);
    }
}
