using UnityEngine;

namespace FAS.Players.Animations
{
	public class ArmLayer : Layer
	{
		private readonly int _isEquippedBool = Animator.StringToHash("isEquipped");
		private readonly int _isArmedBool = Animator.StringToHash("isArmed");
		private readonly int _equipTrigger = Animator.StringToHash("equip");
		

		public ArmLayer(Animator animator, int index) : base(animator, index)
		{
		}
		
		public void SetUnequipped() => Animator.SetBool(_isEquippedBool, false);
		
		public void SetEquipped() => Animator.SetBool(_isEquippedBool, true);

		public void SetUnarmed()
		{
			Animator.SetBool(_isArmedBool, false);
			Animator.SetTrigger(_equipTrigger);
		}

		public void SetArmed()
		{
			Animator.SetBool(_isArmedBool, true);
			Animator.SetTrigger(_equipTrigger);
		}
	}
}