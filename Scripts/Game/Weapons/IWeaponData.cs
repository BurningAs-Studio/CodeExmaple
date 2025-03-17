using UnityEngine;

namespace FAS
{
	public interface IWeaponData
	{
		public WeaponType Type { get; }
		
		public Transform LeftHandPoint { get; }

		public float EnableLeftHandSpeed { get; }
		public float ShootingRange { get; }
		
		public int AmmoInMagazine { get; }
		public int Damage { get; }

		public bool IsCanShoot { get; }
	}
}