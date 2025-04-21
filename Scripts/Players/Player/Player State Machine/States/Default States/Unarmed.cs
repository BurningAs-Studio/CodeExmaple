namespace FAS.Players.States
{
	public class Unarmed : DefaultState
	{
		public override void Enter()
		{
			base.Enter();
			InputEvents.OnAttackButtonDown += TransitToThrowAttack;
			Animator.SetUnarmed();
		}
		
		private void TransitToThrowAttack() => RequestTransition(ArmedState);

		public override void Perform()
		{
			base.Perform();
			
			if (Input.IsMovementJoystickActive)
				Mover.ManualMovement();

			if (Input.GetJoystickDirection2D().magnitude != 0)
				Rotator.RequestSmoothRotateFull(Input.GetJoystickDirection3D().normalized);
			
			Animator.SetLocomotionValue(Mover.CurrentVelocity.magnitude / Mover.CurrentMaxSpeed);
		}

		public override void Exit()
		{
			base.Exit();
			InputEvents.OnAttackButtonDown -= TransitToThrowAttack;
		}
	}
}