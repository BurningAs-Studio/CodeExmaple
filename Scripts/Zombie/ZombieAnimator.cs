using UnityEngine;
using Zenject;

namespace FAS.Zombies
{
	public class ZombieAnimator : MonoBehaviour
	{
		[SerializeField] private float _changeLocomotionSpeed = 1f;
		
		[Inject] private Animator _animator;
		
		public AnimatorStateInfo DamageLayerInfo { get; private set; }
		public AnimatorStateInfo BaseLayerInfo { get; private set; }
		
		public Quaternion DeltaRotation => _animator.deltaRotation;
		public Vector3 DeltaPosition => _animator.deltaPosition;
		public Vector3 Velocity => _animator.velocity;

		private float _currentLocomotionValue;
		private float _targetLocomotionValue;

		private readonly int _locomotionValueAnimHash = Animator.StringToHash("locomotion");
		private readonly int _deathTypeValueAnimHash = Animator.StringToHash("deathType");
		private readonly int _isAttackBoolAnimHash = Animator.StringToHash("isAttack");
		private readonly int _deathTriggerAnimHash = Animator.StringToHash("death");
		
		private readonly int _takeDamageAnimHash = Animator.StringToHash("Take Damage");
		private readonly int _emptyAnimHash = Animator.StringToHash("Empty");
		
		private const int BASE_LAYER = 0;
		private const int DAMAGE_LAYER = 1;
		
		private const int DEATH_FORWARD_TYPE_INDEX = 0;
		private const int DEATH_BACKWARD_TYPE_INDEX = 1;
		private const int DEATH_RIGHT_TYPE_INDEX = 2;
		private const int DEATH_LEFT_TYPE_INDEX = 3;

		private const float WALK_LOCOMOTION_VALUE = 0.5f;
		private const float RUN_LOCOMOTION_VALUE = 1f;

		public void PlayTakeDamageAnim() => _animator.Play(_takeDamageAnimHash, DAMAGE_LAYER, 0f);

		public void PlayDeathForwardAnim() => PlayDeathAnim(DEATH_FORWARD_TYPE_INDEX);

		public void PlayDeathBackwardAnim() => PlayDeathAnim(DEATH_BACKWARD_TYPE_INDEX);
		
		public void PlayDeathRightAnim() => PlayDeathAnim(DEATH_RIGHT_TYPE_INDEX);
		
		public void PlayDeathLeftAnim() => PlayDeathAnim(DEATH_LEFT_TYPE_INDEX);

		private void PlayDeathAnim(int animType)
		{
			_animator.SetInteger(_deathTypeValueAnimHash, animType);
			_animator.SetTrigger(_deathTriggerAnimHash);
		}

		public void RequestWalkAnim() => _targetLocomotionValue = WALK_LOCOMOTION_VALUE;
		
		public void RequestRunAnim() => _targetLocomotionValue = RUN_LOCOMOTION_VALUE;
		
		public bool IsLayerActive(AnimatorStateInfo info) => info.shortNameHash != _emptyAnimHash;
		
		public void PlayAttackAnim() => _animator.SetBool(_isAttackBoolAnimHash, true);

		public void StopAttackAnim() => _animator.SetBool(_isAttackBoolAnimHash, false);

		public void Restart() => _animator.Rebind();

		private void Update()
		{
			DamageLayerInfo = _animator.GetCurrentAnimatorStateInfo(DAMAGE_LAYER);
			BaseLayerInfo = _animator.GetCurrentAnimatorStateInfo(BASE_LAYER);

			if (_currentLocomotionValue != _targetLocomotionValue)
			{
				_currentLocomotionValue = Mathf.MoveTowards(
					_currentLocomotionValue, _targetLocomotionValue, _changeLocomotionSpeed * Time.deltaTime);

				_animator.SetFloat(_locomotionValueAnimHash, _currentLocomotionValue);
			}
			
			_targetLocomotionValue = 0;
		}
	}
}