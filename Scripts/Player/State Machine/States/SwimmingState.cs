using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class SwimmingState : PlayerState
    {
        [SerializeField] private float _swimmingSpeedMultiplier = 0.5f;
        
        [Inject] private BaseAnimatorLayer _baseAnimatorLayer;
        [Inject] private IWeaponVisibility _weaponVisibility;
        [Inject] private IWeaponHolder _weaponHolder;
        
        public override void Enter()
        {
            if (PetballInteraction.IsHoldingPetBall)
                PetballInteraction.HidePetball();
            else if (_weaponHolder.IsWeaponHolding)
                _weaponVisibility.Hide();

            VisualEffects.PlayWaterSplashEffect(Transform.position + Transform.forward);
            Mover.MultiplySpeed(_swimmingSpeedMultiplier);
            InputView.DisableChangeWeaponButtons();
            InputView.DisablePetballThrowButton();
            InputView.DisableWeaponModeButtons();
            InputView.DisableWorkbenchButton();
            AnimationRig.DisableAllRigs();
            Building.ExitPlacingMode();
            _baseAnimatorLayer.ChangeLocomotionTypeToSwimming();
            SoundEffects.PlayWaterSplashSoundEffect();
        }

        public override void Perform()
        {
            GroundChecker.CheckGround();
            
            if (GroundChecker.IsGrounded)
                RequestTransition(States.DefaultState);
            
            if (Input.IsMovementJoystickActive)
                Mover.ManualMovement();
        }

        public override void Exit()
        {
            if (PetballInteraction.IsHasPetBalls)
                InputView.EnablePetBallThrowButton();
            
            if (PetballInteraction.IsHoldingPetBall)
            {
                PetballInteraction.ShowPetball();
            }
            else
            {
                if (_weaponHolder.IsWeaponHolding)
                    _weaponVisibility.Show();
            }

            InputView.EnableWorkbenchButton();

            if (_weaponHolder.IsWeaponHolding)
            {
                InputView.EnableWeaponModeButtons();

                if (!Inventory.IsOpen)
                    InputView.EnableChangeWeaponButtons();
            }
            
            Mover.ResetSpeedMultiplier();
            base.Exit();
        }
    }
}