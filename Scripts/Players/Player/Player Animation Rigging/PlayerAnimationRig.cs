using UnityEngine.Animations.Rigging;
using FAS.Players.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Players.AnimRig
{
	public class PlayerAnimationRig : MonoBehaviour
	{
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
		
		[Space(10)]
		[Header("Head Rig")]
		[SerializeField] private Rig _head;
		[SerializeField] private Transform _headTarget;

		[Inject(Id = BonesSkeletonType.Firing)] private BonesHolder _bones;
		[Inject] private PlayerAnimator _animator;

		private readonly AnimRigWeightData _rightHandWeightData = new ();
		private readonly AnimRigWeightData _leftHandWeightData = new ();
		private readonly AnimRigWeightData _handsWeightData = new ();
		private readonly AnimRigWeightData _spineWeightData = new ();
		private readonly AnimRigWeightData _headWeightData = new ();
		
		private Transform _currentRightHandTarget;
		private Transform _currentLeftHandTarget;
		
		public void SetHeadTargetRotation(Vector3 rotation) => _headTarget.localRotation = Quaternion.Euler(rotation);

		public Quaternion GetHeadTargetRotation() => _headTarget.localRotation;

		public void RequestEnableSpine() => _spineWeightData.RequestEnable();

		public void RequestEnableHands() => _handsWeightData.RequestEnable();

		public void RequestEnableHead() => _headWeightData.RequestEnable();
		
		public void RequestEnableRightHand(float weight = 1, Transform target = null)
		{
			_currentRightHandTarget = target;
			_rightHandWeightData.RequestEnable(weight);
		}

		public void RequestEnableLeftHand(float weight = 1, Transform target = null)
		{
			_currentLeftHandTarget = target;
			_leftHandWeightData.RequestEnable(weight);
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
			
			if (_headWeightData.IsWeightUpdated(out var headWeight))
				_head.weight = headWeight;
			
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
		}
	}
}
