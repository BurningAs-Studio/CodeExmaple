
namespace FAS.FakePlayers.States
{
	public class DefaultDeath : DeathState
	{
		public override void Enter()
		{
			base.Enter();
			AnimEvents.OnDeathComplete += OnDeathComplete;
			Mover.TryStopMove();

			var attackDirection =
				(DamageReceiver.LastDamageDealer.transform.position - transform.position).normalized;

			switch (DirectionHelper.GetHitDirection(transform.forward, attackDirection))
			{
				default:
				case HitDirection.Front:
					Animator.PlayDeathBackwardAnim();
					break;
				case HitDirection.Right:
					Animator.PlayDeathLeftAnim();
					break;
				case HitDirection.Back:
					Animator.PlayDeathForwardAnim();
					break;
				case HitDirection.Left:
					Animator.PlayDeathRightAnim();
					break;
			}
		}

		protected override void PerformDeath() => Mover.RequestDisableNavMesh();

		public override void Exit()
		{
			AnimEvents.OnDeathComplete -= OnDeathComplete;
			base.Exit();
		}
	}
}