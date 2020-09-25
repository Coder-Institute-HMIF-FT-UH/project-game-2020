using System;
using System.Collections;
using UnityEngine;

public class DateMaster : MonoBehaviour
{
    private DateTime currentDate,
        oldDate;
    private bool isStart = true;
    
    private TimeSpan difference;
    public TimeSpan Difference => difference;
    
    private IEnumerator SetIsStartFalse()
    {
        yield return new WaitForSeconds(.5f);
        isStart = false;
    }
    
    private void Awake()
    {
        GetDate();
        StartCoroutine(SetIsStartFalse());
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetString(PlayerPrefsConstant.DateMasterSysString, DateTime.Now.ToBinary().ToString());
        
            Debug.Log("Saving this date to prefs: " + DateTime.Now);
        }
        else if(!pauseStatus && !isStart)
        {
            GetDate();
        }
    }

    private void OnDisable()
    {
        // Save the current system time as a string
        PlayerPrefs.SetString(PlayerPrefsConstant.DateMasterSysString, DateTime.Now.ToBinary().ToString());
        
        Debug.Log("Saving this date to prefs: " + DateTime.Now);
    }

    private void GetDate()
    {
        // Store the current time when it starts
        currentDate = DateTime.Now;
        
        // Grab the old time from the player prefs as a long
        long temp = Convert.ToInt64(PlayerPrefs.GetString(PlayerPrefsConstant.DateMasterSysString));
        
        // Convert the old time from binary to a DateTime
        DateTime oldDate = DateTime.FromBinary(temp);
        Debug.Log("oldDate: " + oldDate);
        
        // Use the Subtract method and store the result as a timespan variable
        difference = currentDate.Subtract(oldDate);
        Debug.Log("Difference: " + difference);
    }
}
