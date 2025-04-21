using System.Collections.Generic;
using UnityEngine;
using VInspector;
using System;

namespace FAS
{
	[RequireComponent(typeof(Health))]
	public class DamageReceiver : MonoBehaviour
	{
		[SerializeField] private List<DamageableCollider> _damageableColliders = new ();

		private Health _health;
		
		public DamageReceiver LastDamageDealer { get; private set; }

		public float LastDamage { get; private set; }
		
		public float LastDamageTime { get; private set; }

		public event Action OnTakeHitWithoutDamage;
		public event Action OnTakeDamageInHead;

		private void Awake()
		{
			_health = GetComponent<Health>();

			foreach (var damageableCollider in _damageableColliders)
				damageableCollider.Initialize();
		}

		private void OnEnable()
		{
			foreach (var damageableCollider in _damageableColliders)
				damageableCollider.OnTakeDamage += TryTakeDamage;
		}

		private void OnDisable()
		{
			foreach (var damageableCollider in _damageableColliders)
				damageableCollider.OnTakeDamage -= TryTakeDamage;
		}
		
		public bool IsSelfCollider(DamageableCollider damageableCollider)
			=> _damageableColliders.Contains(damageableCollider);

		public void DisableDamageableColliders()
		{
			foreach (var damageableCollider in _damageableColliders)
				damageableCollider.Disable();
		}

		public void EnableDamageableColliders()
		{
			foreach (var damageableCollider in _damageableColliders)
				damageableCollider.Enable();
		}

		public void TryTakeDamage(float damage, BodyPart bodyPart, DamageReceiver damageDealer)
		{
			if (IsCanTakeDamage())
				TakeDamage(damage, damageDealer);
			else
				OnTakeHitWithoutDamage?.Invoke();
			
			if (bodyPart == BodyPart.Head)
				OnTakeDamageInHead?.Invoke();
		}

		private bool IsCanTakeDamage()
		{
			return !_health.IsDead;
		}

		private void TakeDamage(float damage, DamageReceiver damageDealer)
		{
			LastDamage = damage;
			LastDamageTime = Time.timeSinceLevelLoad;
			LastDamageDealer = damageDealer;
			_health.TryTakeDamage(damage);
		}

#if UNITY_EDITOR
		[Button("FIND COLLIDERS")]
		private void FindColliders()
		{
			_damageableColliders.Clear();
			_damageableColliders.AddRange(GetComponentsInChildren<DamageableCollider>(true));
		}
#endif
	}
}