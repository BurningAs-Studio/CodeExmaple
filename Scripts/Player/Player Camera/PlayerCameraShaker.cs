using Cinemachine;
using UnityEngine;
using System;

namespace PetWorld.Player
{
	[Serializable]
	public class PlayerCameraShaker
	{
		[SerializeField] private CinemachineImpulseSource _takeDamageImpulse;
		
		public void PlayShotImpulse(CinemachineImpulseSource impulseSource)
		{
			impulseSource.GenerateImpulse();
		}

		public void PlayTakeDamageImpulse()
		{
			_takeDamageImpulse.GenerateImpulse();
		}
	}   
}
