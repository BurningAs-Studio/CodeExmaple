using UnityEngine;

namespace PetWorld.Player
{
    public class PlayerBonesHolder : MonoBehaviour
    {
        [SerializeField] private Transform _hips;
        [SerializeField] private Transform _leftUpLeg;
        [SerializeField] private Transform _leftLeg;
        [SerializeField] private Transform _leftFoot;
        [SerializeField] private Transform _rightUpLeg;
        [SerializeField] private Transform _rightLeg;
        [SerializeField] private Transform _RightFoot;
        [SerializeField] private Transform _spine;
        [SerializeField] private Transform _spine1;
        [SerializeField] private Transform _spine2;
        [SerializeField] private Transform _neck;
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _leftShoulder;
        [SerializeField] private Transform _leftArm;
        [SerializeField] private Transform _leftForeArm;
        [SerializeField] private Transform _leftHand;
        [SerializeField] private Transform _rightShoulder;
        [SerializeField] private Transform _rightArm;
        [SerializeField] private Transform _rightForeArm;
        [SerializeField] private Transform _rightHand;

        public Transform Hips => _hips;
        public Transform LeftUpLeg => _leftUpLeg;
        public Transform LeftLeg => _leftLeg;
        public Transform LeftFoot => _leftFoot;
        public Transform RightUpLeg => _rightUpLeg;
        public Transform RightLeg => _rightLeg;
        public Transform RightFoot => _RightFoot;
        public Transform Spine => _spine;
        public Transform Spine1 => _spine1;
        public Transform Spine2 => _spine2;
        public Transform Neck => _neck;
        public Transform Head => _head;
        public Transform LeftShoulder => _leftShoulder;
        public Transform LeftArm => _leftArm;
        public Transform LeftForeArm => _leftForeArm;
        public Transform LeftHand => _leftHand;
        public Transform RightShoulder => _rightShoulder;
        public Transform RightArm => _rightArm;
        public Transform RightForeArm => _rightForeArm;
        public Transform RightHand => _rightHand;
    }
}