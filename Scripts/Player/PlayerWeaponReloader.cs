using Zenject;
using System;

namespace PetWorld.Player
{
	[Serializable]
	public class PlayerWeaponReloader : ListenerSubscriber
	{
		[Inject] private PlayerAnimatorEventReceiver _eventReceiver;
		[Inject] private IReadOnlyPlayerInputEvents _inputEvents;
		[Inject] private ReloadAnimatorLayer _animatorLayer;
		[Inject] private IPlayerInputControl _inputControl;
		[Inject] private PlayerSoundEffects _soundEffects;
		[Inject] private PlayerAnimationRig _rig;
		
		public bool IsReloading {get; private set;}
		
		public event Action OnReloadComplete;

		public override void AddListeners()
		{
			_eventReceiver.OnReloadAnimFinished += OnReloadAnimFinished;
		}

		public override void RemoveListeners()
		{
			_eventReceiver.OnReloadAnimFinished -= OnReloadAnimFinished;
		}
		
		public void Reload(WeaponAnimType weaponAnimType)
		{
			IsReloading = true;
			_inputControl.DisableButtonsInput();

			switch (weaponAnimType)
			{
				case WeaponAnimType.Rifle:
					ReloadRifle();
					break;
				case WeaponAnimType.Shotgun:
					ReloadShotGun();
					break;
				case WeaponAnimType.Pistol:
					ReloadPistol();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void ReloadRifle()
		{
			_rig.DisableHandsRig();
			_animatorLayer.PlayRifleReloadAnim();
			_soundEffects.PlayRifleReloadSoundEffect();
		}

		private void ReloadShotGun()
		{
			_animatorLayer.PlayShotGunReloadAnim();
			_soundEffects.PlayShotgunReloadSoundEffect();
		}

		private void ReloadPistol()
		{
			_rig.DisableHandsRig();
			_animatorLayer.PlayPistolReloadAnim();
			_soundEffects.PlayPistolReloadSoundEffect();
		}

		private void OnReloadAnimFinished()
		{
			_inputControl.EnableButtonsInput();
			IsReloading = false;
			OnReloadComplete?.Invoke();
		}
	}
}

