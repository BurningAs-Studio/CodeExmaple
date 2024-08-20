using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class AimingState : GroundedState
    {
        [SerializeField] private float _aimingSpeedMultiplier = 0.5f;

        [Inject] [NonSerialized] private BaseAnimatorLayer _baseAnimatorLayer;
        [Inject] [NonSerialized] private PlayerAim _aim;
        
        public override void Enter()
        {
            base.Enter();
            PetballInteraction.OnFinishHold += OnFinishPetBallAim;
            
            Rotator.LookAt(CamerasSwitcher.FollowCameraPosition);
            Mover.MultiplySpeed(_aimingSpeedMultiplier);
            CameraRotator.CameraToPlayerRotation();
            InputView.SwitchAimButtonToDisable();
            CamerasSwitcher.PlayAimCamera();
            _aim.Enable();
        }
        
        private void OnFinishPetBallAim()
        {
            if (WeaponHolder.IsWeaponHolding)
            {
                InputView.DisableWeaponModeButtons();
                _aim.Disable();
                _baseAnimatorLayer.ChangeLocomotionTypeToUnarmed();
                AnimationRig.DisableHandsRig();
            }
        }

        public override void Perform()
        {
            _aim.CheckAimHits();
            _aim.UpdateCrosshairSize(Mover.IsMovementProcess);
            base.Perform();
        }

        public override void Exit()
        {
            if (PetballInteraction.IsHoldingPetBall)
            {
                //_baseAnimatorLayer.EnablePetballAnim();
                _baseAnimatorLayer.ChangeLocomotionTypeToPistolAim();
            }
            
            _aim.Disable();
            InputView.SwitchAimButtonToEnable();
            CamerasSwitcher.PlayFollowCamera();
            Mover.ResetSpeedMultiplier();
            PetballInteraction.OnFinishHold -= OnFinishPetBallAim;
            base.Exit();
        }
    }
}