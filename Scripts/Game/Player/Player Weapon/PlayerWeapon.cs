using FAS.Players.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerWeapon : MonoBehaviour, IReadOnlyPlayerWeapon
	{
		[SerializeField] private PlayerWeaponShootFocus _focus;

		[Inject] private PlayerWeaponChanger _weaponChanger;
		[Inject] private PlayerSoundEffects _soundEffects;
		[Inject] private PlayerCameraShaker _cameraShaker;
		[Inject] private DamageReceiver _damageReceiver;
		[Inject] private PlayerAnimator _animator;
		[Inject] private PlayerAim _aim;

		public IWeaponData Data => _currentWeapon;

		public bool IsChangedThisFrame => _weaponChanger.IsChangedThisFrame;
		
		private Weapon _currentWeapon;

		public bool IsHasWeapon => _currentWeapon != null;
		
		private void OnEnable()
		{
			_weaponChanger.OnWeaponChanged += SetCurrentWeapon;
		}

		private void OnDisable()
		{
			_weaponChanger.OnWeaponChanged -= SetCurrentWeapon;
		}

		private void SetCurrentWeapon(Weapon weapon)
		{
			_currentWeapon = weapon;
			_currentWeapon.Equip();
		}
		public void Fire()
		{
			if (_currentWeapon.AmmoInMagazine > 0)
			{
				_currentWeapon.Fire(_aim.LastAimedPosition, _damageReceiver, _aim.IsHasObstacle, _aim.CurrentTarget);
				_soundEffects.TryPlayRandomSoundEffect(_currentWeapon.ShootAudioClips);
				PlayShotImpact();

				if (_currentWeapon.AmmoInMagazine <= 0)
					_currentWeapon.SetEmpty();
			}
			else
			{
				_animator.ChangeFireSkeletonAnim(Data.Type);
			}
		}

		public void UpdateShootingFocus()
		{
			if (_focus.CurrentFade > 0)
				_focus.SmoothResetFade();
		}

		private void PlayShotImpact()
		{
			_soundEffects.TryPlayRandomSoundEffect(_currentWeapon.ShootAudioClips);
			_aim.AddCrosshairSize(_currentWeapon.CrosshairShotSizeStep);
			_cameraShaker.PlayImpulse(_currentWeapon.ShotCameraImpulse);
			_focus.AddFadeStep(_currentWeapon.FocusShotFadeStep);
			_animator.PlayWeaponShootAnim(Data.Type);
		}
	}
}