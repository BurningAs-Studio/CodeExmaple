using Cinemachine;
using UnityEngine;
using VInspector;

namespace FAS.Weapons
{
	public enum WeaponAnimType
	{
		ThrowMeleeWeapon = 0
	}
	
	public abstract class Weapon : MonoBehaviour, IReadOnlyWeapon
	{
		[SerializeField] private GameObject _body;
		[SerializeField] private Transform _armPoint;
		[SerializeField] private Transform _disarmPoint;
		[SerializeField] private CinemachineImpulseSource _cameraImpulse;
		[SerializeField] private bool _isUseVignetteOnAttack;
		[ShowIf(nameof(_isUseVignetteOnAttack))]
		[SerializeField] private VignetteData _vignetteData;
		[EndIf]

		public CinemachineImpulseSource CameraImpulse => _cameraImpulse;
		
		public abstract WeaponAnimType AnimType { get; }
		public abstract WeaponType Type { get; }
		public abstract WeaponName Name { get; }
		
		public bool IsReadyToAttack { get; private set; }
		public bool IsEquipped { get; private set; }

		public abstract void EnableDetailedMaterials();
		
		public abstract void EnableSimpleMaterials();
		
		public abstract void Attack();

		public bool TryGetVignetteData(out VignetteData vignetteData)
		{
			vignetteData = _vignetteData;
			return _isUseVignetteOnAttack;
		}
		
		public void Disarm() => transform.SetParent(_disarmPoint, false);
		
		public void Arm() => transform.SetParent(_armPoint, false);
		
		public virtual void Hide() => _body.gameObject.SetActive(false);
		
		public virtual void Show() => _body.gameObject.SetActive(true);
		
		public virtual void Unready() => IsReadyToAttack = false;

		public virtual void Ready() => IsReadyToAttack = true;

		public virtual void UnEquip() => IsEquipped = false;
		
		public virtual void Equip() => IsEquipped = true;
	}
}