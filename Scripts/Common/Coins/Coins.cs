using FAS.Save;
using UnityEngine;

namespace FAS
{
    public class Coins
    {
        private const int COINS_AMOUNT_FOR_LEVEL = 100;
        
        public bool IsHasCoinsToPay(int amount) => SaveData.CoinsAmount >= amount;
        
        public void AddCoins(int amount) => SaveData.CoinsAmount += amount;

        public void DecreaseCoins(int amount) => SaveData.CoinsAmount -= amount;
        
        public void AddCoinsForLevel() => AddCoins(COINS_AMOUNT_FOR_LEVEL);
    }
}