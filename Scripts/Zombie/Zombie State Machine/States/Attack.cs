using UnityEngine;

namespace FAS.Zombies.States
{
	public class Attack : ZombieState
	{
		[SerializeField] private int _damage = 10;
		[SerializeField] private float _delay = 0.5f;

		private float _nextTimeDealDamage;

		private const float DISTANCE_TO_STOP_ATTACK = 0.3f;
		
		public override void Enter()
		{
			base.Enter();
			_nextTimeDealDamage = Time.timeSinceLevelLoad + _delay;
			Animator.PlayAttackAnim();
			Mover.TryStopMove();
		}

		public override void Perform()
		{
			if (TargetFinder.IsHasTarget)
			{
				var targetPosition = TargetFinder.GetCurrentTargetPosition();
				targetPosition.y = transform.position.y;

				if (Vector3.SqrMagnitude(transform.position - targetPosition)
				    - Mover.StoppingSqrMagnitude > DISTANCE_TO_STOP_ATTACK)
					RequestTransition(StopAttackState);
				else if (Time.timeSinceLevelLoad > _nextTimeDealDamage)
					DealDamage();
			}
			else
			{
				RequestTransition(StopAttackState);
			}
		}

		private void DealDamage()
		{
			TargetFinder.CurrentTarget.TakeDamage(_damage, DamageReceiver);
			_nextTimeDealDamage = Time.timeSinceLevelLoad + _delay;
		}

		public override void Exit()
		{
			Animator.StopAttackAnim();
			base.Exit();
		}
	}
}