using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
	[Serializable]
	public class PlayerAttacker
	{
		[SerializeField] private PlayerWeaponChanger _weaponChanger;
		[SerializeField] private PlayerWeapon _weapon;
		
		[field: SerializeField] public PlayerAim Aim {get; private set;}
		
		[Inject] [NonSerialized] private PlayerAnimationRig _animationRig;
		[Inject] [NonSerialized] private DamageReceiver _damageReceiver;

		private bool _isDelayedAttackRequested;
		private bool _isOverTimeDamageWeapon;
		
		public IWeaponVisibility WeaponVisibility => _weapon;
		public IWeaponHolder WeaponHolder => _weapon;
		
		[Inject]
		public void Construct(DiContainer diContainer)
		{
			diContainer.Inject(_weaponChanger);
			diContainer.Inject(_weapon);
			diContainer.Inject(Aim);
		}

		public void Initialize()
		{
			_weaponChanger.Initialize();
			Aim.Initialize();
		}

		public void StartAttack()
		{
			_isOverTimeDamageWeapon = true;
			
			if (!_weapon.IsReadyToFire)
				RequestDelayedAttack();
		}
		
		private void RequestDelayedAttack()
		{
			_weapon.ReadyToFire();
			_isDelayedAttackRequested = true;
		}

		public void StopAttack()
		{
			_weapon.StopShooting();
			_isOverTimeDamageWeapon = false;
		}
		
		private void Shoot()
		{
			if (_weapon.Data.IsHasAmmoInMagazine)
			{
				_weapon.Fire();
				TryTakeDamage();
			}
			else
			{
				_weapon.DryFire();
			}
		}
		
		private void TryTakeDamage()
		{
			if (Aim.IsHasTargetInAim)
				Aim.CurrentTarget.TakeDamage(_weapon.Data.Damage, _damageReceiver);
		}

		public void AttackProcess()
		{
			if (_isDelayedAttackRequested && _animationRig.IsTorsoRigEnabled)
			{
				Shoot();
				_isDelayedAttackRequested = false;
			}
			else if (_isOverTimeDamageWeapon && _weapon.Data.IsCanShoot)
			{
				Shoot();
			}
			
			_weapon.UpdateShootingFocus();
		}
	}
}