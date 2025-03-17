using UnityEngine;

namespace FAS.Players.States
{
	public class Respawn : PlayerState
	{
		public override void Enter()
		{
			CharacterController.enabled = false;
			Transform.position = Spawnable.SpawnPosition;
			RequestTransition(IdleState);
		}

		public override void Perform()
		{
		}

		public override void Exit()
		{
			CharacterController.enabled = true;
			CharacterController.Move(Vector3.zero);
			CameraRotator.ResetRotation();
			InputControl.EnableMovementInput();
			InputControl.EnableButtonsInput();
			Pickup.Enable();
			base.Exit();
		}
	}
}