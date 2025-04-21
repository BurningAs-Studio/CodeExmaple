using UnityEngine;

namespace FAS.Players.Animations
{
	public class BaseLayer : Layer
	{
        private readonly int _horizontalLocomotionValue = Animator.StringToHash("horizontalLocomotion");
        private readonly int _verticalLocomotionValue = Animator.StringToHash("verticalLocomotion");
        private readonly int _locomotionValue = Animator.StringToHash("locomotion");
        
        public BaseLayer(Animator animator, int index) : base(animator, index)
        {
        }

        public void SetLocomotionValue(float value) => Animator.SetFloat(_locomotionValue, value);
        
        public void SetLocomotionValue(Vector2 vector)
        {
	        Animator.SetFloat(_horizontalLocomotionValue, vector.y);
	        Animator.SetFloat(_verticalLocomotionValue, vector.x);
        }
	}
}