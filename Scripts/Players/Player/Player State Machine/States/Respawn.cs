using UnityEngine;

namespace FAS.Players.States
{
	public class Respawn : PlayerState
	{
		public override void Enter()
		{
			CharacterController.enabled = false;
			Transform.position = Spawnable.SpawnPosition;
			RequestTransition(UnarmedState);
		}

		public override void Perform()
		{
		}

		public override void Exit()
		{
			CharacterController.enabled = true;
			CharacterController.Move(Vector3.zero);
			Health.TryHeal(float.MaxValue);
			CameraRotator.ResetRotation();
			InputControl.EnableMovementInput();
			InputControl.EnableButtonsInput();
			DamageReceiver.EnableDamageableColliders();
			Pickup.Enable();
			base.Exit();
		}
	}
}