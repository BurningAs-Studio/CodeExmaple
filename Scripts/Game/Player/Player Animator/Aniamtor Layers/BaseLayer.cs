using UnityEngine;

namespace FAS.Players.Animations
{
	public class BaseLayer : Layer
	{
        private readonly int _horizontalLocomotionValue = Animator.StringToHash("horizontalLocomotion");
        private readonly int _verticalLocomotionValue = Animator.StringToHash("verticalLocomotion");
        private readonly int _changeLocomotionTrigger = Animator.StringToHash("changeLocomotion");
        private readonly int _locomotionType = Animator.StringToHash("locomotionType");
        private readonly int _locomotionValue = Animator.StringToHash("locomotion");
        private readonly int _isGroundedBool = Animator.StringToHash("isGrounded");
        private readonly int _jumpAnimHash = Animator.StringToHash("jump");
        
        public BaseLayer(Animator animator, int index) : base(animator, index)
        {
        }

        public void SetLocomotionValue(float value) => Animator.SetFloat(_locomotionValue, value);
        
        public void SetLocomotionValue(Vector2 vector)
        {
	        Animator.SetFloat(_horizontalLocomotionValue, vector.y);
	        Animator.SetFloat(_verticalLocomotionValue, vector.x);
        }

        public void SetLocomotionType(int index)
        {
	        Animator.SetInteger(_locomotionType, index);
	        UpdateLocomotionType();
        }
        
        public void UpdateLocomotionType() => Animator.SetTrigger(_changeLocomotionTrigger);
        
        public void IsGrounded(bool state) => Animator.SetBool(_isGroundedBool, state);
        
        public void PlayJumpAnim() => Animator.SetTrigger(_jumpAnimHash);
	}
}