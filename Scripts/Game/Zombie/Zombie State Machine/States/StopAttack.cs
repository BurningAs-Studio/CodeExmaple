namespace FAS.Zombies.States
{
	public class StopAttack : ZombieState
	{
		private readonly int _stopAttackAnimHash = UnityEngine.Animator.StringToHash("Stop Attack");
		private readonly int _attackAnimHash = UnityEngine.Animator.StringToHash("Attack");

		public override void Enter()
		{
			base.Enter();
			Mover.SetStoppingDistance(0);
			Rotator.SetAutoAngularSpeed(0);
			Mover.StopMove();
		}

		public override void Perform()
		{
			Mover.RequestEnableRootMotion();

			if (Animator.BaseLayerInfo.shortNameHash != _stopAttackAnimHash
			    && Animator.BaseLayerInfo.shortNameHash != _attackAnimHash)
			{
				if (TargetFinder.IsHasTarget)
					RequestTransition(ChaseState);
				else
					RequestTransition(IdleState);
			}
		}
	}
}