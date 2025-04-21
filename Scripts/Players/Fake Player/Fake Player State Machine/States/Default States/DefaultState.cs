namespace FAS.FakePlayers.States
{
	public abstract class DefaultState : FakePlayerState
	{
		public override void Enter()
		{
			FatalityTarget.OnReceivedFatality += OnReceivedFatality;
			Health.OnTakeDamage += OnTakeDamage;
			KickReceiver.OnKick += OnKick;
			Health.OnZeroHealth += OnDead;
		}
		
		private void OnReceivedFatality() => RequestTransition(ReceiveFatalityState);
		
		private void OnKick() => RequestTransition(PuppetState);

		private void OnDead() => RequestTransition(DefaultDeathState);

		private void OnTakeDamage()
		{
			SoundEffects.PlayTakeDamageSound();
			Animator.PlayTakeDamageAnim();
		}

		public override void Perform()
		{
			if (TargetFinder.IsHasTargetZombie)
				PerformWithTarget();
			else
				PerformWithoutTarget();
			
			if (GroundChecker.IsGrounded)
				Jump.CheckAndRestVelocity();
			else
				Jump.FreeFall();

			Jump.ApplyGravity();
		}

		protected virtual void PerformWithoutTarget()
		{
			
		}

		protected virtual void PerformWithTarget()
		{
			if (!KickState.IsCooldownActive && KickState.IsTargetInRange(TargetFinder.CurrentTargetZombie.Position))
				RequestTransition(KickState);
		}

		public override void Exit()
		{
			Health.OnZeroHealth -= OnDead;
			KickReceiver.OnKick -= OnKick;
			Health.OnTakeDamage -= OnTakeDamage;
			FatalityTarget.OnReceivedFatality -= OnReceivedFatality;
			base.Exit();
		}
	}
}