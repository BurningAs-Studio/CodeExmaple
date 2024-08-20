using System;

namespace PetWorld.Player
{
    [Serializable]
    public abstract class GroundedState : PlayerState
    {
        public override void Enter()
        {
            InputEvents.OnAttackButtonDown += TryStartAttack;
            InputEvents.OnAttackButtonUp += StopAttack;
        }

        private void TryStartAttack()
        {
            if (PetballInteraction.IsHoldingPetBall)
            {
                if (PetballInteraction.IsCanThrow)
                    ThrowPetball();
            }
            else
            {
                StartAttack();
            }
        }

        private void ThrowPetball()
        {
            //Animator.PlayAimThrowPetBallAnim();
            PetballInteraction.Throw();
        }

        protected virtual void StartAttack()
        {
            Attacker.StartAttack();
        }
        
        private void StopAttack()
        {
            Attacker.StopAttack();
        }

        public override void Perform()
        {
            if (!GroundChecker.IsGrounded)
            {
                Jump.FreeFall();

                if (WatterChecker.IsWatter())
                {
                    RequestTransition(States.SwimmingState);
                    return;
                }
            }

            Jump.ApplyGravity();
            GroundChecker.CheckGround();

            if (Input.IsMovementJoystickActive)
                Mover.ManualMovement();
            
            Attacker.AttackProcess();
        }

        public override void Exit()
        {
            InputEvents.OnAttackButtonDown -= TryStartAttack;
            InputEvents.OnAttackButtonUp -= StopAttack;
            base.Exit();
        }
    }
}