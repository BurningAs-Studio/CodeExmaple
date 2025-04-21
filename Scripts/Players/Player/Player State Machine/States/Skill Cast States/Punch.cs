using UnityEngine;
using Zenject;

namespace FAS.Players.States
{
	public class Punch : SkillCastState
	{
		[SerializeField] private float _moveSpeed = 1f;
		[SerializeField] private float _punchForce = 300f;

		[Inject] private HitEffectsPool _hitEffectsPool;
		[Inject] private PunchEffectTrigger _trigger;
		
		private readonly Vector3 _punchEffectOffset = Vector3.up * 1.5f;

		private const float STOP_PUNCH_ANIM_NORMALIZED_TIME = 0.9f;

		private void OnEnable()
		{
			_trigger.OnTriggerKickable += PhysicalPunch;
			_trigger.OnTriggerDamageable += DealDamage;
		}

		private void OnDisable()
		{
			_trigger.OnTriggerKickable -= PhysicalPunch;
			_trigger.OnTriggerDamageable -= DealDamage;
		}

		public override void Enter()
		{
			base.Enter();
			AnimEvents.OnPunch += OnPunch;
			Animator.PlayPunchAnim();
			VisualEffects.PlayPunchBuffEffect();
		}
		
		private void PhysicalPunch(IKickable kickable)
		{
			kickable.DeathKick(transform.position, Vector3.zero, _punchForce);
			_hitEffectsPool.Get().PlayPunchHitEffect(kickable.Position + _punchEffectOffset);
		}

		private void OnPunch()
		{
			CameraShaker.PlayPunchImpulse();
			VisualEffects.PlayPunchEffect();
		}
		
		private void DealDamage(DamageableCollider damageable)
		{
			if (!DamageReceiver.IsSelfCollider(damageable))
				damageable.TakeDamage(float.MaxValue, DamageReceiver);
		}

		public override void Perform()
		{
			base.Perform();

			if (Animator.SkillCastLayer.IsActive
			    && Animator.SkillCastLayer.CurrentAnimNTime > STOP_PUNCH_ANIM_NORMALIZED_TIME)
				RequestTransition(UnarmedState);
			
			Mover.MoveForward(_moveSpeed);
		}

		public override void Exit()
		{
			Animator.StopPunchAnim();
			AnimEvents.OnPunch -= OnPunch;
			base.Exit();
		}
	}
}