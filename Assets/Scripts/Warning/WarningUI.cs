using UnityEngine;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    public WarningScriptableObject warningScriptableObject;
    [SerializeField] private Image itemHolder;
    [SerializeField] private Text lowItemText;

    public void SetWarningUI()
    {
        if (warningScriptableObject)
        {
            itemHolder.sprite = warningScriptableObject.itemHolder;
            lowItemText.text = warningScriptableObject.lowItemText;
        }
    }
}
