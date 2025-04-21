using UnityEngine;

namespace FAS.Players.States
{
	public class Death : StunState
	{
		private const float FRONT_ANGLE_MIN = -45f;
		private const float FRONT_ANGLE_MAX = 45f;
		private const float RIGHT_ANGLE_MIN = 45f;
		private const float RIGHT_ANGLE_MAX = 135f;
		private const float BACK_ANGLE_MIN = 135f;
		private const float BACK_ANGLE_MAX = -135f;
		private const float LEFT_ANGLE_MIN = -135f;
		private const float LEFT_ANGLE_MAX = -45f;
		
		protected override PlayerState ExitState => RespawnState;

		public override void Enter()
		{
			base.Enter();
			Pickup.Disable();
			DamageReceiver.DisableDamageableColliders();
			Animator.SetLocomotionValue(0);
			
			var attackDirection =
				(DamageReceiver.LastDamageDealer.transform.position - transform.position).normalized;
			var angle = Vector3.SignedAngle(transform.forward, attackDirection, Vector3.up);

			if (angle >= FRONT_ANGLE_MIN && angle <= FRONT_ANGLE_MAX)
				Animator.PlayDeathBackwardAnim();
			else if (angle > RIGHT_ANGLE_MIN && angle <= RIGHT_ANGLE_MAX)
				Animator.PlayDeathLeftAnim();
			else if (angle >= BACK_ANGLE_MIN || angle <= BACK_ANGLE_MAX)
				Animator.PlayDeathForwardAnim();
			else if (angle >= LEFT_ANGLE_MIN && angle < LEFT_ANGLE_MAX)
				Animator.PlayDeathRightAnim();
			else
				Animator.PlayDeathBackwardAnim();
		}
	}
}