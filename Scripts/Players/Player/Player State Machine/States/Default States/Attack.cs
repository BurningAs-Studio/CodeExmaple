using FAS.Weapons;

namespace FAS.Players.States
{
	public class Attack : DefaultState
	{
		public override void Enter()
		{
			Animator.RequestEnableAttackLayer();

			var currentWeapon = Weapon.CurrentWeapon;
			if (currentWeapon.AnimType == WeaponAnimType.ThrowMeleeWeapon)
				Animator.PlayThrowMeleeWeaponAnim();
			
			CameraShaker.PlayImpulse(currentWeapon.CameraImpulse);
			
			if (currentWeapon.TryGetVignetteData(out VignetteData vignetteData))
				Vignette.Play(vignetteData);
		}

		public override void Perform()
		{
			CameraRotator.RequestDisable();
			Animator.RequestEnableAttackLayer();
			
			if (Animator.AttackLayer.IsActive
			    && Animator.AttackLayer.IsEnabled
			    && Animator.AttackLayer.CurrentAnimNTime > 0.9f)
				RequestTransition(ArmedState);
		}
	}
}