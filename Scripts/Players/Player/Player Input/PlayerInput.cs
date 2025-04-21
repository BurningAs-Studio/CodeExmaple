using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerInput : BasePlayerInput, IReadOnlyPlayerInput, IPlayerInputControl
	{
		[SerializeField] private InputActionReference _joystickMoveAction;
		[SerializeField] private PlayerInputView _view;
		
		[Inject] private Camera _mainCamera;
		
		public bool IsMovementJoystickActive { get; private set; } = true;
		
		public void DisableMovementInput() => IsMovementJoystickActive = false;
		public void EnableMovementInput() => IsMovementJoystickActive = true;
		public void DisableButtonsInput() => IsButtonsInputActive = false;
		public void EnableButtonsInput() => IsButtonsInputActive = true;
		
		private void OnEnable()
		{
			_view.SpeedBoostButton.OnButtonDown.AddListener(OnSpeedBoostButtonClicked);
			_view.FatalityButton.OnButtonDown.AddListener(OnFatalityButtonClicked);	
			_view.SettingsButton.OnButtonDown.AddListener(OnSettingsButtonClicked);	
			_view.PunchButton.OnButtonDown.AddListener(OnPunchButtonClicked);
			_view.AttackButton.OnButtonDown.AddListener(OnAttackButtonDown);
			_view.JumpButton.OnButtonDown.AddListener(OnJumpButtonClicked);
			_view.KickButton.OnButtonDown.AddListener(OnKickButtonClicked);
			_view.AttackButton.OnButtonUp.AddListener(OnAttackButtonUp);	
			_joystickMoveAction.action.Enable();
		}

		private void OnDisable()
		{
			_view.SpeedBoostButton.OnButtonDown.RemoveListener(OnSpeedBoostButtonClicked);
			_view.FatalityButton.OnButtonDown.RemoveListener(OnFatalityButtonClicked);
			_view.SettingsButton.OnButtonDown.RemoveListener(OnSettingsButtonClicked);	
			_view.PunchButton.OnButtonDown.RemoveListener(OnPunchButtonClicked);
			_view.AttackButton.OnButtonDown.RemoveListener(OnAttackButtonDown);
			_view.JumpButton.OnButtonDown.RemoveListener(OnJumpButtonClicked);
			_view.KickButton.OnButtonDown.RemoveListener(OnKickButtonClicked);
			_view.AttackButton.OnButtonUp.RemoveListener(OnAttackButtonUp);	
			_joystickMoveAction.action.Disable();
		}
		
		private void OnSpeedBoostButtonClicked() => TrySendOnButtonClickEvent(Events.InvokeOnSpeedBoostButtonClicked);
		private void OnSettingsButtonClicked() => TrySendOnButtonClickEvent(Events.InvokeOnSettingsButtonClicked);
		private void OnFatalityButtonClicked() => TrySendOnButtonClickEvent(Events.InvokeOnFatalityButtonClicked);
		private void OnPunchButtonClicked() => TrySendOnButtonClickEvent(Events.InvokeOnPunchButtonClicked);
		private void OnKickButtonClicked() => TrySendOnButtonClickEvent(Events.InvokeOnKickButtonClicked);
		private void OnJumpButtonClicked() => TrySendOnButtonClickEvent(Events.InvokeOnJumpButtonClicked);
		private void OnAttackButtonDown() => TrySendOnButtonClickEvent(Events.InvokeOnAttackButtonDown);
		private void OnAttackButtonUp() => TrySendOnButtonClickEvent(Events.InvokeOnAttackButtonUp);
		
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
