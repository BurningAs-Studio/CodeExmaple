using System.Collections.Generic;
using UnityEngine;

namespace FAS.Players
{
	public class PlayerVisualEffects : MonoBehaviour
	{
		[SerializeField] private List<ParticleSystem> _cameraBloodEffects = new ();
		[SerializeField] private ParticleSystem _speedBoostEffect;
		[SerializeField] private ParticleSystem _punchBuffEffect;
		[SerializeField] private ParticleSystem _kickWindEffect;
		[SerializeField] private ParticleSystem _punchEffect;
		[SerializeField] private Transform _punchEffectPoint;

		private void Awake()
		{
			_punchEffect.transform.SetParent(null);
		}

		public void PlaySpeedBoostEffect() => _speedBoostEffect.Play();
		
		public void PlayPunchBuffEffect() => _punchBuffEffect.Play();
		
		public void PlayPunchEffect()
		{
			_punchEffect.transform.SetPositionAndRotation(_punchEffectPoint.position, _punchEffectPoint.rotation);
			_punchEffect.Play();
		}

		public void PlayKickEffect() => _kickWindEffect.Play();
		
		public void StopKickEffect() => _kickWindEffect.Stop();
		
		public void PlayTakeDamageEffect(float damage)
		{
			var bloodEffectsCount = damage switch
			{
				>= 10 and < 20 => 1,
				>= 20 and < 30 => 2,
				>= 30 and < 40 => 3,
				>= 40 => 4,
				_ => 0
			};

			for (var i = 0; i <= bloodEffectsCount; i++)
				_cameraBloodEffects[i].Play();
		}
	}
}