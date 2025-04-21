namespace FAS.Players.States
{
	public abstract class DefaultState : PlayerState
	{
		private bool _isJumpRequested;
		
		public override void Enter()
		{
			InputEvents.OnFatalityButtonClicked += RequestExecuteFatality;
			InputEvents.OnPunchButtonClicked += RequestPunch;
			InputEvents.OnJumpButtonClicked += RequestJump;
			InputEvents.OnKickButtonClicked += RequestKick;
			Health.OnTakeDamage += OnTakeDamage;
			KickReceiver.OnKick += OnKick;
			Health.OnZeroHealth += OnDead;
		}
		
		private void OnDead() => RequestTransition(DeadState);

		private void OnTakeDamage()
		{
			VisualEffects.PlayTakeDamageEffect(DamageReceiver.LastDamage);
			CameraShaker.PlayTakeDamageImpulse();
			SoundEffects.PlayTakeDamageSound();
			Animator.PlayTakeDamageAnim();
		}
		
		private void RequestExecuteFatality() => RequestTransition(ExecuteFatalityState);
		
		private void RequestPunch() => RequestTransition(PunchState);
		
		private void RequestKick() => RequestTransition(KickState);

		private void OnKick() => RequestTransition(PuppetState);
		
		private void RequestJump() => _isJumpRequested = true;
		
		public override void Perform()
		{
			if (CharacterController.isGrounded)
			{
				Jump.CheckAndRestVelocity();
				Jump.TryJump(_isJumpRequested);
			}
			else
			{
				Jump.FreeFall();
			}

			Jump.ApplyGravity();
			
			_isJumpRequested = false;
		}

		public override void Exit()
		{
			Health.OnZeroHealth -= OnDead;
			KickReceiver.OnKick -= OnKick;
			Health.OnTakeDamage -= OnTakeDamage;
			InputEvents.OnKickButtonClicked -= RequestKick;
			InputEvents.OnJumpButtonClicked -= RequestJump;
			InputEvents.OnPunchButtonClicked -= RequestPunch;
			InputEvents.OnFatalityButtonClicked -= RequestExecuteFatality;
			base.Exit();
		}
	}
}