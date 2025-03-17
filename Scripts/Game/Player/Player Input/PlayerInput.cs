using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;
using System;

namespace FAS.Players
{
	public class PlayerInput : MonoBehaviour, IReadOnlyPlayerInput, IPlayerInputControl, IPlayerInputView
	{
		[SerializeField] private PlayerInputView _view;
		[SerializeField] private InputActionReference _joystickMoveAction;

		[Inject] private Camera _mainCamera;

		private Vector2 _movementInput;

		public readonly PlayerInputEvents Events = new();
		
		public bool IsMovementJoystickActive { get; private set; } = true;
		public bool IsButtonsInputActive { get; private set; } = true;

		private void OnEnable()
		{
			_view.SpeedBoostButton.OnButtonDown.AddListener(OnSpeedBoostButtonClicked);
			_view.ChangeWeaponButton.OnButtonDown.AddListener(OnChangeWeaponClicked);
			_view.InteractButton.OnButtonDown.AddListener(OnInteractButtonDown);
			_view.PunchButton.OnButtonDown.AddListener(OnPunchButtonClicked);
			_view.InteractButton.OnButtonUp.AddListener(OnInteractButtonUp);
			_view.AttackButton.OnButtonDown.AddListener(OnAttackButtonDown);
			_view.JumpButton.OnButtonDown.AddListener(OnJumpButtonClicked);
			_view.KickButton.OnButtonDown.AddListener(OnKickButtonClicked);
			_view.AttackButton.OnButtonUp.AddListener(OnAttackButtonUp);	
			_joystickMoveAction.action.Enable();
		}

		private void OnDisable()
		{
			_view.SpeedBoostButton.OnButtonDown.RemoveListener(OnSpeedBoostButtonClicked);
			_view.ChangeWeaponButton.OnButtonDown.RemoveListener(OnChangeWeaponClicked);
			_view.InteractButton.OnButtonDown.RemoveListener(OnInteractButtonDown);
			_view.PunchButton.OnButtonDown.RemoveListener(OnPunchButtonClicked);
			_view.InteractButton.OnButtonUp.RemoveListener(OnInteractButtonUp);
			_view.AttackButton.OnButtonDown.RemoveListener(OnAttackButtonDown);
			_view.JumpButton.OnButtonDown.RemoveListener(OnJumpButtonClicked);
			_view.KickButton.OnButtonDown.RemoveListener(OnKickButtonClicked);
			_view.AttackButton.OnButtonUp.RemoveListener(OnAttackButtonUp);	
			_joystickMoveAction.action.Disable();
		}
		
		public void DisableMovementInput() => IsMovementJoystickActive = false;
		public void EnableMovementInput() => IsMovementJoystickActive = true;
		public void DisableButtonsInput() => IsButtonsInputActive = false;
		public void EnableButtonsInput() => IsButtonsInputActive = true;

		private void OnSpeedBoostButtonClicked() => TrySendOnButtonClickEvent(Events.InvokeOnSpeedBoostButtonClicked);
		private void OnChangeWeaponClicked() => TrySendOnButtonClickEvent(Events.InvokeOnChangeWeaponClicked);
		private void OnPunchButtonClicked() => TrySendOnButtonClickEvent(Events.InvokeOnPunchButtonClicked);
		private void OnInteractButtonDown() => TrySendOnButtonClickEvent(Events.InvokeOnInteractButtonDown);
		private void OnKickButtonClicked() => TrySendOnButtonClickEvent(Events.InvokeOnKickButtonClicked);
		private void OnInteractButtonUp() => TrySendOnButtonClickEvent(Events.InvokeOnInteractButtonUp);
		private void OnJumpButtonClicked() => TrySendOnButtonClickEvent(Events.InvokeOnJumpButtonClicked);
		private void OnAttackButtonDown() => TrySendOnButtonClickEvent(Events.InvokeOnAttackButtonDown);
		private void OnAttackButtonUp() => TrySendOnButtonClickEvent(Events.InvokeOnAttackButtonUp);

		public void HideInteractButton()
		{
			if (_view.InteractButton.IsHold)
				OnInteractButtonUp();
			
			_view.InteractButton.gameObject.SetActive(false);
		}

		public void ShowInteractButton()
		{
			_view.InteractButton.gameObject.SetActive(true);
		}

		private void TrySendOnButtonClickEvent(Action action)
		{
			if (IsButtonsInputActive)
				action?.Invoke();
		}

		public Vector3 GetJoystickDirection3D()
		{
			var movementDirection = Vector3.zero;

			if (IsMovementJoystickActive)
			{
				movementDirection.x = _joystickMoveAction.action.ReadValue<Vector2>().x;
				movementDirection.z = _joystickMoveAction.action.ReadValue<Vector2>().y;
				movementDirection = _mainCamera.transform.TransformDirection(movementDirection);
				movementDirection.y = 0f;
			}

			return movementDirection.normalized;
		}

		public Vector2 GetJoystickDirection2D()
		{
			var moveVector = Vector2.zero;

			if (IsMovementJoystickActive)
				moveVector = _joystickMoveAction.action.ReadValue<Vector2>();
			
			return moveVector;
		}
	}
}
