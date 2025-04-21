using Cinemachine;
using UnityEngine;

namespace FAS.Players
{
	public class PlayerCameraShaker : MonoBehaviour, ICameraShaker
	{
		[SerializeField] private CinemachineImpulseSource _fatalityTornadoKickImpulse;
		[SerializeField] private CinemachineImpulseSource _takeDamageImpulse;
		[SerializeField] private CinemachineImpulseSource _punchImpulse;
		
		public void PlayImpulse(CinemachineImpulseSource impulseSource) => impulseSource.GenerateImpulse();

		public void PlayFatalityTornadoKickImpulse() => _fatalityTornadoKickImpulse.GenerateImpulse();
		
		public void PlayTakeDamageImpulse() => _takeDamageImpulse.GenerateImpulse();
		
		public void PlayPunchImpulse() => _punchImpulse.GenerateImpulse();
	}
}