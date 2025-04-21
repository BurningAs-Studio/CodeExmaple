using UnityEngine;

namespace FAS.Players.Animations
{
	public class JumpLayer : Layer
	{
		private readonly int _isGroundedBoolAnimHash = Animator.StringToHash("isGrounded");
		private readonly int _isFreeFallBoolAnimHash = Animator.StringToHash("isFreeFall");
		private readonly int _isJumpBoolAnimHash = Animator.StringToHash("isJump");
		
		public JumpLayer(Animator animator, int index) : base(animator, index)
		{
		}
		
		public void IsGrounded(bool state) => Animator.SetBool(_isGroundedBoolAnimHash, state);

		public void IsFreeFall(bool state) => Animator.SetBool(_isFreeFallBoolAnimHash, state);
		
		public void StopJumpAnim() => Animator.SetBool(_isJumpBoolAnimHash, false);
		
		public void PlayJumpAnim() => Animator.SetBool(_isJumpBoolAnimHash, true);
	}
}