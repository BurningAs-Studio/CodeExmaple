using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public abstract class StunState : PlayerState
    {
        [SerializeField] protected float StunTime = 10f;
        
        protected float ExitTime;
        
        public override void Enter()
        {
            InputControl.DisableMovementInput();
            InputControl.DisableButtonsInput();
            ExitTime = Time.timeSinceLevelLoad + StunTime;
        }
        
        public override void Perform()
        {
            Jump.ApplyGravity();
            
            if (Time.timeSinceLevelLoad > ExitTime)
                RequestTransition(States.DefaultState);
        }

        public override void Exit()
        {
            InputControl.DisableButtonsInput();
            InputControl.DisableMovementInput();
            base.Exit();
        }
    }
}