using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private Text coinValue;
    
    private void Awake()
    {
        long currentCoin = Convert.ToInt64(PlayerPrefs.GetString(PlayerPrefsConstant.Coins));
        coinValue.text = $"{currentCoin:N0}";
    }
}
