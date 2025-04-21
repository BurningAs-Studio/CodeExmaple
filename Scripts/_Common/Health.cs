using UnityEngine;
using System;

namespace FAS
{
	public class Health : MonoBehaviour
	{
		[SerializeField] private int _startHealth = 100;
		
		public float CurrentHealth { get; private set; }
		public float StartHealth => _startHealth;
		
		public bool IsFull => CurrentHealth >= _startHealth;
		public bool IsDead => CurrentHealth <= 0;

		public event Action OnFullHealthTakeDamage;
		public event Action OnTakeDamage;
		public event Action OnTakeHeal;
		public event Action OnZeroHealth;

		private void Awake()
		{
			CurrentHealth = _startHealth;
		}

		public void TryHeal(float amount = 1)
		{
			if (CurrentHealth < _startHealth)
				Heal(amount);
		}

		protected virtual void Heal(float amount)
		{
			CurrentHealth += amount;
			
			if (CurrentHealth > _startHealth)
				CurrentHealth = _startHealth;
			
			OnTakeHeal?.Invoke();
		}

		public virtual void TryTakeDamage(float amount)
		{
			if (CurrentHealth > 0)
			{
				if (CurrentHealth >= _startHealth)
					FullHealthTakeDamage();
			
				CurrentHealth -= amount;
				OnTakeDamage?.Invoke();

				if (CurrentHealth <= 0)
				{
					CurrentHealth = 0;
					Kill();
				}	
			}
		}

		protected virtual void FullHealthTakeDamage()
		{
			OnFullHealthTakeDamage?.Invoke();
		}

		protected virtual void Kill()
		{
			OnZeroHealth?.Invoke();
		}
	}
}