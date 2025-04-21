using System.Collections.Generic;
using UnityEngine;
using FAS.Pickups;
using Zenject;

namespace FAS.Players
{
	public class PlayerPickup : MonoBehaviour, IPickupVisitor
	{
		[Inject] private PlayerHeadsHolder _headsHolder;
		[Inject] private Health _health;

		private bool _isActive = true;
		
		private void OnTriggerEnter(Collider other)
		{
			if (_isActive && other.TryGetComponent(out PickupHandler pickupHandler) && pickupHandler.IsPickupOnTouch)
				pickupHandler.ReceiveVisitor(this);
		}
		
		public void Disable() => _isActive = false;
		
		public void Enable() => _isActive= true;

		public void Visit(HealthBox healthBox)
		{
			healthBox.Pickup();
			_health.TryHeal(healthBox.HealAmount);
		}

		public void Visit(PickupHead head)
		{
			head.Pickup();
			//_headsHolder.Add();
			print("Head picked up");
		}
	}
}