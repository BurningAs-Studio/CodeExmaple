using UnityEngine;

namespace FAS.Players.Animations
{
	public class PuppetLayer : Layer
	{
		private readonly int _getUpProneAnimHash = Animator.StringToHash("GetUpProne");
		
		public PuppetLayer(Animator animator, int index) : base(animator, index)
		{
		}

		public void PlayGetUpProne() => Animator.Play(_getUpProneAnimHash, Index, 0.5f);
	}
}