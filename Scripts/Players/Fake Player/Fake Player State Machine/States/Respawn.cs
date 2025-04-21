using RootMotion.Dynamics;
using FAS.Players;
using UnityEngine;
using Zenject;

namespace FAS.FakePlayers.States
{
	public class Respawn : FakePlayerState
	{
		[Inject] private RespawnPointsHolder _respawnPoints;
		[Inject] private PlayerHead _head;
		
		private Vector3 _spawnPosition;

		public override void Enter()
		{
			_spawnPosition = _respawnPoints.GetEnemyPosition();
			RequestTransition(IdleState);
			Mover.SetStoppingDistance(0);
			_head.TakeOn();
		}

		public override void Perform()
		{
			Mover.RequestDisableNavMesh();
			Body.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

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
			Health.TryHeal(float.MaxValue);
			DamageReceiver.EnableDamageableColliders();
			KickReceiver.SetReadyToKick();
			Pickup.Enable();
			Mover.TryStopMove();
			Animator.Restart();
			base.Exit();
		}
	}
}