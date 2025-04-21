namespace FAS.Players.States
{
	public class ReceiveFatality : PlayerState
	{
		// [Inject] private HitEffectsPool _hitEffectsPool;
		// [Inject] private PlayerHead _head;
		//
		// private bool _isWaitingForEnableBloodFlowingEffect;
		// private bool _isFrameSkipped;
		//
		// private float _nextTimeToEnableBloodFlowingEffect;
		//
		// private const float ENABLE_BLOOD_FLOWING_EFFECT_DELAY = 1f;
		
		public override void Enter()
		{
			// _isFrameSkipped = false;
			// FatalityTarget.OnPerformFatality += PerformFatality;
			// FatalityTarget.OnFinishFatality += OnDeathComplete;
			// DamageReceiver.DisableDamageableColliders();
			// Animator.SetLocomotionValue(0);
			// Mover.RequestTeleport(FatalityTarget.CurrentData.Position);
		}

		public override void Perform()
		{
			
		}
		//
		// private void PerformFatality()
		// {
		// 	Animator.PlayReceiveFatalityAnim(FatalityTarget.CurrentData.Type);
		// 	VisualEffects.PlayBloodShowerEffect();
		// 	_nextTimeToEnableBloodFlowingEffect = Time.timeSinceLevelLoad + ENABLE_BLOOD_FLOWING_EFFECT_DELAY;
		// 	_isWaitingForEnableBloodFlowingEffect = true;
		// 	_head.FatalityTakeOff();
		// 	_hitEffectsPool.Get().PlayFatalityTornadoKickHitEffect(_head.transform.position);
		// }
		//
		// protected override void PerformDeath()
		// {
		// 	if (_isFrameSkipped)
		// 	{
		// 		Mover.RequestTransformMove(FatalityTarget.CurrentData.Position, 999);
		// 		Rotator.RequestRotate(FatalityTarget.CurrentData.Rotation);
		//
		// 		if (_isWaitingForEnableBloodFlowingEffect
		// 		    && Time.timeSinceLevelLoad > _nextTimeToEnableBloodFlowingEffect)
		// 		{
		// 			VisualEffects.PlayBloodFlowingEffect();
		// 			_isWaitingForEnableBloodFlowingEffect = false;
		// 		}
		// 	}
		// 	else
		// 	{
		// 		_isFrameSkipped = true;
		// 	}
		// }

		public override void Exit()
		{
			// VisualEffects.StopBloodFlowingEffect();
			// VisualEffects.StopBloodShowerEffect();
			// FatalityTarget.OnFinishFatality -= OnDeathComplete;
			// FatalityTarget.OnPerformFatality -= PerformFatality;
			base.Exit();
		}
	}
}