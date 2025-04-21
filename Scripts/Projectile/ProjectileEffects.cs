using UnityEngine;
using System;

namespace FAS.Projectiles
{
	[Serializable]
	public class ProjectileEffects
	{
		[SerializeField] private ParticleSystem _hitCommon;
		[SerializeField] private ParticleSystem _headshot;
		[SerializeField] private ParticleSystem _bodyshot;
		[SerializeField] private ParticleSystem _body;

		public void Initialize(Transform parent)
		{
			_hitCommon.transform.SetParent(parent);
			_bodyshot.transform.SetParent(parent);
			_headshot.transform.SetParent(parent);
		}
		
		public void PlayBodyEffect() => _body.Play();
		
		public void StopBodyEffect() => _body.Stop();
		
		public void PlayCommonHitEffect(Vector3 hitPosition)
		{
			_hitCommon.Play();
			_hitCommon.transform.position = hitPosition;
		}

		public void PlayBodyHitEffect(Vector3 hitPosition, float scaleMultiplier = 1)
		{
			_bodyshot.Play();
			_bodyshot.transform.position = hitPosition;
			_bodyshot.transform.localScale = Vector3.one * scaleMultiplier;
		}
		
		public void PlayHeadHitEffect(Vector3 hitOrigin, Vector3 hitPosition, float scaleMultiplier = 0.5f)
		{
			_headshot.Play();
			_headshot.transform.position = hitPosition;
			_headshot.transform.LookAt(hitOrigin);
			_headshot.transform.localScale = Vector3.one * scaleMultiplier;
		}
	}
}