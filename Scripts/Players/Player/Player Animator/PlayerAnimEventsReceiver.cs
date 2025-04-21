using UnityEngine;
using System;

namespace FAS.Players.Animations
{
	public class PlayerAnimEventsReceiver : MonoBehaviour
	{
		public event Action OnFootstep;
		public event Action OnFatality;
		public event Action OnAttack;
		public event Action OnDisarm;
		public event Action OnPunch;
		public event Action OnArm;

		private void AE_Fatality() => OnFatality?.Invoke();
		
		private void AE_FootStep() => OnFootstep?.Invoke();
		
		private void AE_Attack() => OnAttack?.Invoke();
		
		private void AE_Disarm() => OnDisarm?.Invoke();
		
		private void AE_Punch() => OnPunch?.Invoke();
		
		private void AE_Arm() => OnArm?.Invoke();
	}
}