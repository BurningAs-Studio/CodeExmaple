using UnityEngine;

namespace FAS.Zombies.States
{
	public class Idle : ZombieState
	{
		[SerializeField] private float _minIdleTime = 3f;
		[SerializeField] private float _maxIdleTime = 10f;

		private float _currentIdleTime;

		public override void Enter()
		{
			base.Enter();
			Health.OnTakeDamage += OnTakeDamage;

			Mover.StopMove();
			_currentIdleTime = Time.timeSinceLevelLoad + Random.Range(_minIdleTime, _maxIdleTime);
		}
		
		private void OnTakeDamage() => RequestTransition(MoveToDamageDealerState);

		public override void Perform()
		{
			if (TargetFinder.IsHasTarget && Mover.CalculatePath(TargetFinder.CurrentTarget.Position))
				RequestTransition(ChaseState);
			
			Mover.RequestEnableRootMotion();
			
			if (Time.timeSinceLevelLoad > _currentIdleTime)
				RequestTransition(PatrolState);
		}

		public override void Exit()
		{
			Health.OnTakeDamage -= OnTakeDamage;
			base.Exit();
		}
	}
}