using RootMotion.Dynamics;
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
			_spawnPosition = _respawnPoints.GetEnemyPosition();
			Mover.SetStoppingDistance(0);
		}

		public override void Perform()
		{
			Mover.RequestEnableRootMotion();
			Mover.Model.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

			if (PuppetMasterHandler.CurrentMode != PuppetMaster.Mode.Active)
			{
				if (Vector3.SqrMagnitude(transform.position - _spawnPosition) > 0.1f)
					Mover.RequestTeleport(_spawnPosition);
				else
					RequestTransition(IdleState);	
			}
		}

		public override void Exit()
		{
			TargetFinder.LoseTarget();
			Health.TryHeal(float.MaxValue);
			KickReceiver.SetReadyToKick();
			DamageReceiver.EnableDamageableColliders();
			Mover.TryStopMove();
			Animator.Restart();
			IsReadyToTransit = false;
		}
	}
}