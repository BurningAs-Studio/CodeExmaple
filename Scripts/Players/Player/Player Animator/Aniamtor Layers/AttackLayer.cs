using UnityEngine;

namespace FAS.Players.Animations
{
	public class AttackLayer : Layer
	{
		private readonly int _throwMeleeWeaponAnimHash = Animator.StringToHash("Throw Melee Weapon");
		
		public AttackLayer(Animator animator, int index) : base(animator, index)
		{
		}

		public void PlayThrowMeleeWeaponAnim() => PlayAnim(_throwMeleeWeaponAnimHash);
	}
}