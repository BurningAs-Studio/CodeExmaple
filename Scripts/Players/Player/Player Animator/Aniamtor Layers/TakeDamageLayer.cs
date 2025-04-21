using UnityEngine;

namespace FAS.Players.Animations
{
	public class TakeDamageLayer : Layer
	{
		private readonly int _takeDamageAnimHash = Animator.StringToHash("Take Damage");
		
		public TakeDamageLayer(Animator animator, int index) : base(animator, index)
		{
		}

		public void PlayTakeDamageAnim()
		{
			Animator.Play(_takeDamageAnimHash, Index, 0f);
		}
	}
}