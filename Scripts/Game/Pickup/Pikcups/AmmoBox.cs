using UnityEngine;

namespace FAS.Pickups
{
	public class AmmoBox : Pickupable
	{
		[field: SerializeField] public int PistolAmmoAmount { get; private set; }
		[field: SerializeField] public int RifleAmmoAmount { get; private set; }
		
		public override void Apply(IPickupVisitor visitor) => visitor.Visit(this);
	}
}