using System;
using UnityEngine;
using VInspector;

namespace FAS.Pickups
{
	public abstract class Pickupable : MonoBehaviour, IPickupVisitable
	{
		[SerializeField] private GameObject _body;
		[SerializeField] private ParticleSystem _pickupEffect;
		[SerializeField] private bool _isRespawnable = true;
		[ShowIf(nameof(_isRespawnable))]
		[SerializeField] private float _respawnDelay = 5f;
		[EndIf]

		private bool _isPickupped;
		
		private float _nextRespawnTime;
		
		public event Action OnRespawn;

		public virtual void Pickup()
		{
			_body.gameObject.SetActive(false);
			_pickupEffect.Play();
			_nextRespawnTime = Time.timeSinceLevelLoad + _respawnDelay;
			_isPickupped = true;
		}

		protected virtual void Respawn()
		{
			_body.gameObject.SetActive(true);
			_isPickupped = false;
			OnRespawn?.Invoke();
		}

		private void Update()
		{
			if (_isRespawnable && _isPickupped && Time.timeSinceLevelLoad > _nextRespawnTime)
				Respawn();
		}

		public abstract void Apply(IPickupVisitor visitor);

#if UNITY_EDITOR
		[SerializeField] private bool _showGizmos = true;

		[ShowIf(nameof(_showGizmos))]
		[SerializeField] private GizmosFormData _gizmosData;

		[EndIf]
		private void OnDrawGizmos()
		{
			var originalMatrix = Gizmos.matrix;
			
			if (_showGizmos)
			{
				Gizmos.color = _gizmosData.Color;
				var position = transform.position + _gizmosData.Offset;

				if (_gizmosData.Form == GizmosForm.Cube || _gizmosData.Form == GizmosForm.WireCube)
				{
					Gizmos.matrix = Matrix4x4.TRS(position, transform.rotation, Vector3.one);
					position = Vector3.zero;
				}

				switch (_gizmosData.Form)
				{
					case GizmosForm.Sphere:
						Gizmos.DrawSphere(position, _gizmosData.Radius);
						break;
					case GizmosForm.Cube:
						Gizmos.DrawCube(position, _gizmosData.Size);
						break;
					case GizmosForm.WireCube:
						Gizmos.DrawWireCube(position, _gizmosData.Size);
						break;
					case GizmosForm.WireSphere:
					default:
						Gizmos.DrawWireSphere(position, _gizmosData.Radius);
						break;
				}

				Gizmos.matrix = originalMatrix;
			}
		}
	}
#endif
}