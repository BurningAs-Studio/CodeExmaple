using UnityEngine;

namespace FAS.Zombies.States
{
	public class Attack : ZombieState
	{
		[SerializeField] private int _damage = 10;
		[SerializeField] private float _delay = 0.5f;

		private float _nextTimeDealDamage;
		
		public override void Enter()
		{
			base.Enter();
			_nextTimeDealDamage = Time.timeSinceLevelLoad + _delay;
			Animator.PlayAttackAnim();
			Mover.StopMove();
		}

		public override void Perform()
		{
			if (TargetFinder.IsHasTarget)
			{
				var targetPosition = TargetFinder.GetCurrentTargetPosition();
				targetPosition.y = transform.position.y;

				if (Vector3.SqrMagnitude(transform.position - targetPosition) > Mover.GetSqrMagnitudeToTarget())
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