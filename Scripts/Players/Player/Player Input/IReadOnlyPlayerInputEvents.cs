using System;

namespace FAS.Players
{
	public interface IReadOnlyPlayerInputEvents
	{
		public event Action OnSpeedBoostButtonClicked;
		public event Action OnFatalityButtonClicked;
		public event Action OnSettingsButtonClicked;
		public event Action OnPunchButtonClicked;
		public event Action OnKickButtonClicked;
		public event Action OnJumpButtonClicked;
		public event Action OnAttackButtonDown;
		public event Action OnAttackButtonUp;
	}
}