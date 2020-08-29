using System;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [SerializeField] private Text hintText;
    [Range(0, 3)] [SerializeField] private int currentHint;
    
    private int maxHint = 3;

    public int MaxHint => maxHint;

    private void Start()
    {
        // Check currentHint prefs
        if(PlayerPrefs.HasKey(PlayerPrefsConstant.CurrentHint))
        {
            currentHint = PlayerPrefs.GetInt(PlayerPrefsConstant.CurrentHint);
        }
        else
        {
            currentHint = 3;
            PlayerPrefs.SetInt(PlayerPrefsConstant.CurrentHint, currentHint);
        }
        
        // Update hintText
        hintText.text = $"{currentHint} / {maxHint}";
    }

    private void Update()
    {
        if (currentHint != PlayerPrefs.GetInt(PlayerPrefsConstant.CurrentHint))
        {
            currentHint = PlayerPrefs.GetInt(PlayerPrefsConstant.CurrentHint);
            // PlayerPrefs.SetFloat(PlayerPrefsConstant.CurrentHint, currentHint);
            UpdateHintUI(currentHint);
        }
    }

    /// <summary>
    /// Update Hint UI
    /// </summary>
    /// <param name="currentHint"></param>
    public void UpdateHintUI(int currentHint)
    {
        hintText.text = $"{currentHint} / {maxHint}"; 
    }
    
    /// <summary>
    /// Just for testing
    /// </summary>
    public void MinusHint()
    {
        currentHint -= 2;
    }
}
