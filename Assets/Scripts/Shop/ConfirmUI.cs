using UnityEngine;
using UnityEngine.UI;

public class ConfirmUI : MonoBehaviour
{
    public ShopScriptableObject shopScriptableObject;
    [SerializeField] private Image itemLogo,
        itemShadowLogo,
        itemAdditionalLogo;
    [SerializeField] private Text itemName,
        itemPrice,
        itemAdditional;

    /// <summary>
    /// Set Confirmation UI
    /// </summary>
    public void SetConfirmUi()
    {
        if(shopScriptableObject)
        {
            itemLogo.sprite = itemShadowLogo.sprite = itemAdditionalLogo.sprite = shopScriptableObject.itemLogo;
            itemName.text = shopScriptableObject.itemName;
            itemPrice.text = $"{shopScriptableObject.price:N0}";
            itemAdditional.text = $"{shopScriptableObject.itemAdditional} x";
        }
    }
}
