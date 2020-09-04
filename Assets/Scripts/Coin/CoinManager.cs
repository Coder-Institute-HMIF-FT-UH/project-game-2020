using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private Text coinValue;

    private long currentCoin;

    private long CurrentCoin()
    {
        return Convert.ToInt64(PlayerPrefs.GetString(PlayerPrefsConstant.Coins));
    }
    
    private void Awake()
    {
        if(PlayerPrefs.HasKey(PlayerPrefsConstant.Coins))
        {
            currentCoin = CurrentCoin();
        }
        else
        {
            currentCoin = 0;
            PlayerPrefs.SetString(PlayerPrefsConstant.Coins, currentCoin.ToString());
        }
        
        UpdateCoinUi();
    }

    private void Update()
    {
        if (currentCoin != CurrentCoin())
        {
            currentCoin = CurrentCoin();
            UpdateCoinUi();
        }
    }

    /// <summary>
    /// Update Coin UI
    /// </summary>
    private void UpdateCoinUi()
    {
        coinValue.text = $"{currentCoin:N0}";
    }

    /// <summary>
    /// Just for testing
    /// </summary>
    public void AddCoin()
    {
        // Uncomment below to gain bonusCoin LOL....
        long bonus = 10000;
        PlayerPrefs.SetString(PlayerPrefsConstant.Coins, (currentCoin+bonus).ToString());
        // currentCoin = CurrentCoin();
    }
}
