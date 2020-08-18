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
        // Set timerText
        string timerText = timerManager.hours > 0
            ? $"{timerManager.hours:00} : {timerManager.minutes:00} : {timerManager.seconds:00}"
            : $"{timerManager.minutes:00} : {timerManager.seconds:00}";
        
        timer.text = timerText;
    }
}
