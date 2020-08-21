using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public ShopScriptableObject shopScriptableObject;
    [SerializeField] private Image itemSprite;
    [SerializeField] private Text priceValue;

    private void Awake()
    {
        itemSprite.sprite = shopScriptableObject.itemSprite;
        // Format int to price-like number
        priceValue.text = $"{shopScriptableObject.price:N0}";
    }
}
