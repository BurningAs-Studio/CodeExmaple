using RootMotion.Dynamics;
using UnityEngine;

namespace FAS.Zombies.States
{
	public class PuppetDeath : DeathState
	{
		[SerializeField] private float _velocityThreshold = 0.1f;
		[SerializeField] private float _checkMovementInterval = 0.5f;
		[SerializeField] private float _maxTimeToRespawn = 10f;
		
		private float _nextCheckMovementTime;
		private float _nextRespawnTime;

		public override void Enter()
		{
			base.Enter();
			PuppetMasterHandler.SetBehaviourState(BehaviourPuppet.State.Unpinned);
			PuppetMasterHandler.SetState(PuppetMaster.State.Frozen);
			PuppetMasterHandler.DisableGetUpLogic();
			PuppetMasterHandler.SetPinWeight(0);
			_nextRespawnTime = Time.timeSinceLevelLoad + _maxTimeToRespawn;
		}

		protected override void PerformDeath()
		{
			Mover.RequestDisable();
			
			if (Time.timeSinceLevelLoad > _nextCheckMovementTime)
			{
				_nextCheckMovementTime = Time.timeSinceLevelLoad + _checkMovementInterval;
			
				if (HasStoppedMoving() || Time.timeSinceLevelLoad > _nextRespawnTime)
					OnDeathComplete();
			}
		}

		protected override void WaitAndMoveUnderground()
		{
			base.WaitAndMoveUnderground();
			var targetPosition = Mover.Model.position;
			targetPosition.y = transform.parent.position.y;
			Mover.Model.position = targetPosition;
		}
		
		private bool HasStoppedMoving()
		{
			for (var i = 0; i < PuppetMasterHandler.Muscles.Length; i++)
			{
				if (PuppetMasterHandler.Muscles[i].rigidbody.linearVelocity.sqrMagnitude
				    > _velocityThreshold)
					return false;
			}
			return true;
		}

		public override void Exit()
		{
			Mover.Model.SetParent(transform.parent, true);
			PuppetMasterHandler.PuppetTransform.SetParent(transform.parent, true);
			PuppetMasterHandler.Restore();
			PuppetMasterHandler.SetMode(PuppetMaster.Mode.Disabled);
			base.Exit();
		}
	}
}