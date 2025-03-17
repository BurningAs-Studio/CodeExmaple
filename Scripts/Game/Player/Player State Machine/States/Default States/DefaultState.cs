namespace FAS.Players.States
{
	public abstract class DefaultState : PlayerState
	{
		private bool _isJumpRequested;
		
		public override void Enter()
		{
			base.Enter();
			InputEvents.OnPunchButtonClicked += RequestPunch;
			InputEvents.OnJumpButtonClicked += RequestJump;
			InputEvents.OnKickButtonClicked += RequestKick;
		}
		
		private void RequestPunch() => RequestTransition(PunchState);

		private void RequestKick() => RequestTransition(KickState);
		
		private void RequestJump() => _isJumpRequested = true;
		
		public override void Perform()
		{
			GroundChecker.CheckGround();
			
			Animator.SetGroundedState(GroundChecker.IsGrounded);

			if (GroundChecker.IsGrounded)
			{
				Jump.CheckAndRestVelocity();
				
				if (Jump.TryJump(_isJumpRequested))
					Animator.PlayJumpAnim();
			}
			else
			{
				Jump.FreeFall();
			}

			Jump.ApplyGravity();

			if (Input.IsMovementJoystickActive)
				Mover.ManualMovement();
            
			if (Weapon.IsHasWeapon)
			{
				Aim.UpdateCrosshairSize(Mover.IsMovementProcess);
				Aim.CheckAimHits();
			}
			
			_isJumpRequested = false;
		}

		public override void Exit()
		{
			InputEvents.OnPunchButtonClicked -= RequestPunch;
			InputEvents.OnKickButtonClicked -= RequestKick;
			InputEvents.OnJumpButtonClicked -= RequestJump;
			base.Exit();
		}
	}
}