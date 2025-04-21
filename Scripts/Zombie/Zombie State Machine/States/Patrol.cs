using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine;

namespace FAS.Zombies.States
{
	public class Patrol : ZombieState
	{
		[SerializeField] private float _patrolRadius = 40f;
		[SerializeField] private float _angularSpeed = 50f;
		
		private Vector3 _patrolCenterPosition;
		
		private const int MAX_FIND_PATROL_POSITION_ATTEMPTS = 5;
		
		private const float MIN_RANDOM_POINT_SQR_MAGNITUDE = 100f;

		private void Awake()
		{
			_patrolCenterPosition = transform.position;
		}

		public override void Enter()
		{
			base.Enter();
			Mover.SetStoppingDistance(0);
			Mover.NavMeshMove(GetNextPatrolPosition());
			Rotator.SetAutoAngularSpeed(_angularSpeed);
		}
		
		protected override void OnTakeDamage()
		{
			base.OnTakeDamage();
			RequestTransition(MoveToDamageDealerState);
		}

		public override void Perform()
		{
			if (TargetFinder.IsHasTarget)
				RequestTransition(ChaseState);

			Animator.RequestWalkAnim();

			if (!Mover.IsProcessMovement && Mover.IsMovedLastFrame)
				RequestTransition(IdleState);
		}
		
		private Vector3 GetNextPatrolPosition()
		{
			var attempts = 0;

			while (attempts < MAX_FIND_PATROL_POSITION_ATTEMPTS)
			{
				var randomMovePosition = Random.insideUnitSphere * _patrolRadius + _patrolCenterPosition;

				if (NavMesh.SamplePosition(randomMovePosition, out var hit, _patrolRadius, NavMesh.AllAreas)
				    && Vector3.SqrMagnitude(hit.position - transform.position) >= MIN_RANDOM_POINT_SQR_MAGNITUDE)
					return hit.position;

				attempts++;
			}

			return _patrolCenterPosition; 
		}

#if UNITY_EDITOR
		[SerializeField] private bool _drawGizmos = true;

		private void OnDrawGizmos()
		{
			if (_drawGizmos)
			{
				Gizmos.color = Color.magenta;

				if (Application.isPlaying)
					Gizmos.DrawWireSphere(_patrolCenterPosition, _patrolRadius);
				else
					Gizmos.DrawWireSphere(transform.position, _patrolRadius);
			}
		}
#endif
	}
}