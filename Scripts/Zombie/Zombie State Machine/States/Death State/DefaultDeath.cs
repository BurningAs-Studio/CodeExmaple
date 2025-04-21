
namespace FAS.Zombies.States
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

		protected override void PerformDeath() => Mover.RequestEnableRootMotion();

		public override void Exit()
		{
			base.Exit();
			AnimEvents.OnDeathComplete -= OnDeathComplete;
		}
	}
}
