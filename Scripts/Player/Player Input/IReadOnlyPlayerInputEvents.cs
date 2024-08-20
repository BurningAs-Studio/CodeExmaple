using System;

namespace PetWorld.Player
{
    public interface IReadOnlyPlayerInputEvents
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
    }
}