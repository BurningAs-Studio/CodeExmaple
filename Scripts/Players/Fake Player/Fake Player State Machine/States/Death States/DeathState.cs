using UnityEngine;

namespace FAS.FakePlayers.States
{
	public abstract class DeathState : FakePlayerState
	{
		[SerializeField] private float _moveUndergroundSpeed = 1f;
		[SerializeField] private float _moveUndergroundDelay = 3f;

		private Vector3 _undergroundPosition;
		
		private float _nextTimeMoveUnderground;
		
		private bool _isDeathComplete;
		
		private const float UNDERGROUND_OFFSET = 5f;
		
		public override void Enter()
		{
			Pickup.Disable();
			DamageReceiver.DisableDamageableColliders();
			Animator.SetLocomotionValue(0);
		}

		protected virtual void OnDeathComplete()
		{
			_undergroundPosition = transform.position - Vector3.up * UNDERGROUND_OFFSET;
			_nextTimeMoveUnderground = Time.timeSinceLevelLoad + _moveUndergroundDelay;
			_isDeathComplete = true;
		}

		public override void Perform()
		{
			if (_isDeathComplete)
				WaitAndMoveUnderground();
			else
				PerformDeath();
		}

		protected virtual void WaitAndMoveUnderground()
		{
			if (Time.timeSinceLevelLoad > _nextTimeMoveUnderground)
			{
				Mover.RequestTransformMove(_undergroundPosition, _moveUndergroundSpeed);
					
				if (Vector3.SqrMagnitude(transform.position - _undergroundPosition) < 0.1f)
					RequestTransition(RespawnState);
			}
		}

		protected abstract void PerformDeath();

		public override void Exit()
		{
			_isDeathComplete = false;
			base.Exit();
		}
	}
}