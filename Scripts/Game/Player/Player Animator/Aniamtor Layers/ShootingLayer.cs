using UnityEngine;

namespace FAS.Players.Animations
{
	public class ShootingLayer : Layer
	{
		private readonly int _pistolShootAnimHash = Animator.StringToHash("Pistol Shoot");
		private readonly int _rifleShootAnimHash = Animator.StringToHash("Rifle Shoot");
		private readonly int _isAimingBool = Animator.StringToHash("isAiming");

		public ShootingLayer(Animator animator, int index) : base(animator, index)
		{
		}
		
		public void SetAimingState(bool state) => Animator.SetBool(_isAimingBool, state);

		public void PlayEmptyAnim() => Animator.Play(PlayerAnimator.EmptyAnimHash, Index, 0);

		public void PlayPistolShootAnim() => Animator.Play(_pistolShootAnimHash, Index, 0);
		
		public void PlayRifleShootAnim() => Animator.Play(_rifleShootAnimHash, Index, 0);
	}
}