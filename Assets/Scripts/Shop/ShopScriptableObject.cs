using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Shop")]
public class ShopScriptableObject : ScriptableObject
{
    public enum PrefsType
    {
        SetFloat,
        SetInt
    }
    
    [Tooltip("Name of the item")]
    public string itemName;
    
    [Tooltip("Warning scriptable object for each item")]
    public WarningScriptableObject[] warning;

    [Tooltip("Key name for each item prefs")]
    public string[] itemPrefsName;
    
    [Tooltip("Select which type of prefs")]
    public PrefsType[] prefsType;
    
    [Tooltip("The price of item")]
    public int price;
    
    [Tooltip("The maximum amount of each items")]
    public int[] maxItem;
    
    [Tooltip("Additional amount for each items")]
    public float[] additionalItem;
    
    [Tooltip("Logo for confirmation")]
    public Sprite itemLogo;
    
    [Tooltip("Individual additional item logo")]
    public Sprite[] additionalItemLogo;
    
    [Tooltip("Sprite for banner in shop")]
    public Sprite itemBanner;
}
