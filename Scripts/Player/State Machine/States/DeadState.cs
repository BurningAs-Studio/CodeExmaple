using UnityEngine;
using System;
using Zenject;

namespace PetWorld.Player
{
    [Serializable]
    public class DeadState : StunState
    {
        [Inject] private DeadAnimatorLayer _deadAnimatorLayer;
        
        public void Initialize()
        {
            RemoveListeners();
        }
        
        public override void Enter()
        {
            base.Enter();
            CamerasSwitcher.PlayDeathCamera();
            ScreenSwitcher.ShowDeathScreen();
            AnimationRig.DisableAllRigs();
            _deadAnimatorLayer.DeadAnim();
        }

        public override void Perform()
        {
            Jump.ApplyGravity();
            
            if (Time.timeSinceLevelLoad > ExitTime)
                RequestTransition(States.RespawnState);
        }

        public override void Exit()
        {
            IsReadyToTransit = false;
        }
    }
}