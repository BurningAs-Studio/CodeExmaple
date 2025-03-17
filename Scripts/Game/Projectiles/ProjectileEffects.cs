using UnityEngine;
using System;

namespace FAS.Projectiles
{
	[Serializable]
	public class ProjectileEffects
	{
		[SerializeField] private ParticleSystem _hitCommon;
		[SerializeField] private ParticleSystem _hitBlood;
		[SerializeField] private ParticleSystem _body;

		public void Initialize(Transform parent)
		{
			_hitCommon.transform.SetParent(parent);
			_hitBlood.transform.SetParent(parent);
		}
		
		public void PlayBodyEffect() => _body.Play();
		
		public void StopBodyEffect() => _body.Stop();
		
		public void PlayCommonHitEffect(Vector3 hitPosition)
		{
			_hitCommon.Play();
			_hitCommon.transform.position = hitPosition;
		}

		public void PlayBloodHitEffect(Vector3 hitPosition)
		{
			_hitBlood.Play();
			_hitBlood.transform.position = hitPosition;
		}
	}
}