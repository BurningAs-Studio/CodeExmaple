using UnityEngine;
using VInspector;
using System;

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
		[Serializable]
		private struct ColliderData
		{
			public Vector3 Center;
			public float Radius;
			public float Height;
		}
		
		[field: SerializeField] public DamageableType Type { get; private set; }
		[SerializeField] private BodyPart _bodyPart = BodyPart.Other;
		[SerializeField] private ColliderData _defaultData;
		[SerializeField] private ColliderData _crouchData;
		
		private CapsuleCollider _collider;
		
		private const float HEADSHOT_MULTIPLIER = 3f;
		
		public BodyPart BodyPart => _bodyPart;
		
		public event Action<float, DamageReceiver> OnTakeDamage;

		public void Initialize() => _collider = GetComponent<CapsuleCollider>();

		public void Enable() => _collider.enabled = true;

		public void Disable() => _collider.enabled = false;

		public void SetDefaultData() => ChangeColliderData(_defaultData);
		
		public void SetCrouchData() => ChangeColliderData(_crouchData);

		private void ChangeColliderData(ColliderData data)
		{
			_collider.center = data.Center;
			_collider.radius = data.Radius;
			_collider.height = data.Height;
		}

		public void TakeDamage(float damage, DamageReceiver damageDealer) =>
			OnTakeDamage?.Invoke(damage * GetMultiplier(), damageDealer);
		
		private float GetMultiplier()
		{
			float multiplier = 1;

			if (_bodyPart == BodyPart.Head)
				multiplier = HEADSHOT_MULTIPLIER;
			
			return multiplier;
		}

#if UNITY_EDITOR
		[Button]
		private void SaveDefaultData()
		{
			var collider = GetComponent<CapsuleCollider>();
			_defaultData.Center = collider.center;
			_defaultData.Radius = collider.radius;
			_defaultData.Height = collider.height;
		}
		
		[Button]
		private void SaveCrouchData()
		{
			var collider = GetComponent<CapsuleCollider>();
			_defaultData.Center = collider.center;
			_defaultData.Radius = collider.radius;
			_defaultData.Height = collider.height;
		}
#endif
	}
}