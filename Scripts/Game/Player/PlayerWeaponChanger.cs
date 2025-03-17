using System.Collections.Generic;
using FAS.Players.Animations;
using UnityEngine;
using VInspector;
using Zenject;
using System;

namespace FAS.Players
{
	public class PlayerWeaponChanger : MonoBehaviour
	{
		[SerializeField] private WeaponType _startWeapon = WeaponType.Rifle;
		
		[Inject] private IReadOnlyPlayerInputEvents _events;
		[Inject] private IReadOnlyPlayerWeapon _weapon;
		[Inject] private PlayerAnimator _animator;
		[Inject] private List<Weapon> _weapons;

		public bool IsChangedThisFrame {get; private set;}

		public event Action<Weapon> OnWeaponChanged;
		
		private void OnEnable()
		{
			_events.OnChangeWeaponButtonClicked += SwitchWeapon;
		}

		private void OnDisable()
		{
			_events.OnChangeWeaponButtonClicked -= SwitchWeapon;
		}

		private void Start()
		{
			ChangeWeapon(_startWeapon);
		}

		private void SetPistol()
		{
			ChangeWeapon(WeaponType.Pistol);
		}

		private void SetRifle()
		{
			ChangeWeapon(WeaponType.Rifle);
		}
		
		private void SwitchWeapon()
		{
			if (_weapon.Data.Type == WeaponType.Pistol)
				SetRifle();
			else
				SetPistol();
		}

		private void ChangeWeapon(WeaponType type)
		{
			Weapon currentWeapon = null;
			
			foreach (var weapon in _weapons)
			{
				if (weapon.Type == type)
				{
					currentWeapon = weapon;
					weapon.gameObject.SetActive(true);
					weapon.Equip();
				}
				else
				{
					weapon.Unequip();
					weapon.gameObject.SetActive(false);
				}
			}
			
			_animator.ChangeWeaponAnim(currentWeapon.Type);
			IsChangedThisFrame = true;
			OnWeaponChanged?.Invoke(currentWeapon);
		}

		private void LateUpdate()
		{
			IsChangedThisFrame = false;
		}
	}
}