using UnityEngine;

namespace FAS
{
	public interface ITarget
	{
		public Vector3 Position { get; }
		
		public void TakeDamage(float damage, DamageReceiver damageDealer);
	}
}