using UnityEngine;
using System;

namespace FAS.Players
{
	public class BasePlayerInput : MonoBehaviour
	{
		public readonly PlayerInputEvents Events = new();
		
		public bool IsButtonsInputActive { get; protected set; } = true;

		protected void TrySendOnButtonClickEvent(Action action)
		{
			if (IsButtonsInputActive)
				action?.Invoke();
		}
	}
}