using PetWorld.Player.Building;
using PetWorld.Pets;
using PetWorld.UI;
using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public abstract class PlayerState : State
    {
        [Inject] [NonSerialized] protected PlayerPetBallInteraction PetballInteraction;
        [Inject] [NonSerialized] protected CharacterController CharacterController;
        [Inject] [NonSerialized] protected IReadOnlyPlayerInputEvents InputEvents;
        [Inject] [NonSerialized] protected PlayerVisualEffects VisualEffects;
        [Inject] [NonSerialized] protected PlayerCameraRotator CameraRotator;
        [Inject] [NonSerialized] protected PlayerWatterChecker WatterChecker;
        [Inject] [NonSerialized] protected IPlayerInputControl InputControl;
        [Inject] [NonSerialized] protected PlayerSoundEffects SoundEffects;
        [Inject] [NonSerialized] protected CamerasSwitcher CamerasSwitcher;
        [Inject] [NonSerialized] protected UIScreenSwitcher ScreenSwitcher;
        [Inject] [NonSerialized] protected PlayerAnimationRig AnimationRig;
        [Inject] [NonSerialized] protected GroundChecker GroundChecker;
        [Inject] [NonSerialized] protected IWeaponHolder WeaponHolder;
        [Inject] [NonSerialized] protected IPlayerStatesHolder States;
        [Inject] [NonSerialized] protected IReadOnlyPlayerInput Input;
        [Inject] [NonSerialized] protected IInventoryAccess Inventory;
        [Inject] [NonSerialized] protected PlayerInputView InputView;
        [Inject] [NonSerialized] protected PlayerBuilding Building;
        [Inject] [NonSerialized] protected PlayerAttacker Attacker;
        [Inject] [NonSerialized] protected PlayerRotator Rotator;
        [Inject] [NonSerialized] protected ISpawnable Spawnable;
        [Inject] [NonSerialized] protected PlayerHealth Health;
        [Inject] [NonSerialized] protected Transform Transform;
        [Inject] [NonSerialized] protected PlayerMover Mover;
        [Inject] [NonSerialized] protected PlayerJump Jump;

        public override void AddListeners()
        {
            Health.OnDead += OnDead;
        }

        private void OnDead()
        {
            RequestTransition(States.DeadState);
        }

        public override void RemoveListeners()
        {
            Health.OnDead -= OnDead;
        }
    }
}