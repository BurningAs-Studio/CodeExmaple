using UnityEngine;
using VInspector;
using System;
using Zenject;

namespace FAS
{
	[Serializable]
	public enum DamageableType
	{
		Generic = 0,
		Player = 1,
		Enemy = 2,
		Ally = 3
	}
	
	[RequireComponent(typeof(CapsuleCollider))]
	public class DamageableCollider : MonoBehaviour
	{
		[field: SerializeField] public DamageableData Data { get; private set; }
		
		private CapsuleCollider _collider;
		
		private float _nextTimeMarkAsTarget;
		
		private const float HEADSHOT_MULTIPLIER = 3f;

		public event Action<float, BodyPart, DamageReceiver> OnTakeDamage;
		public event Action OnTryDisableOutline;
		public event Action OnTryEnableOutline;

		public void Initialize() => _collider = GetComponent<CapsuleCollider>();

		public void Enable() => _collider.enabled = true;

		public void Disable() => _collider.enabled = false;

		public void TakeDamage(float damage, DamageReceiver damageDealer) =>
			OnTakeDamage?.Invoke(damage * GetMultiplier(), Data.BodyPart, damageDealer);
		
		private float GetMultiplier()
		{
			float multiplier = 1;

			if (Data.BodyPart == BodyPart.Head)
				multiplier = HEADSHOT_MULTIPLIER;
			
			return multiplier;
		}

		public void TryDisableOutline() => OnTryDisableOutline?.Invoke();
		
		public void TryEnableOutline() => OnTryEnableOutline?.Invoke();
	}
}