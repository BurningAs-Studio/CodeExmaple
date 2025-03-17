using UnityEngine;
using Zenject;

namespace FAS.Zombies.States
{
	public class Respawn : ZombieState
	{
		[Inject] private RespawnPointsHolder _respawnPoints;
		
		private Vector3 _spawnPosition;
		
		public override void Enter()
		{
			base.Enter();
			_spawnPosition = _respawnPoints.GetEnemyPosition();
			Mover.SetStoppingDistance(0);
			RequestTransition(IdleState);
		}

		public override void Perform()
		{
			Mover.RequestEnableRootMotion();

			if (Vector3.SqrMagnitude(transform.position - _spawnPosition) > 0.1f)
				Mover.RequestTeleport(_spawnPosition);
			else
				RequestTransition(IdleState);
		}

		public override void Exit()
		{
			TargetFinder.LoseTarget();
			Health.TryHeal(999);
			DamageReceiver.EnableDamageableColliders();
			Mover.StopMove();
			Animator.Restart();
		}
	}
}