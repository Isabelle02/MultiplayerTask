using System;
using UnityEngine;

public static class CurrencyManager
{
    private const string CoinsKey = "CoinsKey";
    
    public static event Action<int> CoinCollect;
    
    public static int Coins
    {
        get => PlayerPrefs.GetInt(CoinsKey, 0);
        set
        {
            CoinCollect?.Invoke(value);
            PlayerPrefs.SetInt(CoinsKey, value);
        }
    }
}