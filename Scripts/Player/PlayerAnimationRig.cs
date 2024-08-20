using UnityEngine.Animations.Rigging;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace PetWorld.Player
{
	public class PlayerAnimationRig : MonoBehaviour
	{
		[SerializeField] private PlayerBonesHolder _bones;
		[SerializeField] private float _enableRightHandSpeed = 25f;
		[SerializeField] private float _disableRightHandSpeed = 5f;
		[SerializeField] private Transform _torsoRotateTarget;

		[Space(5)]
		[SerializeField] private Rig _handsRig;
		[Space(10)]
		[Header("Left Hand Rig")]
		[SerializeField] private ChainIKConstraint _leftHandRig;
		[SerializeField] private Transform _leftHandTarget;

		[Space(10)]
		[Header("Right Hand Rig")]
		[SerializeField] private ChainIKConstraint _rightHandRig;
		[SerializeField] private Transform _rightHandTarget;

		[Space(10)]
		[Header("Torso Rig")]
		[SerializeField] private Rig _torsoRig;
		[SerializeField] private Rig _firingSkeletonTorsoRig;

		[Inject] private ShootingAnimatorLayer _shootingAnimatorLayer;
		[Inject] private IWeaponHolder _weaponHolder;

		private Transform _currentLeftHandTarget;
		private Transform _lastLeftHandTarget;

		private Tween _changeFiringSkeletonTorsoWeightTween;
		private Tween _changeRightHandWeightTween;
		private Tween _changeLeftHandWeightTween;
		private Tween _changeHandsWeightTween;
		private Tween _changeTorsoWeightTween;

		private bool _isHandsRigLastState;
		private bool _isLeftHandRigLastState;
		private bool _isTorsoRigLastState;

		private const float CHANGE_HANDS_RIG_WEIGHT_DURATION = 0.2f;
		private const float CHANGE_TORSO_RIG_WEIGHT_DURATION = 0.2f;
		private const float ENABLE_LEFT_HAND_RIG_DELAY = 0.1f;
		
		public bool IsTorsoRigEnabled => Mathf.Approximately(_torsoRig.weight, 1);
		
		public void LeftHandToTarget(Transform target)
		{
			EnableHandsRig();
			_currentLeftHandTarget = target;
			EnableLeftHandRig();
		}
		
		private void EnableHandsRig()
		{
			_changeHandsWeightTween.Kill();
			_changeHandsWeightTween = DOVirtual.Float(_handsRig.weight, 1, CHANGE_HANDS_RIG_WEIGHT_DURATION,
				weight => _handsRig.weight = weight)
				.SetEase(Ease.Flash);
		}

		private void EnableLeftHandRig()
		{
			_changeLeftHandWeightTween.Kill();
			_changeLeftHandWeightTween = DOVirtual.Float(_leftHandRig.weight, 1, CHANGE_HANDS_RIG_WEIGHT_DURATION,
					weight => _leftHandRig.weight = weight)
				.SetDelay(ENABLE_LEFT_HAND_RIG_DELAY)
				.SetEase(Ease.Flash);
		}

		public void DisableHandsRig()
		{
			_changeHandsWeightTween.Kill();
			_changeHandsWeightTween = DOVirtual.Float(_handsRig.weight, 0, CHANGE_HANDS_RIG_WEIGHT_DURATION,
					weight => _handsRig.weight = weight)
				.SetEase(Ease.Flash);

			_changeLeftHandWeightTween.Kill();
			_changeLeftHandWeightTween = DOVirtual.Float(_leftHandRig.weight, 0, CHANGE_HANDS_RIG_WEIGHT_DURATION,
					weight => _leftHandRig.weight = weight)
				.SetEase(Ease.Flash);

			_changeRightHandWeightTween.Kill();
			_changeRightHandWeightTween = DOVirtual.Float(_rightHandRig.weight, 0, CHANGE_HANDS_RIG_WEIGHT_DURATION,
					weight => _rightHandRig.weight = weight)
				.SetEase(Ease.Flash);
		}

		public void EnableTorsoRig()
		{
			_changeTorsoWeightTween.Kill();
			_changeTorsoWeightTween = DOVirtual.Float(_torsoRig.weight, 1, CHANGE_TORSO_RIG_WEIGHT_DURATION,
					weight => _torsoRig.weight = weight)
				.SetEase(Ease.Flash);

			_changeFiringSkeletonTorsoWeightTween.Kill();
			_changeFiringSkeletonTorsoWeightTween = DOVirtual.Float(_firingSkeletonTorsoRig.weight, 1, CHANGE_TORSO_RIG_WEIGHT_DURATION,
					weight => _firingSkeletonTorsoRig.weight = weight)
				.SetEase(Ease.Flash);
		}

		public void DisableTorsoRig()
		{
			_changeTorsoWeightTween.Kill();
			_changeTorsoWeightTween = DOVirtual.Float(_torsoRig.weight, 0, CHANGE_TORSO_RIG_WEIGHT_DURATION,
					weight => _torsoRig.weight = weight)
				.SetEase(Ease.Flash);

			_changeFiringSkeletonTorsoWeightTween.Kill();
			_changeFiringSkeletonTorsoWeightTween = DOVirtual.Float(_firingSkeletonTorsoRig.weight, 0, CHANGE_TORSO_RIG_WEIGHT_DURATION,
					weight => _firingSkeletonTorsoRig.weight = weight)
				.SetEase(Ease.Flash);
		}

		public void DisableAllRigs()
		{
			DisableTorsoRig();
			DisableHandsRig();
		}

		private void Update()
		{
			if (_handsRig.weight > 0)
			{
				if (_shootingAnimatorLayer.IsActive)
				{
					if (_rightHandRig.weight < 1)
						_rightHandRig.weight = Mathf.MoveTowards(
							_rightHandRig.weight, 1, _weaponHolder.Data.EnableRightHandRigSpeed * Time.deltaTime);

					_rightHandTarget.SetPositionAndRotation(_bones.RightHand.position, _bones.RightHand.rotation);
				}
				else if (_rightHandRig.weight > 0)
				{
					_rightHandRig.weight = Mathf.MoveTowards(
						_rightHandRig.weight, 0, _disableRightHandSpeed * Time.deltaTime);

					if (_rightHandRig.weight <= 0)
						_shootingAnimatorLayer.ResetFiringAnim();
				}

				if (_currentLeftHandTarget != null)
					_leftHandTarget.SetPositionAndRotation(_currentLeftHandTarget.position, _currentLeftHandTarget.rotation);
				else
					_leftHandTarget.SetPositionAndRotation(_bones.LeftHand.position, _bones.LeftHand.rotation);
			}

		}
	}
}