using PetWorld.Player;
using PetWorld.UI;
using Zenject;
using System;

namespace PetWorld
{
    [Serializable]
    public class PlayerInteractableVisitor : IInteractableVisitor
    {
        [Inject] [NonSerialized] private IReadOnlyPlayerInputEvents _inputEvents;
        [Inject] [NonSerialized] private IInventoryResourceHolder _resourceHolder;
        [Inject] [NonSerialized] private UIScreenSwitcher _screenSwitcher;
        [Inject] [NonSerialized] private IInventoryAccess _inventory;
        [Inject] [NonSerialized] private CraftMenu _craftMenu;
        
        public event Action OnFinishVisit;

        public void Visit(ResourcePickup pickup)
        {
            pickup.PickUp();
            _resourceHolder.Add(pickup.Resource.Type, pickup.Resource.Amount);
            OnFinishVisit?.Invoke();
        }

        public void Visit(Workbench workbench)
        {
            _inputEvents.OnCloseCraftMenuButtonClicked += OnFinishVisitWorkbench;
            _screenSwitcher.ShowCraftScreen();
            _inventory.Open();
            _craftMenu.Open();
        }

        private void OnFinishVisitWorkbench()
        {
            _inventory.Close();
            OnFinishVisit?.Invoke();
        }
    }
}