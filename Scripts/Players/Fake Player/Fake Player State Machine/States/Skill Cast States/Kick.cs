using UnityEngine;
using Zenject;

namespace FAS.FakePlayers.States
{
	public class Kick : SkillCastState
	{
		[SerializeField] private float _moveSpeedMultiplier = 0.2f;
		[SerializeField] private float _kickingTime = 2f;
		[SerializeField] private float _kickForce = 1000f;
		[SerializeField] private Vector3 _kickDirectionOffset = new (0, 0.5f, 0);

		[Inject] private HitEffectsPool _hitEffectsPool;
		[Inject] private KickTrigger _trigger;
		
		private readonly Vector3 _kickEffectOffset = Vector3.up * 1.5f;
		
		private float _timeToChangeState;
		
		private void OnEnable()
		{
			_trigger.OnTriggerKickable += PhysicalKick;
			_trigger.OnTriggerDamageable += TakeDamage;
		}

		private void OnDisable()
		{
			_trigger.OnTriggerKickable -= PhysicalKick;
			_trigger.OnTriggerDamageable -= TakeDamage;
		}

		public bool IsTargetInRange(Vector3 targetPosition)
			=> Vector3.SqrMagnitude(transform.position - targetPosition) <= _trigger.Radius * _trigger.Radius;

		public override void Enter()
		{
			base.Enter();
			Animator.PlayKickAnim();
			VisualEffects.PlayKickEffect();
			_trigger.Enable();
			_timeToChangeState = Time.timeSinceLevelLoad + _kickingTime;
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
		}
		
		private void PhysicalKick(IKickable kickable)
		{
			kickable.Kick(transform.position, _kickDirectionOffset, _kickForce);
			_hitEffectsPool.Get().PlayPunchHitEffect(kickable.Position + _kickEffectOffset);
		}

		private void TakeDamage(DamageableCollider damageable)
			=> damageable.TakeDamage(float.MaxValue, DamageReceiver);

		public override void Exit()
		{
			VisualEffects.StopKickEffect();
			Animator.StopKickAnim();
			_trigger.Disable();
			base.Exit();
		}
	}
}