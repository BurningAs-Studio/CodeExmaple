using UnityEngine;
using System;

namespace FAS.Zombies
{
	[RequireComponent(typeof(Animator))]
	public class ZombieAnimEventsReceiver : MonoBehaviour
	{
		public event Action OnDeathComplete;

		private void AE_OnDeathComplete() => OnDeathComplete?.Invoke();
	}
}