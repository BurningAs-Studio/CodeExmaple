using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerWeapon : ListenerSubscriber, IWeaponHolder, IWeaponVisibility
    {
        [SerializeField] private PlayerWeaponAnimator _animator;
        [SerializeField] private PlayerWeaponReloader _reloader;
        [SerializeField] private PlayerWeaponShotFocus _focus;
        
        [Inject] [NonSerialized] private ShootingAnimatorLayer _shootingAnimatorLayer;
        [Inject] [NonSerialized] private IReadOnlyPlayerInputEvents _inputEvents;
        [Inject] [NonSerialized] private PlayerCameraShaker _cameraShaker;
        [Inject] [NonSerialized] private PlayerSoundEffects _soundEffects;
        [Inject] [NonSerialized] private PlayerInputView _inputView;
        [Inject] [NonSerialized] private PlayerAim _aim;

        private Weapon _currentWeapon;

        public IWeaponData Data => _currentWeapon;

        public bool IsWeaponHolding => _currentWeapon != null;
        
        public bool IsReadyToFire => _aim.IsActive;
        
        [Inject]
        public override void Construct(DiContainer diContainer)
        {
            diContainer.Inject(_animator);
            diContainer.Inject(_reloader);
            diContainer.Inject(_focus);
            base.Construct(diContainer);
        }

        public override void AddListeners()
        {
            _inputEvents.OnReloadButtonUp += TryReload;
            _reloader.OnReloadComplete += Show;
            _aim.OnDisable += OnAimDisabled;
            _aim.OnEnable += OnAimEnabled;
        }


        public override void RemoveListeners()
        {
            _inputEvents.OnReloadButtonUp -= TryReload;
            _reloader.OnReloadComplete -= Show;
            _aim.OnDisable -= OnAimDisabled;
            _aim.OnEnable -= OnAimEnabled;

            if (IsWeaponHolding)
            {
                // _currentWeapon.OnCannotReload -= _inputView.DisableReloadButton;
                // _currentWeapon.OnCanReload -= _inputView.EnableReloadButton;
                // _currentWeapon.OnNeedReload -= _reloader.TryReload;
            }
        }

        private void OnAimDisabled()
        {
            _animator.SetWeaponAnimation(_currentWeapon.AnimType, _currentWeapon.LeftHandPoint);
        }
        
        private void OnAimEnabled()
        {
            _animator.SetWeaponAimAnimation(_currentWeapon.AnimType, _currentWeapon.LeftHandPoint);
        }
        
        public void Set(Weapon weapon)
        {
            _currentWeapon = weapon;
            Show();

            //_petBallInteraction.TryStopHold();

            // _currentWeapon.OnCannotReload += _inputView.DisableReloadButton;
            // _currentWeapon.OnCanReload += _inputView.EnableReloadButton;
            // _currentWeapon.OnNeedReload += _reloader.TryReload;
            // _currentWeapon.CheckCanReload();
        }

        public void Lose()
        {
            Hide();
            _currentWeapon = null;
            // _currentWeapon.OnCannotReload -= _inputView.DisableReloadButton;
            // _currentWeapon.OnCanReload -= _inputView.EnableReloadButton;
            // _currentWeapon.OnNeedReload -= _reloader.TryReload;
        }

        public void Hide()
        {
            _currentWeapon.Hide();
        }

        public void Show()
        {
            _currentWeapon.Show();
            
            if (_aim.IsActive)
                _animator.SetWeaponAimAnimation(_currentWeapon.AnimType, _currentWeapon.LeftHandPoint);
            else
                _animator.SetWeaponAnimation(_currentWeapon.AnimType, _currentWeapon.LeftHandPoint);
        }

        public void StopShooting()
        {
            if (Data.IsNeedAutoReload)
                TryReload();
        }

        private void TryReload()
        {
            if (!_reloader.IsReloading && _currentWeapon.IsCanReload)
            {
                _currentWeapon.Reload();
                _reloader.Reload(_currentWeapon.AnimType);
            }
        }

        public void ReadyToFire()
        {
            _aim.Enable();
        }

        public void Fire()
        {
            _currentWeapon.Fire(_aim.LastAimedPosition, _aim.IsHasTargetInAim);
            PlayShotImpact();
        }

        public void DryFire()
        {
            _currentWeapon.DryFire();
        }

        public void UpdateShootingFocus()
        {
            if (_focus.CurrentFade > 0)
                _focus.SmoothResetFade();
        }
        
        private void PlayShotImpact()
        {
            _soundEffects.TryPlayRandomSoundEffect(_currentWeapon.ShotAudioClips);
            _aim.AddCrosshairSize(_currentWeapon.CrosshairShotSizeStep);
            _cameraShaker.PlayShotImpulse(_currentWeapon.ShotCameraImpulse);
            _focus.AddFadeStep(_currentWeapon.FocusShotFadeStep);

            switch (_currentWeapon.AnimType)
            {
                case WeaponAnimType.Rifle:
                    _shootingAnimatorLayer.PlayRifleShootAnim();
                    break;
                case WeaponAnimType.Shotgun:
                    break;
                case WeaponAnimType.Pistol:
                    _shootingAnimatorLayer.PlayPistolShootAnim();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}