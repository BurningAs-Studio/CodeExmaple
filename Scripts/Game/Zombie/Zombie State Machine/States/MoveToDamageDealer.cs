using UnityEngine;

namespace FAS.Zombies.States
{
	public class MoveToDamageDealer : ZombieState
	{
		[SerializeField] private float _angularSpeed = 500f;
		
		public override void Enter()
		{
			base.Enter();
			Mover.NavMeshMove(DamageReceiver.LastDamageDealer.transform.position);
			Mover.SetStoppingDistance(0);
			Rotator.SetAutoAngularSpeed(_angularSpeed);
		}

		public override void Perform()
		{
			Animator.RequestRunAnim();

			if (TargetFinder.IsHasTarget)
				RequestTransition(ChaseState);
			else if (!Mover.IsProcessMovement && Mover.IsMovedLastFrame)
				RequestTransition(IdleState);
		}
	}
}