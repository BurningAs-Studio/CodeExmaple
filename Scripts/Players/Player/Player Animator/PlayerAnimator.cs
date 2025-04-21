using FAS.Players.AnimRig;
using FAS.Fatality;
using UnityEngine;
using Zenject;

namespace FAS.Players.Animations
{
	public enum AnimatorType
	{
		Firing,
		Main
	}
	
	public class PlayerAnimator : MonoBehaviour, ISpeedMultiplier
	{
		[Inject] private PlayerAnimationRig _animationRig;
		[Inject] private TakeDamageLayer _takeDamageLayer;
		[Inject] private SkillCastLayer _skillCastLayer;
		[Inject] private FatalityLayer _fatalityLayer;
		[Inject] private AttackLayer _attackLayer;
		[Inject] private PuppetLayer _puppetLayer;
		[Inject] private DeathLayer _deathLayer;
		[Inject] private Animator _mainAnimator;
		[Inject] private BaseLayer _baseLayer;
		[Inject] private JumpLayer _jumpLayer;
		[Inject] private ArmLayer _armLayer;
		
		private float _animatorSpeedMultiplier;
		
		private bool _isEnableAttackLayerRequested;
		
		public bool IsMultipliedThisFrame { get; private set; }
		
		public IReadOnlyAnimatorLayer SkillCastLayer => _skillCastLayer;
		public IReadOnlyAnimatorLayer FatalityLayer => _fatalityLayer;
		public IReadOnlyAnimatorLayer PuppetLayer => _puppetLayer;
		public IReadOnlyAnimatorLayer AttackLayer => _attackLayer;
		public IReadOnlyAnimatorLayer JumpLayer => _jumpLayer;
		public IReadOnlyAnimatorLayer BaseLayer => _baseLayer;
		public IReadOnlyAnimatorLayer ArmLayer => _armLayer;
		
		public static readonly int EmptyAnimHash = Animator.StringToHash("Empty");
		
		private const int DEATH_FORWARD_TYPE_INDEX = 0;
		private const int DEATH_BACKWARD_TYPE_INDEX = 1;
		private const int DEATH_RIGHT_TYPE_INDEX = 2;
		private const int DEATH_LEFT_TYPE_INDEX = 3;
		
		private const float DEFAULT_ANIMATOR_SPEED = 1.0f;

		public void PlayDeathBackwardAnim() => _deathLayer.PlayDeathAnim(DEATH_BACKWARD_TYPE_INDEX);

		public void PlayDeathForwardAnim() => _deathLayer.PlayDeathAnim(DEATH_FORWARD_TYPE_INDEX);
		
		public void PlayDeathRightAnim() => _deathLayer.PlayDeathAnim(DEATH_RIGHT_TYPE_INDEX);
		
		public void PlayDeathLeftAnim() => _deathLayer.PlayDeathAnim(DEATH_LEFT_TYPE_INDEX);
		
		public void PlayExecuteFatalityAnim(FatalityType type)
		{
			int animType;
			
			switch (type)
			{
				case FatalityType.TornadoKick:
				default:
					animType = 0;
					break;
			}
			
			_fatalityLayer.PlayExecuteFatalityAnim(animType);
		}
		
		public void PlayReceiveFatalityAnim(FatalityType type)
		{
			int animType;
			
			switch (type)
			{
				case FatalityType.TornadoKick:
				default:
					animType = 0;
					break;
			}
			
			_fatalityLayer.PlayReceiveFatalityAnim(animType);
		}

		public void RequestEnableAttackLayer() => _isEnableAttackLayerRequested = true;

		public void SetLocomotionValue(Vector2 value) => _baseLayer.SetLocomotionValue(value);
		
		public void SetLocomotionValue(float value) => _baseLayer.SetLocomotionValue(value);
		
		public void PlayThrowMeleeWeaponAnim() => _attackLayer.PlayThrowMeleeWeaponAnim();

		public void PlayTakeDamageAnim() => _takeDamageLayer.PlayTakeDamageAnim();
		
		public void PlayPunchAnim() => _skillCastLayer.PlayPunchAnim();
		
		public void StopPunchAnim() => _skillCastLayer.StopPunchAnim();

		public void PlayStandUpAnim() => _puppetLayer.PlayGetUpProne();
		
		public void PlayKickAnim() => _skillCastLayer.PlayKickAnim();
		
		public void StopKickAnim() => _skillCastLayer.StopKickAnim();
		
		public void SetUnequipped() => _armLayer.SetUnequipped();
		
		public void SetEquipped() => _armLayer.SetEquipped();
		
		public void SetUnarmed() => _armLayer.SetUnarmed();
		
		public void Restart() => _mainAnimator.Rebind();
		
		public void SetArmed() => _armLayer.SetArmed();
		
		private void UpdateAttackLayerWeight()
		{
			if (_isEnableAttackLayerRequested)
			{
				if (!_attackLayer.IsEnabled)
					_attackLayer.EnableWeightSmooth();
				
				_isEnableAttackLayerRequested = false;
			}
			else
			{
				_attackLayer.DisableWeightSmooth();
			}
		}
		
		private void UpdateSpeed() => _mainAnimator.speed = DEFAULT_ANIMATOR_SPEED * _animatorSpeedMultiplier;
		
		public void SetSpeedMultiplier(float multiplier)
		{
			IsMultipliedThisFrame = true;
			_animatorSpeedMultiplier = multiplier;
		}

		private void ResetSpeedMultiplier() => _animatorSpeedMultiplier = 1;
		
		private void Update()
		{
			UpdateAttackLayerWeight();
			UpdateSpeed();
		}

		private void LateUpdate()
		{
			if (IsMultipliedThisFrame)
				IsMultipliedThisFrame = false;
			else
				ResetSpeedMultiplier();
		}
	}
}