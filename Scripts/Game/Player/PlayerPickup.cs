using System.Collections.Generic;
using UnityEngine;
using FAS.Pickups;
using Zenject;

namespace FAS.Players
{
	public class PlayerPickup : MonoBehaviour, IPickupVisitor
	{
		[Inject] private List<Weapon> _weapons = new ();
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

		public void Visit(AmmoBox ammoBox)
		{
			ammoBox.Pickup();

			foreach (var weapon in _weapons)
			{
				switch (weapon.Type)
				{
					case WeaponType.Pistol:
						weapon.AddAmmo(ammoBox.PistolAmmoAmount);
						break;
					case WeaponType.Rifle:
						weapon.AddAmmo(ammoBox.RifleAmmoAmount);
						break;
				}
			}
		}
	}
}