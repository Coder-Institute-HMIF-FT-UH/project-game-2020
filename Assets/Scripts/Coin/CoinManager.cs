using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private Text coinValue;
    
    private void Awake()
    {
        long currentCoin = Convert.ToInt64(PlayerPrefs.GetString(PlayerPrefsConstant.Coins));
        // Uncomment below to gain bonusCoin LOL....
        // long bonus = 10000000;
        // PlayerPrefs.SetString(PlayerPrefsConstant.Coins, (currentCoin+bonus).ToString());
        // currentCoin = Convert.ToInt64(PlayerPrefs.GetString(PlayerPrefsConstant.Coins));
        coinValue.text = $"{currentCoin:N0}";
    }
}
