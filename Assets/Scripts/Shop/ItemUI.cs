﻿using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public ShopScriptableObject shopScriptableObject;
    [SerializeField] private Image itemBanner;
    [SerializeField] private Text priceValue;

    private void Awake()
    {
        itemBanner.sprite = shopScriptableObject.itemBanner;
        // Format int to price-like number
        priceValue.text = $"{shopScriptableObject.price:N0}";
    }

    /// <summary>
    /// Set confirmation UI
    /// </summary>
    /// <param name="confirmContainer"></param>
    public void SetConfirmation(ConfirmUI confirmContainer)
    {
        if(!confirmContainer) return;
        confirmContainer.shopScriptableObject = shopScriptableObject;
        confirmContainer.SetConfirmUi();
        confirmContainer.gameObject.SetActive(true);
    }
}
