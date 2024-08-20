using PetWorld.Player.Building;
using UnityEngine;
using VInspector;
using Zenject;
using System;

namespace PetWorld.Player
{
	[Serializable]
	public class PlayerInput : ListenerSubscriber, IReadOnlyPlayerInput, IPlayerInputControl
	{
		[SerializeField] private PlayerInputEvents _events;
		[SerializeField] private PlayerInputView _view;
		
		[Foldout("Default Input")]
		[SerializeField] private CustomButton _closeInventoryButton;
		[SerializeField] private CustomButton _closeCraftMenuButton;
		[SerializeField] private CustomButton _openInventoryButton;
		[SerializeField] private WorkbenchButton _workbenchButton;
		[SerializeField] private CustomButton _interactButton;
		[SerializeField] private Joystick _movementJoystick;
		[SerializeField] private CustomButton _attackButton;
		
		[Foldout("Weapon Input")]
		[SerializeField] private CustomButton _disableAimButton;
		[SerializeField] private CustomButton _enableAimButton;
		[SerializeField] private CustomButton _reloadButton;
		
		[Foldout("Weapon Changer")]
		[SerializeField] private SelectWeaponButton _mainWeaponButton;
		[SerializeField] private SelectWeaponButton _pistolButton;
		
		[Foldout("PetBall Selection")]
		[SerializeField] private CustomButton _throwPetBallButton;
		[EndFoldout]

		[Inject] [NonSerialized] private Camera _mainCamera;
		
		public IReadOnlyPlayerInputEvents Events => _events;
		public PlayerInputView View => _view;

		public bool IsMovementJoystickActive {get; private set;} = true;
		public bool IsButtonsInputActive {get; private set;} = true;

		public void Initialize()
		{
			_view.Initialize( _closeInventoryButton, _openInventoryButton, _throwPetBallButton, _mainWeaponButton,
				_disableAimButton, _enableAimButton, _workbenchButton, _interactButton, _reloadButton, _attackButton,
				_pistolButton);
		}

		public override void AddListeners()
		{
			_closeInventoryButton.OnButtonDown += CloseInventoryButtonClicked;
			_closeCraftMenuButton.OnButtonDown += CloseCraftMenuButtonClicked;
			_openInventoryButton.OnButtonDown += OpenInventoryButtonClicked;
			_throwPetBallButton.OnButtonDown += ThrowPetBallButtonClicked;
			_interactButton.OnButtonDown += InteractButtonClicked;
			_attackButton.OnButtonDown += AttackButtonDown;

			_disableAimButton.OnButtonDown += DisableAimModButtonClicked;
			_enableAimButton.OnButtonDown += EnableAimModButtonClicked;

			_mainWeaponButton.OnButtonDown += MainWeaponButtonClicked;
			_pistolButton.OnButtonDown += PistolButtonClicked;

			_attackButton.OnButtonUp += AttackButtonUp;
			_reloadButton.OnButtonDown += ReloadButtonClicked;
		}

		public override void RemoveListeners()
		{
			_closeInventoryButton.OnButtonDown -= CloseInventoryButtonClicked;
			_closeCraftMenuButton.OnButtonDown -= CloseCraftMenuButtonClicked;
			_openInventoryButton.OnButtonDown -= OpenInventoryButtonClicked;
			_throwPetBallButton.OnButtonDown -= ThrowPetBallButtonClicked;
			_interactButton.OnButtonDown -= InteractButtonClicked;
			_attackButton.OnButtonDown -= AttackButtonDown;

			_disableAimButton.OnButtonDown -= DisableAimModButtonClicked;
			_enableAimButton.OnButtonDown -= EnableAimModButtonClicked;

			_mainWeaponButton.OnButtonDown -= MainWeaponButtonClicked;
			_pistolButton.OnButtonDown -= PistolButtonClicked;

			_attackButton.OnButtonUp -= AttackButtonUp;
			_reloadButton.OnButtonDown -= ReloadButtonClicked;
		}
		
		public void DisableMovementInput() => IsMovementJoystickActive = false;
		public void EnableMovementInput() => IsMovementJoystickActive = true;
		public void DisableButtonsInput() => IsButtonsInputActive = false;
		public void EnableButtonsInput() => IsButtonsInputActive = true;

		private void CloseInventoryButtonClicked() => TrySendOnButtonClickEvent(_events.InvokeOnCloseInventoryButtonClicked);
		private void OpenInventoryButtonClicked() => TrySendOnButtonClickEvent(_events.InvokeOnOpenInventoryButtonClicked);
		private void DisableAimModButtonClicked() => TrySendOnButtonClickEvent(_events.InvokeOnDisableAimModButtonClicked);
		private void EnableAimModButtonClicked() => TrySendOnButtonClickEvent(_events.InvokeOnEnableAimModButtonClicked);
		private void ThrowPetBallButtonClicked() => TrySendOnButtonClickEvent(_events.InvokeOnThrowPetBallButtonClicked);
		private void MainWeaponButtonClicked() => TrySendOnButtonClickEvent(_events.InvokeOnMainWeaponButtonClicked);
		private void InteractButtonClicked() => TrySendOnButtonClickEvent(_events.InvokeOnInteractButtonClicked);
		private void PistolButtonClicked() => TrySendOnButtonClickEvent(_events.InvokeOnPistolButtonClicked);
		private void ReloadButtonClicked() => TrySendOnButtonClickEvent(_events.InvokeOnReloadButtonUp);
		private void AttackButtonDown() => TrySendOnButtonClickEvent(_events.InvokeOnAttackButtonDown);
		private void CloseCraftMenuButtonClicked() => _events.InvokeOnCloseCraftMenuButtonClicked();
		private void AttackButtonUp() => TrySendOnButtonClickEvent(_events.InvokeOnAttackButtonUp);

		
		private void TrySendOnButtonClickEvent(Action action)
		{
			if (IsButtonsInputActive)
				action?.Invoke();
		}
		
		public Vector3 GetJoystickDirectionNormalized()
		{
			var movementDirection = Vector3.zero;
			
			if (IsMovementJoystickActive)
			{
#if UNITY_EDITOR
				movementDirection.x = Math.Abs(_movementJoystick.Horizontal) > 0 ?
					_movementJoystick.Horizontal : Input.GetAxisRaw("Horizontal");
				movementDirection.z = Math.Abs(_movementJoystick.Vertical) > 0 ?
					_movementJoystick.Vertical : Input.GetAxisRaw("Vertical");
#elif UNITY_ANDROID
			movementDirection.x = _movementJoystick.Horizontal;
			movementDirection.z = _movementJoystick.Vertical;
#endif
				movementDirection = _mainCamera.transform.TransformDirection(movementDirection);
				movementDirection.y = 0f;
			}
			return movementDirection.normalized;
		}

		public Vector2 GetJoystickDirection()
		{
			var vector = Vector2.zero;
			
			if (IsMovementJoystickActive)
			{
#if UNITY_EDITOR
				vector = Mathf.Clamp01(_movementJoystick.Direction.magnitude) > 0 ? 
					_movementJoystick.Direction : 
					new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
				
#elif UNITY_ANDROID
			vector = _movementJoystick.Direction;
#endif
			}
			return vector;
		}
	}
}