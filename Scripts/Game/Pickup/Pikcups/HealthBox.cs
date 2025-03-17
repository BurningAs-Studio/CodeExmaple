using UnityEngine;

namespace FAS.Pickups
{
	public class HealthBox : Pickupable
	{
		[field: SerializeField] public int HealAmount { get; set; }
		public override void Apply(IPickupVisitor visitor) => visitor.Visit(this);
	}
}