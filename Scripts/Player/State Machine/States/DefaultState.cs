using System;

namespace PetWorld.Player
{
    [Serializable]
    public class DefaultState : GroundedState
    {
        public override void Enter()
        {
            base.Enter();
            PetballInteraction.OnFinishHold += OnFinishPetBallAim;
        }

        private void OnFinishPetBallAim()
        {
            //Animator.DisablePetBallLayer();
			
            if (WeaponHolder.IsWeaponHolding)
                InputView.DisableWeaponModeButtons();
        }
        
        protected override void StartAttack()
        {
            base.StartAttack();
            RequestTransition(States.AimingState);
        }

        public override void Exit()
        {
            PetballInteraction.OnFinishHold -= OnFinishPetBallAim;
            base.Exit();
        }
    }
}