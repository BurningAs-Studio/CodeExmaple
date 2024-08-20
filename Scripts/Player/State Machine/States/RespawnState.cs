using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class RespawnState : PlayerState
    {
        public override void Enter()
        {
            CharacterController.enabled = false;
            Transform.position = Spawnable.SpawnPosition;
            RequestTransition(States.DefaultState);
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
            base.Exit();
        }
    }
}