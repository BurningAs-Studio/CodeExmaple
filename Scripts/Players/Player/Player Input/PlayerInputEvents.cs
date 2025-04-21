using System;

namespace FAS.Players
{
	public class PlayerInputEvents : IReadOnlyPlayerInputEvents
	{
		public event Action OnSpeedBoostButtonClicked;
		public event Action OnFatalityButtonClicked;
		public event Action OnSettingsButtonClicked;
		public event Action OnPunchButtonClicked;
		public event Action OnKickButtonClicked;
		public event Action OnJumpButtonClicked;
		public event Action OnAttackButtonDown;
		public event Action OnAttackButtonUp;

		public void InvokeOnSpeedBoostButtonClicked() => OnSpeedBoostButtonClicked?.Invoke();
		public void InvokeOnFatalityButtonClicked() => OnFatalityButtonClicked?.Invoke();
		public void InvokeOnSettingsButtonClicked() => OnSettingsButtonClicked?.Invoke();
		public void InvokeOnPunchButtonClicked() => OnPunchButtonClicked?.Invoke();
		public void InvokeOnJumpButtonClicked() => OnJumpButtonClicked?.Invoke();
		public void InvokeOnKickButtonClicked() => OnKickButtonClicked?.Invoke();
		public void InvokeOnAttackButtonDown() => OnAttackButtonDown?.Invoke();
		public void InvokeOnAttackButtonUp() => OnAttackButtonUp?.Invoke();
	}
}