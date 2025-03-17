using Zenject;

namespace FAS.Players.States
{
	public abstract class SkillCastState : PlayerState
	{
		[Inject] private PlayerSpeedBoost _speedBoost;
		
		public override void Enter()
		{
			DamageReceiver.DisableDamageableColliders();
			Aim.HideCrosshair();
			base.Enter();
		}
		
		public override void Perform()
		{
			_speedBoost.RequestDisable();
		}
		
		public override void Exit()
		{
			base.Exit();
			Aim.ShowCrosshair();
			DamageReceiver.EnableDamageableColliders();
		}
	}
}