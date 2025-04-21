using UnityEngine;
using FAS.Players;

namespace FAS
{
	[RequireComponent(typeof(SphereCollider))]
	public class RespawnPoint : MonoBehaviour
	{
		public float SqrDistanceToPlayer;
		
		private SphereCollider _collider;
		
		public bool IsHasTarget { get; private set; }

		private void Awake()
		{
			_collider = GetComponent<SphereCollider>();
		}
		
		public void SetRadius(float radius) => _collider.radius = radius;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Player player))
				IsHasTarget = true;
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Player player))
				IsHasTarget = false;
		}
	}
}