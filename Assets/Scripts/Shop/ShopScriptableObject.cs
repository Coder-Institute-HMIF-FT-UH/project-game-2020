using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Shop")]
public class ShopScriptableObject : ScriptableObject
{
    public string itemName;
    // Prefs
    public enum PrefsType
    {
        SetFloat,
        SetInt
    }
    public string[] itemPrefsName;
    public PrefsType[] prefsType;

    public int price;
    public float[] additionalItem;
    public Sprite itemLogo;
    public Sprite[] additionalItemLogo;
    public Sprite itemBanner;
}
