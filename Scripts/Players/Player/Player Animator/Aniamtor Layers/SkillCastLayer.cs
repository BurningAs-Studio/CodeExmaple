using UnityEngine;

namespace FAS.Players.Animations
{
	public class SkillCastLayer : Layer
	{
		private readonly int _isPunchingBoolAnimHash = Animator.StringToHash("isPunching");
		private readonly int _isKickingBoolAnimHash = Animator.StringToHash("isKicking");
		
		public SkillCastLayer(Animator animator, int index) : base(animator, index)
		{
		}
		
		public void PlayPunchAnim() => Animator.SetBool(_isPunchingBoolAnimHash, true);
		
		public void StopPunchAnim() => Animator.SetBool(_isPunchingBoolAnimHash, false);
		
		public void PlayKickAnim() => Animator.SetBool(_isKickingBoolAnimHash, true);
		
		public void StopKickAnim() => Animator.SetBool(_isKickingBoolAnimHash, false);
	}
}