using UnityEngine;
using System;

namespace FAS
{
	public class PunchEffectTrigger : MonoBehaviour
	{
		[SerializeField] private ParticleSystem _particles;
		
		public event Action<DamageableCollider> OnTrigger;
		
		private SphereCollider _collider;

		private bool _isActive;

		private void Awake()
		{
			_collider = GetComponent<SphereCollider>();
			transform.SetParent(null);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out DamageableCollider damageable))
				OnTrigger?.Invoke(damageable);
		}

		private void Enable()
		{
			_collider.enabled = true;
			_isActive = true;
		}

		private void Disable()
		{
			_isActive = false;
			_collider.enabled = false;
		}

		private void UpdatePosition()
		{
			var particles = new ParticleSystem.Particle[_particles.particleCount];
			_particles.GetParticles(particles);
            
			_collider.transform.position = particles[0].position;
		}

		private void Update()
		{
			if (_particles.particleCount > 0)
			{
				if (!_isActive)
					Enable();

				UpdatePosition();
			}
			else if (_isActive)
			{
				Disable();
			}
		}
	}	
}