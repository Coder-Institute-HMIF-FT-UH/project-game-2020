using UnityEngine;
using UnityEngine.UI;

public class InGameTimer : MonoBehaviour
{
    public TimerManager timerManager;
    
    private Text timer;
    private bool isFinished = false;

    public bool IsFinished
    {
        get => isFinished;
        set => isFinished = value;
    }

    private void Start()
    {
        timer = GetComponent<Text>();
    }

    private void Update()
    {
        if (!isFinished)
        {
            timerManager.Timer(UpdateTimerUi);
        }
    }

    /// <summary>
    /// Update Timer UI
    /// </summary>
    private void UpdateTimerUi()
    {
        // Format string
        string hours = timerManager.hours.ToString("00"),
            minutes = timerManager.minutes.ToString("00"),
            seconds = timerManager.seconds.ToString("00");
        
        // Set timerText
        string timerText = timerManager.hours > 0
            ? $"{hours} : {minutes} : {seconds}"
            : $"{minutes} : {seconds}";
        
        timer.text = timerText;
    }
}
