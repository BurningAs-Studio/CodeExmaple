using UnityEngine.Animations.Rigging;
using FAS.Players.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Players.AnimRig
{
	public class PlayerAnimationRig : MonoBehaviour
	{
		[SerializeField] private BonesHolder _bones;
		[SerializeField] private Transform _spineRotateTarget;

		[Space(5)]
		[SerializeField] private Rig _hands;

		[Space(10)]
		[Header("Left Hand Rig")]
		[SerializeField] private ChainIKConstraint _leftHand;
		[SerializeField] private Transform _leftHandTarget;


		[Space(10)]
		[Header("Right Hand Rig")]
		[SerializeField] private ChainIKConstraint _rightHand;
		[SerializeField] private Transform _rightHandTarget;

		[Space(10)]
		[Header("Torso Rig")]
		[SerializeField] private Rig _spine;
		[SerializeField] private Rig _firingSkeletonSpine;

		[Inject] private IReadOnlyPlayerWeapon _weapons;
		[Inject] private PlayerAnimator _animator;

		private readonly WeightData _rightHandWeightData = new ();
		private readonly WeightData _leftHandWeightData = new ();
		private readonly WeightData _handsWeightData = new ();
		private readonly WeightData _spineWeightData = new ();
		
		private Transform _currentRightHandTarget;
		private Transform _currentLeftHandTarget;
		
		public void RequestEnableSpine() => _spineWeightData.IsEnableRequested = true;

		public void RequestEnableHands() => _handsWeightData.IsEnableRequested = true;

		public void RequestEnableRightHand(Transform target = null)
		{
			_currentRightHandTarget = target;
			_rightHandWeightData.IsEnableRequested = true;
		}

		public void RequestEnableLeftHand(Transform target = null)
		{
			_currentLeftHandTarget = target;
			_leftHandWeightData.IsEnableRequested = true;
		}

		private void Update()
		{
			if (_spineWeightData.IsWeightUpdated(out var spineWeight))
			{
				_firingSkeletonSpine.weight = spineWeight;
				_spine.weight = spineWeight;
			}
			
			if (_handsWeightData.IsWeightUpdated(out var handsWeight))
				_hands.weight = handsWeight;
			
			var leftHandEnableWeightDuration = 0.1f;

			if (_weapons.IsHasWeapon)
				leftHandEnableWeightDuration = _weapons.Data.EnableLeftHandSpeed;
			
			if (_leftHandWeightData.IsWeightUpdated(out var leftHandWeight, leftHandEnableWeightDuration))
				_leftHand.weight = leftHandWeight;
			
			if (_rightHandWeightData.IsWeightUpdated(out var rightHandWeight))
				_rightHand.weight = rightHandWeight;

			if (_leftHand.weight > 0)
			{
				if (_currentLeftHandTarget != null)
					_leftHandTarget.SetPositionAndRotation(_currentLeftHandTarget.position, _currentLeftHandTarget.rotation);
				else
					_leftHandTarget.SetPositionAndRotation(_bones.LeftHand.position, _bones.LeftHand.rotation);
			}

			if (_rightHand.weight > 0)
			{
				if (_currentRightHandTarget != null)
					_rightHandTarget.SetPositionAndRotation(_currentRightHandTarget.position, _currentRightHandTarget.rotation);
				else
					_rightHandTarget.SetPositionAndRotation(_bones.RightHand.position, _bones.RightHand.rotation);
			}

			_rightHandWeightData.Reset();
			_leftHandWeightData.Reset();
			_handsWeightData.Reset();
			_spineWeightData.Reset();
		}
	}
}
