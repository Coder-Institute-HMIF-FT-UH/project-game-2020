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
        // PlayerPrefs.DeleteKey(PlayerPrefsConstant.CurrentHint);
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
            UpdateHintUi();
        }
    }

    /// <summary>
    /// Update Hint UI
    /// </summary>
    private void UpdateHintUi()
    {
        hintText.text = $"{currentHint} / {maxHint}"; 
    }
    
    /// <summary>
    /// Just for testing
    /// </summary>
    public void MinusHint()
    {
        PlayerPrefs.SetInt(PlayerPrefsConstant.CurrentHint, 1);
    }
}
