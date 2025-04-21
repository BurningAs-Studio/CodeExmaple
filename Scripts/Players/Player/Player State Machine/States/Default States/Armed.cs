using UnityEngine;
using Zenject;

namespace FAS.Players.States
{
	public class Armed : DefaultState
	{
		[SerializeField] private float _moveSpeedMultiplier = 0.5f;

		public override void Enter()
		{
			base.Enter();
			InputEvents.OnAttackButtonUp += TransitToAttack;
			Rotator.ForceRotateTo(CameraRotator.CurrentHorizontalRotation);
			CamerasSwitcher.PlayAimCamera();
			Animator.SetArmed();
		}

		//private void TransitToDefault() => RequestTransition(UnarmedState);
		private void TransitToAttack() => RequestTransition(AttackState);

		public override void Perform()
		{
			if (CamerasSwitcher.CurrentCamera == VirtualCameraType.Aim && TargetFinder.IsHasTarget)
				CameraRotator.RequestLookAt(TargetFinder.CurrentTarget.HeadPosition);

			TargetFinder.RequestFindClosestTarget();
			Mover.SetSpeedMultiplier(_moveSpeedMultiplier);
			
			base.Perform();
			
			if (Input.IsMovementJoystickActive)
				Mover.ManualMovement();

			Animator.SetLocomotionValue(new Vector2(
				Mover.CurrentVelocity.x / Mover.CurrentMaxSpeed,
				Mover.CurrentVelocity.y / Mover.CurrentMaxSpeed));

			Rotator.RequestRotateHorizontal(CameraRotator.CurrentHorizontalRotation);
		}

		public override void Exit()
		{
			//CamerasSwitcher.PlayFollowCamera();
			InputEvents.OnAttackButtonUp -= TransitToAttack;
			base.Exit();
		}
	}
}