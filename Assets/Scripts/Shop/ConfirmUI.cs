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
            float currentItemValue;
            
            // If item that wanted to be bought equals to 1, ...
            if (shopScriptableObject.itemPrefsName.Length == 1)
            {
                switch (shopScriptableObject.prefsType[0])
                {
                    case ShopScriptableObject.PrefsType.SetFloat:
                        // Get currentItemValue
                        currentItemValue = PlayerPrefs.GetFloat(shopScriptableObject.itemPrefsName[0]);
                        Debug.Log("Current item: " + currentItemValue);
                        
                        // If item is not full, buy it
                        if(!IsItemFull(currentItemValue, shopScriptableObject.maxItem[0]))
                        {
                            // Additional item is divided by 100
                            // because battery is float type.
                            PlayerPrefs.SetFloat(shopScriptableObject.itemPrefsName[0],
                                currentItemValue + shopScriptableObject.additionalItem[0]);
                        }
                        // If item is full, ...
                        else
                        {
                            isItemFull = true;
                            SetWarning(shopScriptableObject.warning[0]);
                        }
                        
                        break;
                    
                    case ShopScriptableObject.PrefsType.SetInt:
                        // Get currentItemValue
                        currentItemValue = PlayerPrefs.GetInt(shopScriptableObject.itemPrefsName[0]);
                        Debug.Log("Current item: " + currentItemValue);
                        
                        // If item is not full, buy it
                        if (!IsItemFull(currentItemValue, shopScriptableObject.maxItem[0]))
                        {
                            PlayerPrefs.SetInt(shopScriptableObject.itemPrefsName[0],
                                (int) (currentItemValue + shopScriptableObject.additionalItem[0]));
                        }
                        // If item is full, ...
                        else
                        {
                            isItemFull = true;
                            SetWarning(shopScriptableObject.warning[0]);
                        }
                        
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            // If item that wanted to be bought is more than 1, ...
            else if (shopScriptableObject.itemPrefsName.Length > 1)
            {
                // Check if item is full
                for (int i = 0; i < shopScriptableObject.itemPrefsName.Length; i++)
                {
                    switch (shopScriptableObject.prefsType[i])
                    {
                        case ShopScriptableObject.PrefsType.SetFloat:
                            currentItemValue = PlayerPrefs.GetFloat(shopScriptableObject.itemPrefsName[i]);
                            break;
                        case ShopScriptableObject.PrefsType.SetInt:
                            currentItemValue = PlayerPrefs.GetInt(shopScriptableObject.itemPrefsName[i]);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (IsItemFull(currentItemValue, shopScriptableObject.maxItem[i]))
                    {
                        isItemFull = true;
                        SetWarning(shopScriptableObject.warning[i]);
                        return;
                    }
                }
                
                // Buy item
                for (int i = 0; i < shopScriptableObject.itemPrefsName.Length; i++)
                {
                    switch (shopScriptableObject.prefsType[i])
                    {
                        case ShopScriptableObject.PrefsType.SetFloat:
                            currentItemValue = PlayerPrefs.GetFloat(shopScriptableObject.itemPrefsName[i]);
                            // Additional item is divided by 100
                            // because battery is float type.
                            PlayerPrefs.SetFloat(shopScriptableObject.itemPrefsName[i],
                                currentItemValue + shopScriptableObject.additionalItem[i] / 100);
                            break;

                        case ShopScriptableObject.PrefsType.SetInt:
                            currentItemValue = PlayerPrefs.GetInt(shopScriptableObject.itemPrefsName[i]);

                            PlayerPrefs.SetInt(shopScriptableObject.itemPrefsName[i],
                                (int) (currentItemValue + shopScriptableObject.additionalItem[i]));
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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
