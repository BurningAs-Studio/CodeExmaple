using FAS.Players.AnimRig;
using UnityEngine;
using Zenject;

namespace FAS.Players.States
{
	public class Attack : DefaultState
	{
		[SerializeField] private float _moveSpeedMultiplier = 0.5f;

		[Inject] private PlayerAnimationRig _animationRig;
		[Inject] private PlayerWeapon _weapon;

		private bool _isDelayedAttackRequested;
		private bool _isOverTimeDamageWeapon;
		
		public override void Enter()
		{
			base.Enter();
			InputEvents.OnAttackButtonUp += TransitToDefault;
			_isOverTimeDamageWeapon = true;
			Animator.SetAimingState(true);
			RequestDelayedAttack();
		}
		
		private void TransitToDefault() => RequestTransition(IdleState);
		
		private void RequestDelayedAttack() => _isDelayedAttackRequested = true;
		
		private void TryShoot()
		{
			if (Animator.ShootingLayer.IsEnabled)
				_weapon.Fire();
		}

		public override void Perform()
		{
			Animator.RequestEnableShootingLayer();
			_animationRig.RequestEnableLeftHand(_weapon.Data.LeftHandPoint);
			_animationRig.RequestEnableRightHand();
			_animationRig.RequestEnableHands();
			_animationRig.RequestEnableSpine();
			
			Mover.SetSpeedMultiplier(_moveSpeedMultiplier);
			
			base.Perform();

			if (_isDelayedAttackRequested || (_isOverTimeDamageWeapon && _weapon.Data.IsCanShoot))
			{
				TryShoot();
				_isDelayedAttackRequested = false;
			}
			
			_weapon.UpdateShootingFocus();
			
			Animator.SetLocomotionValue(new Vector2(
					Mover.CurrentVelocity.x / Mover.CurrentMaxSpeed,
					Mover.CurrentVelocity.y / Mover.CurrentMaxSpeed));
			
			Rotator.RequestRotateTo(CameraRotator.CurrentHorizontalRotation);
		}

		public override void Exit()
		{
			Animator.SetAimingState(false);
			_isDelayedAttackRequested = false;
			_isOverTimeDamageWeapon = false;
			base.Exit();
			InputEvents.OnAttackButtonUp -= TransitToDefault;
		}
	}
}