using System.Collections.Generic;
using FAS.Players.Animations;
using FAS.Projectiles;
using System.Linq;
using FAS.Weapons;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Players
{
	public class PlayerWeapon : MonoBehaviour, IReadOnlyPlayerWeapon
	{
		[SerializeField] private bool _isHasStartWeapon;
		[ShowIf(nameof(_isHasStartWeapon))]
		[SerializeField] private WeaponType _startWeaponType;
		[SerializeField] private WeaponName _startWeaponName;
		[EndIf]
		
		[Inject] private PlayerAnimEventsReceiver _animEvents;
		[Inject] private PlayerTargetFinder _targetFinder;
		[Inject] private DamageReceiver _damageReceiver;
		[Inject] private ProjectilePool _projectilePool;
		[Inject] private PlayerAnimator _animator;
		[Inject] private List<Weapon> _weapons;

		private Weapon _equippedWeapon;

		public IReadOnlyWeapon CurrentWeapon => _equippedWeapon;

		public bool IsHasWeapon => _equippedWeapon != null;
		
		private void Awake()
		{
			foreach (var weapon in _weapons)
				weapon.gameObject.SetActive(false);
			
			if (_isHasStartWeapon)
			{
				var startWeapon = _weapons.FirstOrDefault(weapon =>
					weapon.Type == _startWeaponType && weapon.Name == _startWeaponName);

				if (startWeapon == null)
					Debug.LogError("Start weapon dosen't exist");
				else
					Equip(startWeapon);
			}
		}

		private void OnEnable()
		{
			_animEvents.OnAttack += OnAttack;
			_animEvents.OnDisarm += Disarm;
			_animEvents.OnArm += Arm;
		}
		
		private void OnDisable()
		{
			_animEvents.OnAttack -= OnAttack;
			_animEvents.OnDisarm -= Disarm;
			_animEvents.OnArm -= Arm;
		}

		public void Equip(Weapon weapon)
		{
			_equippedWeapon = weapon;
			_equippedWeapon.gameObject.SetActive(true);
			_equippedWeapon.Equip();
			_equippedWeapon.Show();
			Disarm();
		}

		public void Disarm()
		{
			_equippedWeapon.EnableSimpleMaterials();
			_equippedWeapon.Disarm();
		}

		public void Arm()
		{
			_equippedWeapon.EnableDetailedMaterials();
			_equippedWeapon.Arm();
		}

		private void OnAttack()
		{
			if (_equippedWeapon.Type == WeaponType.Range)
			{
				if (_equippedWeapon.Name == WeaponName.Axe)
				{
					var projectile = _projectilePool.Get();
					projectile.transform.SetPositionAndRotation(
						_equippedWeapon.transform.position, _equippedWeapon.transform.rotation);
					
					projectile.Launch(_targetFinder.CurrentTarget.HeadPosition, float.MaxValue, _damageReceiver);
				}
			}
		}

		private void Update()
		{
			if (IsHasWeapon)
				_animator.SetEquipped();
			else
				_animator.SetUnequipped();
		}
	}
}