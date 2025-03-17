using System;
using DG.Tweening;
using FAS.Players.AnimRig;
using UnityEngine;
using Zenject;

namespace FAS.Players.Animations
{
	public class PlayerAnimator : MonoBehaviour
	{
		[SerializeField] private Animator _mainAnimator;
		[SerializeField] private Animator _shootingAnimator;

		[Inject] private PlayerAnimationRig _animationRig;
		
		private ShootingLayer _shootingLayerFiringSkeleton;
		
		private TakeDamageLayer _takeDamageLayer;
		private Layer _additionalShootingLayer;
		private SkillCastLayer _skillCastLayer;
		private ShootingLayer _shootingLayer;
		private DeathLayer _deathLayer;
		private BaseLayer _baseLayer;
		private Layer _jumpLayer;

		private Tween _shootingLayerWeightTween;
		
		private float _animatorSpeedMultiplier;
		
		private bool _isEnableShootingLayerRequested;
		
		public IReadOnlyAnimatorLayer SkillCastLayer => _skillCastLayer;
		public IReadOnlyAnimatorLayer ShootingLayer => _shootingLayer;
		public IReadOnlyAnimatorLayer JumpLayer => _jumpLayer;
		public IReadOnlyAnimatorLayer BaseLayer => _baseLayer;
		
		public static readonly int EmptyAnimHash = Animator.StringToHash("Empty");
		
		private const int PISTOL_LOCOMOTION_VALUE = 0;
		private const int RIFLE_LOCOMOTION_VALUE = 1;

		private const float DEFAULT_ANIMATOR_SPEED = 1.0f;
		
		private void Awake()
		{
			_baseLayer = new BaseLayer(_mainAnimator, 0);
			_jumpLayer = new Layer(_mainAnimator, 1);
			_shootingLayer = new ShootingLayer(_mainAnimator, 2);
			_additionalShootingLayer = new Layer(_mainAnimator, 3);
			_takeDamageLayer = new TakeDamageLayer(_mainAnimator, 4);
			_skillCastLayer = new SkillCastLayer(_mainAnimator, 5);
			_deathLayer = new DeathLayer(_mainAnimator, 6);
			
			_shootingLayerFiringSkeleton = new ShootingLayer(_shootingAnimator, 0);
		}
		
		public void PlayWeaponShootAnim(WeaponType weaponType)
		{
			switch (weaponType)
			{
				case WeaponType.Pistol:
				default:
					_shootingLayer.PlayPistolShootAnim();
					_shootingLayerFiringSkeleton.PlayPistolShootAnim();
					break;
				case WeaponType.Rifle:
					_shootingLayer.PlayRifleShootAnim();
					_shootingLayerFiringSkeleton.PlayRifleShootAnim();
					break;
			}
		}

		public void ChangeFireSkeletonAnim(WeaponType weaponType)
		{
			switch (weaponType)
			{
				case WeaponType.Pistol:
				default:
					_shootingLayerFiringSkeleton.PlayPistolShootAnim();
					break;
				case WeaponType.Rifle:
					_shootingLayerFiringSkeleton.PlayRifleShootAnim();
					break;
			}
		}

		public void ChangeWeaponAnim(WeaponType weaponType)
		{
			switch (weaponType)
			{
				case WeaponType.Pistol:
				default:
					_baseLayer.SetLocomotionType(PISTOL_LOCOMOTION_VALUE);
					break;
				case WeaponType.Rifle:
					_baseLayer.SetLocomotionType(RIFLE_LOCOMOTION_VALUE);
					break;
			}
		}
		
		public void SetLocomotionValue(Vector2 value) => _baseLayer.SetLocomotionValue(value);
		
		public void SetLocomotionValue(float value) => _baseLayer.SetLocomotionValue(value);

		public void PlayTakeDamageAnim() => _takeDamageLayer.PlayTakeDamageAnim();

		public void PlayPistolDeathAnim() => _deathLayer.PlayPistolDeathAnim();
		
		public void PlayRifleDeathAnim() => _deathLayer.PlayRifleDeathAnim();

		public void SetGroundedState(bool state) => _baseLayer.IsGrounded(state);
		
		public void PlayPunchAnim() => _skillCastLayer.PlayPunchAnim();
		
		public void StopPunchAnim() => _skillCastLayer.StopPunchAnim();

		public void PlayKickAnim() => _skillCastLayer.PlayKickAnim();
		
		public void StopKickAnim() => _skillCastLayer.StopKickAnim();

		public void PlayJumpAnim() => _baseLayer.PlayJumpAnim();

		public void RequestEnableShootingLayer() => _isEnableShootingLayerRequested = true;
		
		public void SetAimingState(bool state)
		{
			_shootingLayer.SetAimingState(state);
			_baseLayer.UpdateLocomotionType();
		}

		private void UpdateShootingLayerWeight()
		{
			if (_isEnableShootingLayerRequested)
			{
				if (!_shootingLayer.IsEnabled)
				{
					_shootingLayer.EnableWeightSmooth();
					_additionalShootingLayer.EnableWeightSmooth();	
				}
				
				_isEnableShootingLayerRequested = false;
			}
			else if (!_shootingLayer.IsDisabled)
			{
				_shootingLayer.DisableWeightSmooth();
				_additionalShootingLayer.DisableWeightSmooth();
			}
		}
		
		private void UpdateSpeed() => _mainAnimator.speed = DEFAULT_ANIMATOR_SPEED * _animatorSpeedMultiplier;
		
		public void SetSpeedMultiplier(float multiplier) => _animatorSpeedMultiplier = multiplier;
		
		private void ResetSpeedMultiplier() => _animatorSpeedMultiplier = 1;
		
		private void Update()
		{
			UpdateShootingLayerWeight();
			UpdateSpeed();
		}

		private void LateUpdate()
		{
			ResetSpeedMultiplier();
		}
	}
}