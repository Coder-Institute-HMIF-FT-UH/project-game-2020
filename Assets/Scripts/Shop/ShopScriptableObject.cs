using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Shop")]
public class ShopScriptableObject : ScriptableObject
{
    public string itemName;
    public int price;
    public Sprite itemSprite;
}
