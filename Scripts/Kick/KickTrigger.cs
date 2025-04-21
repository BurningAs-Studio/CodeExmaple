using UnityEngine;
using System;

namespace FAS
{
	[RequireComponent(typeof(SphereCollider))]
	public class KickTrigger : MonoBehaviour
	{
		[SerializeField] private Transform _obstacleCheckingPoint;
		[SerializeField] private LayerMask _obstacleLayers;

		private SphereCollider _collider;
		
		public float Radius => _collider.radius;
		
		public event Action<DamageableCollider> OnTriggerDamageable;
		public event Action<IKickable> OnTriggerKickable;

		private void Awake()
		{
			_collider = GetComponent<SphereCollider>();
			Disable();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IKickable kickable)
			    && kickable.IsReadyToKick
			    && !IsBlockedCollider(kickable.Position))
				OnTriggerKickable?.Invoke(kickable);
			else if (other.TryGetComponent(out DamageableCollider damageable)
			         && damageable.Data.Type == DamageableType.Generic
			         && !IsBlockedCollider(damageable.transform.position))
				OnTriggerDamageable?.Invoke(damageable);
		}

		private bool IsBlockedCollider(Vector3 targetPosition)
		{
			targetPosition.y = _obstacleCheckingPoint.position.y;
			return Physics.Linecast(_obstacleCheckingPoint.position, targetPosition, _obstacleLayers);
		}

		public void Disable() => _collider.enabled = false;
	
		public void Enable() => _collider.enabled = true;
	}	
}