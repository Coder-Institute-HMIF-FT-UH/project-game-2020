using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmUI : MonoBehaviour
{
    public ShopScriptableObject shopScriptableObject;

    [SerializeField] private Image itemLogo,
        itemShadowLogo;
    [SerializeField] private Image[] itemAdditionalLogo;
    [SerializeField] private Text itemName,
        itemPrice,
        coinValue;
    [SerializeField] private Text[] additionalItem;
    [SerializeField] private WarningUI warningContainer;
    [SerializeField] private WarningScriptableObject lowCoin;
    
    private long currentCoin;
    
    /// <summary>
    /// Set Confirmation UI
    /// </summary>
    public void SetConfirmUi()
    {
        if(shopScriptableObject)
        {
            itemLogo.sprite = itemShadowLogo.sprite = shopScriptableObject.itemLogo;
            for(int i = 0; i < shopScriptableObject.additionalItemLogo.Length; i++)
            {
                itemAdditionalLogo[i].sprite = shopScriptableObject.additionalItemLogo[i];
                additionalItem[i].text = $"{shopScriptableObject.additionalItem[i]} x";
            }
            itemName.text = shopScriptableObject.itemName;
            itemPrice.text = $"{shopScriptableObject.price:N0}";
        }
    }

    /// <summary>
    /// Buy Item
    /// </summary>
    public void BuyItem()
    {
        if (!shopScriptableObject) return;
        currentCoin = Convert.ToInt64(PlayerPrefs.GetString(PlayerPrefsConstant.Coins));
        
        // if current coin is greater or equals to price, buy it
        if(currentCoin >= shopScriptableObject.price)
        {
            StartCoroutine(CoinAnimation(0.005f));
            // Add item
            for (int i = 0; i < shopScriptableObject.itemPrefsName.Length; i++)
            {
                switch (shopScriptableObject.prefsType[i])
                {
                    case ShopScriptableObject.PrefsType.SetFloat:
                    {
                        float currentItem = PlayerPrefs.GetFloat(shopScriptableObject.itemPrefsName[i]); 
                        Debug.Log("Current Item: " + currentItem);
                        PlayerPrefs.SetFloat(shopScriptableObject.itemPrefsName[i],
                            currentItem + shopScriptableObject.additionalItem[i] / 100);
                        Debug.Log("Buy: " + PlayerPrefs.GetFloat(shopScriptableObject.itemPrefsName[i]));
                        break;
                    }
                    case ShopScriptableObject.PrefsType.SetInt:
                    {
                        int currentItem = PlayerPrefs.GetInt(shopScriptableObject.itemPrefsName[i]); 
                        Debug.Log("Current Item: " + currentItem);
                        PlayerPrefs.SetInt(shopScriptableObject.itemPrefsName[i],
                            currentItem + (int) shopScriptableObject.additionalItem[i]);
                        Debug.Log("Buy: " + PlayerPrefs.GetInt(shopScriptableObject.itemPrefsName[i]));
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        else // else
        {
            warningContainer.warningScriptableObject = lowCoin;
            warningContainer.gameObject.SetActive(true); 
            warningContainer.SetWarningUI(); // Warning
        }
    }

    private IEnumerator CoinAnimation(float waitTime)
    {
        // Decrease animation by 1000
        for(long i = currentCoin; i >= currentCoin - shopScriptableObject.price; i-=1000)
        {
            coinValue.text = $"{i:N0}";
            yield return new WaitForSeconds(waitTime);
        }
        // Update currentCoin
        currentCoin -= shopScriptableObject.price;
        // Set coin prefs
        PlayerPrefs.SetString(PlayerPrefsConstant.Coins, currentCoin.ToString());
        Debug.Log("Current Coin: " + PlayerPrefs.GetString(PlayerPrefsConstant.Coins));
    }
}
