using UnityEngine;
using System;

namespace PetWorld
{
    [Serializable]
    public abstract class CountableItem
    {
        [SerializeField] protected CountableItemView View;
		
        public abstract int Amount { get; protected set;}
        
        public event Action<int> OnAmountUpdated;

        public void Initialize()
        {
            View.UpdateAmount(Amount);
        }
		
        public void Add(int amount)
        {
            Amount += amount;
            View.UpdateAmount(Amount);
            OnAmountUpdated?.Invoke(Amount);
        }

        public void Remove(int amount)
        {
            Amount -= amount;
			
            if (Amount < 0)
                Amount = 0;
			
            View.UpdateAmount(Amount);
            OnAmountUpdated?.Invoke(Amount);
        }
    }
}