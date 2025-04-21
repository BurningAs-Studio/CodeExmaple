using UnityEngine;
using System;

namespace FAS.FakePlayers
{
	public class FakePlayerAnimEventsReceiver : MonoBehaviour
	{
		public event Action OnDeathComplete;
		public event Action OnFootstep;
		public event Action OnPunch;
		
		private void AE_OnDeathComplete() => OnDeathComplete?.Invoke();

		private void AE_FootStep() => OnFootstep?.Invoke();
		
		private void AE_Punch() => OnPunch?.Invoke();
	}
}