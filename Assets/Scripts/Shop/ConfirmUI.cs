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

    private bool isItemFull; 
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
        isItemFull = false;
        
        if (!shopScriptableObject) return;
        currentCoin = Convert.ToInt64(PlayerPrefs.GetString(PlayerPrefsConstant.Coins));
        
        // if current coin is greater or equals to price, ...
        if(currentCoin >= shopScriptableObject.price)
        {
            // Add item
            for (int i = 0; i < shopScriptableObject.itemPrefsName.Length; i++)
            {
                switch (shopScriptableObject.prefsType[i])
                {
                    case ShopScriptableObject.PrefsType.SetFloat:
                    {
                        float currentItem = PlayerPrefs.GetFloat(shopScriptableObject.itemPrefsName[i]); 
                        Debug.Log("Current Item: " + currentItem);
                        
                        // If item is not full, buy it 
                        if (!IsItemFull(currentItem, shopScriptableObject.maxItem[i]) && !isItemFull)
                        {
                            PlayerPrefs.SetFloat(shopScriptableObject.itemPrefsName[i],
                                currentItem + shopScriptableObject.additionalItem[i] / 100);
                            Debug.Log("Buy: " + PlayerPrefs.GetFloat(shopScriptableObject.itemPrefsName[i]));
                        }
                        else
                        {
                            isItemFull = true;
                            SetWarning(shopScriptableObject.warning[i]);
                        }
                        
                        break;
                    }
                    case ShopScriptableObject.PrefsType.SetInt:
                    {
                        int currentItem = PlayerPrefs.GetInt(shopScriptableObject.itemPrefsName[i]); 
                        Debug.Log("Current Item: " + currentItem);
                        
                        // If item is not full, buy it
                        if (!IsItemFull(currentItem, shopScriptableObject.maxItem[i]) && !isItemFull)
                        {
                            PlayerPrefs.SetInt(shopScriptableObject.itemPrefsName[i],
                                currentItem + (int) shopScriptableObject.additionalItem[i]);
                            Debug.Log("Buy: " + PlayerPrefs.GetInt(shopScriptableObject.itemPrefsName[i]));
                        }
                        else
                        {
                            isItemFull = true;
                            SetWarning(shopScriptableObject.warning[i]);
                        }
                        
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            // If item is not full, minus coin
            if(!isItemFull)
            {
                StartCoroutine(CoinAnimation(0.005f));
            }
        }
        else // else
        {
            SetWarning(lowCoin);
        }
    }

    /// <summary>
    /// Set warning
    /// </summary>
    /// <param name="warning"></param>
    private void SetWarning(WarningScriptableObject warning)
    {
        warningContainer.warningScriptableObject = warning;
        warningContainer.gameObject.SetActive(true); 
        warningContainer.SetWarningUI(); // Warning
    }

    /// <summary>
    /// Check if item is full
    /// </summary>
    /// <param name="currentItem"></param>
    /// <param name="maxItem"></param>
    /// <returns></returns>
    private bool IsItemFull(float currentItem, float maxItem)
    {
        return currentItem >= maxItem;
    }

    /// <summary>
    /// Check if item is full
    /// </summary>
    /// <param name="currentItem"></param>
    /// <param name="maxItem"></param>
    /// <returns></returns>
    private bool IsItemFull(int currentItem, int maxItem)
    {
        return currentItem >= maxItem;
    }
    
    /// <summary>
    /// Play coin animation and minus coin
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
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
