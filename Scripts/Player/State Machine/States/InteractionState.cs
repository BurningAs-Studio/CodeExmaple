using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class InteractionState : PlayerState
    {
        [SerializeField] private PlayerInteractableVisitor _visitor;
        
        [Inject] [NonSerialized] private PlayerInteractables _interactables;
        
        private Interactable _currentInteractable;
        
        [Inject]
        public override void Construct(DiContainer diContainer)
        {
            base.Construct(diContainer);
            diContainer.Inject(_visitor);
        }

        public override void AddListeners()
        {
            base.AddListeners();
            _visitor.OnFinishVisit += FinishInteraction;
        }

        public override void RemoveListeners()
        {
            base.RemoveListeners();
            _visitor.OnFinishVisit -= FinishInteraction;
        }

        public override void Enter()
        {
            InputControl.DisableMovementInput();
            InputControl.DisableButtonsInput();
            InputView.DisableInteractButton();
            _interactables.Disable();
			
            _currentInteractable = _interactables.GetClosestInteractable();
            
            if (_currentInteractable.IsHasInteractionPoint)
                StartMoveToInteractPosition(_currentInteractable.InteractionPoint.position);
            else
                Interact();
        }
        
        private void StartMoveToInteractPosition(Vector3 interactPosition)
        {
            Mover.StartAutoMovement(interactPosition);
            Mover.OnAutoMovementCompleted += OnFinishMoveToInteractPosition;
        }

        private void OnFinishMoveToInteractPosition()
        {
            Mover.OnAutoMovementCompleted -= OnFinishMoveToInteractPosition;
            Interact();
        }

        private void Interact()
        {
            _currentInteractable.Interact(_visitor);
            _interactables.Remove(_currentInteractable);
        }

        private void FinishInteraction()
        {
            RequestTransition(States.DefaultState);
        }

        public override void Perform()
        {
            if (Mover.IsAutoMovement)
                Mover.AutoMovement();
            else if (Mover.IsMovementProcess)
                Mover.Inertia();
        }

        public override void Exit()
        {
            _interactables.Enable();
            InputControl.EnableMovementInput();
            InputControl.EnableButtonsInput();
            base.Exit();
        }
    }
}