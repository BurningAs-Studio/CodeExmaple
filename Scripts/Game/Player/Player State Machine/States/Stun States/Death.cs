namespace FAS.Players.States
{
	public class Death : StunState
	{
		protected override PlayerState ExitState => RespawnState;

		public override void Enter()
		{
			base.Enter();
			Pickup.Disable();
			DamageReceiver.DisableDamageableColliders();
			
			if (Weapon.Data.Type == WeaponType.Rifle)
				Animator.PlayRifleDeathAnim();
			
			Animator.SetLocomotionValue(0);
		}
	}
}