using UnityEngine;

namespace FAS.Players.Animations
{
	public class BaseFiringAnimatorLayer : Layer
	{
		private readonly int AimIdle = Animator.StringToHash("Axe Aim Idle");
		
		public BaseFiringAnimatorLayer(Animator animator, int index) : base(animator, index)
		{
		}

		public void PlayAimIdleAnim() => PlayAnim(AimIdle);
	}
}