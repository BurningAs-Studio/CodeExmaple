namespace FAS.Players.States
{
	public class Idle : DefaultState
	{
		public override void Enter()
		{
			base.Enter();
			InputEvents.OnAttackButtonDown += TransitToAttack;
		}
		
		private void TransitToAttack() => RequestTransition(AttackState);

		public override void Perform()
		{
			if (GroundChecker.IsGrounded && Weapon.IsHasWeapon && Weapon.Data.Type == WeaponType.Rifle
			    && !Weapon.IsChangedThisFrame && !Animator.BaseLayer.IsTransition && !Animator.JumpLayer.IsActive)
			{
				AnimationRig.RequestEnableHands();
				AnimationRig.RequestEnableLeftHand(Weapon.Data.LeftHandPoint);
			}
			
			base.Perform();

			if (Input.GetJoystickDirection2D().magnitude != 0)
				Rotator.RequestSmoothRotateTo(Input.GetJoystickDirection3D().normalized);
			
			Animator.SetLocomotionValue(Mover.CurrentVelocity.magnitude / Mover.CurrentMaxSpeed);
		}

		public override void Exit()
		{
			base.Exit();
			InputEvents.OnAttackButtonDown -= TransitToAttack;
		}
	}
}