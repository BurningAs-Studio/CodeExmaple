using UnityEngine;

namespace FAS.Zombies.States
{
	public class Death : ZombieState
	{
		[SerializeField] private float _moveUndergroundSpeed = 1f;
		[SerializeField] private float _moveUndergroundDelay = 3f;

		private Vector3 _undergroundPosition;
		
		private float _nextTimeMoveUnderground;
		
		private bool _isDeathAnimComplete;
		
		private const float FRONT_ANGLE_MIN = -45f;
		private const float FRONT_ANGLE_MAX = 45f;
		private const float RIGHT_ANGLE_MIN = 45f;
		private const float RIGHT_ANGLE_MAX = 135f;
		private const float BACK_ANGLE_MIN = 135f;
		private const float BACK_ANGLE_MAX = -135f;
		private const float LEFT_ANGLE_MIN = -135f;
		private const float LEFT_ANGLE_MAX = -45f;
		private const float UNDERGROUND_OFFSET = 5f;
		
		public override void Enter()
		{
			AnimEventsReceiver.OnDeathComplete += OnDeathAnimComplete;
			DamageReceiver.DisableDamageableColliders();
			Mover.StopMove();

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

		private void OnDeathAnimComplete()
		{
			_undergroundPosition = transform.position - Vector3.up * UNDERGROUND_OFFSET;
			_nextTimeMoveUnderground = Time.timeSinceLevelLoad + _moveUndergroundDelay;
			_isDeathAnimComplete = true;
		}

		public override void Perform()
		{
			if (_isDeathAnimComplete)
			{
				if (Time.timeSinceLevelLoad > _nextTimeMoveUnderground)
				{
					Mover.RequestTransformMove(_undergroundPosition, _moveUndergroundSpeed);
					
					if (Vector3.SqrMagnitude(transform.position - _undergroundPosition) < 0.1f)
						RequestTransition(RespawnState);
				}
			}
			else
			{
				Mover.RequestEnableRootMotion();
			}
		}

		public override void Exit()
		{
			base.Exit();
			AnimEventsReceiver.OnDeathComplete -= OnDeathAnimComplete;
			_isDeathAnimComplete = false;
		}
	}
}
