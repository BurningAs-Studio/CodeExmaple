using UnityEngine;

namespace FAS.Zombies.States
{
	public class Chase : ZombieState
	{
		[SerializeField] private float _angularSpeed = 500f;
		[SerializeField] private float _stoppingDistance = 1f;

		public override void Enter()
		{
			base.Enter();
			Mover.SetStoppingDistance(_stoppingDistance);
			Mover.NavMeshMove(TargetFinder.GetCurrentTargetPosition());
			Rotator.SetAutoAngularSpeed(_angularSpeed);
		}
		
		public override void Perform()
		{
			Animator.RequestRunAnim();
			
			if (TargetFinder.IsHasTarget)
			{
				Mover.NavMeshMove(TargetFinder.GetCurrentTargetPosition());

				if (!Mover.IsProcessMovement && Mover.IsMovedLastFrame)
				{
					if (Mover.IsMovingToTarget)
						RequestTransition(AttackState);
					else
						RequestTransition(IdleState);
				}
			}
			else
			{
				RequestTransition(IdleState);
			}
		}
	}
}