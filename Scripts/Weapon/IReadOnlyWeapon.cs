using Cinemachine;

namespace FAS.Weapons
{
	public interface IReadOnlyWeapon
	{
		public CinemachineImpulseSource CameraImpulse { get; }

		public WeaponAnimType AnimType { get; }
		public WeaponType Type { get; }
		public WeaponName Name { get; }
		
		public bool IsReadyToAttack { get; }
		public bool IsEquipped { get; }

		public bool TryGetVignetteData(out VignetteData vignetteData);

	}
}