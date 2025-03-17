using UnityEngine;

namespace FAS.Players.Animations
{
	public class DeathLayer : Layer
	{
		public DeathLayer(Animator animator, int index) : base(animator, index)
		{
		}
		
		private readonly int _pistolDeathAnimHash = Animator.StringToHash("Pistol Death");
		private readonly int _rifleDeathAnimHash = Animator.StringToHash("Rifle Death");
		
		public void PlayPistolDeathAnim() => Animator.Play(_pistolDeathAnimHash);
		
		public void PlayRifleDeathAnim() => Animator.Play(_rifleDeathAnimHash);
	}
}