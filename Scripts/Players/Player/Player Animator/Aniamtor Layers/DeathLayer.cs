using UnityEngine;

namespace FAS.Players.Animations
{
	public class DeathLayer : Layer
	{
		private readonly int _deathTypeValueAnimHash = Animator.StringToHash("deathType");
		private readonly int _deathTriggerAnimHash = Animator.StringToHash("death");

		public DeathLayer(Animator animator, int index) : base(animator, index)
		{
		}
		
		public void PlayDeathAnim(int animType)
		{
			Animator.SetInteger(_deathTypeValueAnimHash, animType);
			Animator.SetTrigger(_deathTriggerAnimHash);
		}
	}
}