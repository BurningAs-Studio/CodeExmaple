using System;

namespace FAS.Players
{
	public class PlayerInputEvents : IReadOnlyPlayerInputEvents
	{
		public event Action OnChangeWeaponButtonClicked;
		public event Action OnSpeedBoostButtonClicked;
		public event Action OnPunchButtonClicked;
		public event Action OnInteractButtonDown;
		public event Action OnKickButtonClicked;
		public event Action OnInteractButtonUp;
		public event Action OnJumpButtonClicked;
		public event Action OnAttackButtonDown;
		public event Action OnAttackButtonUp;

		public void InvokeOnSpeedBoostButtonClicked() => OnSpeedBoostButtonClicked?.Invoke();
		public void InvokeOnChangeWeaponClicked() => OnChangeWeaponButtonClicked?.Invoke();
		public void InvokeOnPunchButtonClicked() => OnPunchButtonClicked?.Invoke();
		public void InvokeOnKickButtonClicked() => OnKickButtonClicked?.Invoke();
		public void InvokeOnInteractButtonDown() => OnInteractButtonDown?.Invoke();
		public void InvokeOnInteractButtonUp() => OnInteractButtonUp?.Invoke();
		public void InvokeOnJumpButtonClicked() => OnJumpButtonClicked?.Invoke();
		public void InvokeOnAttackButtonDown() => OnAttackButtonDown?.Invoke();
		public void InvokeOnAttackButtonUp() => OnAttackButtonUp?.Invoke();
	}
}