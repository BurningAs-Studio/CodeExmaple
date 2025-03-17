using UnityEngine;

namespace FAS.Players.States
{
	public class Kick : SkillCastState
	{
		[SerializeField] private float _moveSpeedMultiplier = 0.2f;
		[SerializeField] private float _kickingTime = 2f;
		[SerializeField] private float _sphereDamageDelay = 0.2f;
		[SerializeField] private float _kickRadius = 5f;
		[SerializeField] private int _kickDamage = 100;
		[SerializeField] private LayerMask _damageableLayers;
		[SerializeField] private int _maxColliders = 50;
		[SerializeField] private LayerMask _obstacleLayers;

		private Collider[] _colliderBuffer;

		private float _nextTimeSphereDamage;
		private float _timeToChangeState;

		private void Awake()
		{
			_colliderBuffer = new Collider[_maxColliders];
		}

		public override void Enter()
		{
			Animator.PlayKickAnim();
			VisualEffects.PlayKickEffect();
			_timeToChangeState = Time.timeSinceLevelLoad + _kickingTime;
			base.Enter();
		}

		public override void Perform()
		{
			base.Perform();
			
			if (Time.timeSinceLevelLoad > _timeToChangeState)
				RequestTransition(IdleState);
			
			Mover.SetSpeedMultiplier(_moveSpeedMultiplier);

			if (GroundChecker.IsGrounded)
				Jump.CheckAndRestVelocity();
			else
				Jump.FreeFall();
			
			Jump.ApplyGravity();

			if (Input.IsMovementJoystickActive)
				Mover.ManualMovement();
			
			if (Time.timeSinceLevelLoad > _nextTimeSphereDamage)
				TakeSphereDamage();
		}

		private void TakeSphereDamage()
		{
			int hits = Physics.OverlapSphereNonAlloc(
				transform.position + Vector3.up, _kickRadius, _colliderBuffer, _damageableLayers);

			for (int i = 0; i < hits; i++)
			{
				if (_colliderBuffer[i].TryGetComponent(out DamageableCollider damageable) && 
				    (damageable.Type == DamageableType.Generic || damageable.BodyPart == BodyPart.Head))
				{
					var targetPosition = _colliderBuffer[i].transform.position + Vector3.up;

					var isBlocked = 
						Physics.Linecast(transform.position + Vector3.up, targetPosition, _obstacleLayers);

					if (!isBlocked)
						damageable.TakeDamage(_kickDamage, DamageReceiver);
				}
			}
		}

		public override void Exit()
		{
			base.Exit();
			VisualEffects.StopKickEffect();
			Animator.StopKickAnim();
		}
	}
}