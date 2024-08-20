using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
	[Serializable]
	public class PlayerWeaponChanger : ListenerSubscriber
	{
		[SerializeField] private PlayerWeaponChangerView _view;
		
		[Inject] [NonSerialized] private IReadOnlyPlayerInputEvents _inputEvents;
		[Inject] [NonSerialized] private IInventoryAmmoHolder _ammoHolder;
		[Inject] [NonSerialized] private EquipmentSlots _equipmentSlots;
		[Inject] [NonSerialized] private IWeaponHolder _weaponHolder;
		
		[Inject]
		public override void Construct(DiContainer diContainer)
		{
			diContainer.Inject(_view);
			base.Construct(diContainer);
		}

		public override void AddListeners()
		{
			_inputEvents.OnMainWeaponButtonClicked += TrySelectMainWeapon;
			_inputEvents.OnPistolButtonClicked += TrySelectPistol;
			
			_equipmentSlots.GetSlot(WeaponType.MainWeapon).OnSetWeapon += SetMainWeapon;
			_equipmentSlots.GetSlot(WeaponType.Pistol).OnSetWeapon += SetPistol;
			
			_ammoHolder.OnValueChanged += TryUpdateAmmoViews;
		}

		public override void RemoveListeners()
		{
			_inputEvents.OnMainWeaponButtonClicked -= TrySelectMainWeapon;
			_inputEvents.OnPistolButtonClicked -= TrySelectPistol;
			
			_equipmentSlots.GetSlot(WeaponType.MainWeapon).OnSetWeapon -= SetMainWeapon;
			_equipmentSlots.GetSlot(WeaponType.Pistol).OnSetWeapon -= SetPistol;
			
			_ammoHolder.OnValueChanged -= TryUpdateAmmoViews;
		}
		
		public void Initialize()
		{
			_view.Initialize();
		}

		private void SetMainWeapon()
		{
			_view.EnableMainWeaponButton();
			SelectMainWeapon();
			TryUpdateMainWeaponButtonAmmoView(AmmoType.ShotgunBullet);
			TryUpdateMainWeaponButtonAmmoView(AmmoType.RifleBullet);
		}
		
		private void SetPistol()
		{
			_view.EnablePistolButton();
			SelectPistol();
			TryUpdatePistolButtonAmmoView(_ammoHolder.GetAmmoAmount(AmmoType.RifleBullet));
		}
		
		private void SelectMainWeapon()
		{
			var mainWeapon = _equipmentSlots.GetSlot(WeaponType.MainWeapon).GetWeapon();
			ChangeWeapon(mainWeapon);
			_view.SelectMainWeaponButton(mainWeapon.ButtonImage);
		}

		private void SelectPistol()
		{
			var pistol = _equipmentSlots.GetSlot(WeaponType.Pistol).GetWeapon();
			ChangeWeapon(pistol);
			_view.SelectPistolButton(pistol.ButtonImage);
		}

		private void TrySelectMainWeapon()
		{
			if (_weaponHolder.Data.Type == WeaponType.Pistol)
				SelectMainWeapon();
		}

		private void TrySelectPistol()
		{
			if (_weaponHolder.Data.Type == WeaponType.MainWeapon)
				SelectPistol();
		}

		private void ChangeWeapon(Weapon weapon)
		{
			if (_weaponHolder.IsWeaponHolding)
				_weaponHolder.Lose();
			
			_weaponHolder.Set(weapon);
		}
		
		private void TryUpdateAmmoViews(AmmoType ammoType, int ammoCount)
		{
			TryUpdateMainWeaponButtonAmmoView(ammoType);
			TryUpdatePistolButtonAmmoView(ammoCount);
		}

		private void TryUpdateMainWeaponButtonAmmoView(AmmoType ammoType)
		{
			var mainWeaponSlot = _equipmentSlots.GetSlot(WeaponType.MainWeapon);
			
			if (!mainWeaponSlot.IsEmpty && mainWeaponSlot.GetWeapon().AmmoType == ammoType)
				_view.UpdateMainWeaponButtonAmmoCount(_ammoHolder.GetAmmoAmount(ammoType));
		}

		private void TryUpdatePistolButtonAmmoView(int ammoCount)
		{
			if (!_equipmentSlots.GetSlot(WeaponType.Pistol).IsEmpty)
				_view.UpdatePistolButtonAmmoCount(ammoCount);
		}
	}
}