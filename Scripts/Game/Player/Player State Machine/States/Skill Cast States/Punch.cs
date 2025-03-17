using UnityEngine;
using Zenject;

namespace FAS.Players.States
{
	public class Punch : SkillCastState
	{
		[SerializeField] private int _damage = 999;
		[SerializeField] private float _moveSpeed = 1f;

		[Inject] private PunchEffectTrigger _trigger;
		
		private const float STOP_PUNCH_ANIM_NORMALIZED_TIME = 0.9f;

		private void OnEnable()
		{
			_trigger.OnTrigger += DealDamage;
		}

		private void OnDisable()
		{
			_trigger.OnTrigger -= DealDamage;
		}

		public override void Enter()
		{
			base.Enter();
			AnimEvents.OnPunch += OnPunch;
			Animator.PlayPunchAnim();
			VisualEffects.PlayPunchBuffEffect();
		}

		private void OnPunch()
		{
			CameraShaker.PlayPunchImpulse();
			VisualEffects.PlayPunchEffect();
		}
		
		private void DealDamage(DamageableCollider damageable)
		{
			if (!DamageReceiver.IsSelfCollider(damageable))
				damageable.TakeDamage(_damage, DamageReceiver);
		}

		public override void Perform()
		{
			base.Perform();

			if (Animator.SkillCastLayer.IsActive
			    && Animator.SkillCastLayer.CurrentAnimNTime > STOP_PUNCH_ANIM_NORMALIZED_TIME)
				RequestTransition(IdleState);
			
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