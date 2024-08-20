using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerInputEvents : IReadOnlyPlayerInputEvents
    {
        public event Action OnCloseInventoryButtonClicked;
        public event Action OnOpenInventoryButtonClicked;
        public event Action OnDisableAimModButtonClicked;
        public event Action OnEnableAimModButtonClicked;
        public event Action OnThrowPetBallButtonClicked;
        public event Action OnCloseCraftMenuButtonClicked;
        public event Action OnMainWeaponButtonClicked;
        public event Action OnInteractButtonClicked;
        public event Action OnPistolButtonClicked;
        public event Action OnAttackButtonDown;
        public event Action OnAttackButtonUp;
        public event Action OnReloadButtonUp;

        public void InvokeOnCloseInventoryButtonClicked() => OnCloseInventoryButtonClicked?.Invoke();
        public void InvokeOnCloseCraftMenuButtonClicked() => OnCloseCraftMenuButtonClicked?.Invoke();
        public void InvokeOnOpenInventoryButtonClicked() => OnOpenInventoryButtonClicked?.Invoke();
        public void InvokeOnDisableAimModButtonClicked() => OnDisableAimModButtonClicked?.Invoke();
        public void InvokeOnEnableAimModButtonClicked() => OnEnableAimModButtonClicked?.Invoke();
        public void InvokeOnThrowPetBallButtonClicked() => OnThrowPetBallButtonClicked?.Invoke();
        public void InvokeOnMainWeaponButtonClicked() => OnMainWeaponButtonClicked?.Invoke();
        public void InvokeOnInteractButtonClicked() => OnInteractButtonClicked?.Invoke();
        public void InvokeOnPistolButtonClicked() => OnPistolButtonClicked?.Invoke();
        public void InvokeOnAttackButtonDown() => OnAttackButtonDown?.Invoke();
        public void InvokeOnAttackButtonUp() => OnAttackButtonUp?.Invoke();
        public void InvokeOnReloadButtonUp() => OnReloadButtonUp?.Invoke();

    }
}