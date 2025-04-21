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

			Mover.TryStopMove();
			_currentIdleTime = Time.timeSinceLevelLoad + Random.Range(_minIdleTime, _maxIdleTime);
		}
		
		protected override void OnTakeDamage()
		{
			base.OnTakeDamage();
			RequestTransition(MoveToDamageDealerState);
		}
		
		public override void Perform()
		{
			if (TargetFinder.IsHasTarget && Mover.CalculatePath(TargetFinder.CurrentTarget.Position))
				RequestTransition(ChaseState);
			
			Mover.RequestEnableRootMotion();
			
			if (Time.timeSinceLevelLoad > _currentIdleTime)
				RequestTransition(PatrolState);
		}
	}
}