using UnityEngine;
using VInspector;

namespace FAS.Pickups
{
	[RequireComponent(typeof(SphereCollider))]
	public class PickupHandler : MonoBehaviour
	{
		[SerializeField] private Pickupable _pickupable;
		[field: SerializeField] public bool IsPickupOnTouch { get; private set; } = true;
		
		private SphereCollider _collider;

		private void Awake() => _collider = GetComponent<SphereCollider>();

		private void OnEnable() => _pickupable.OnRespawn += Restore;
		
		private void OnDisable() => _pickupable.OnRespawn -= Restore;

		private void Restore() => _collider.enabled = true;
		
		public void ReceiveVisitor(IPickupVisitor visitor)
		{
			_collider.enabled = false;
			_pickupable.Apply(visitor);	
		}
		
#if UNITY_EDITOR
		[Button]
		private void TryFindPickupInParent() => _pickupable = transform.parent.GetComponent<Pickupable>();
#endif
	}
}
