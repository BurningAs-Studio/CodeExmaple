using UnityEngine;
using System;

namespace FAS.Players.Animations
{
	public class PlayerAnimEventsReceiver : MonoBehaviour
	{
		public event Action OnFootstep;
		public event Action OnPunch;
		
		private void AE_FootStep() => OnFootstep?.Invoke();
		
		private void AE_Punch() => OnPunch?.Invoke();
	}
}