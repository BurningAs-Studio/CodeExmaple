namespace FAS.FakePlayers.States
{
	public class Idle : DefaultState
	{
		public override void Enter()
		{
			base.Enter();
			Animator.SetLocomotionValue(0);
			Mover.SetStoppingDistance(0);
			Mover.TryStopMove();
		}
		
		protected override void PerformWithTarget()
		{
			base.PerformWithTarget();
			
			Rotator.RequestSmoothRotateBodyHorizontal(
				TargetFinder.CurrentTargetZombie.Position - transform.position);

			// if (Rotator.GetBodyAngleToTarget(TargetFinder.CurrentTargetZombie.Position) < 1)
			// 	RequestTransition(AttackState);
		}
	}
}