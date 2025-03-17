using UnityEngine;
using System;

namespace FAS.Projectiles
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField] private ProjectileEffects _effects;
		[SerializeField] private Transform _body;
		[SerializeField] private float _moveSpeed = 15f;
		[SerializeField] private LayerMask _collisionsLayers;
		
		private DamageReceiver _currentDamageReceiver;
		private DamageableCollider _currentTarget;
		
		private Vector3 _destination;

		private int _currentDamage;
		
		private bool _isHasObstacle;
		private bool _isMoving;
		
		public event Action<Projectile> OnMoveComplete;
		
		public void Initialize()
		{
			gameObject.SetActive(false);
			_effects.Initialize(transform.parent);
		}

		private void StopMove()
		{
			_isMoving = false;
			_effects.StopBodyEffect();
			OnMoveComplete?.Invoke(this);
		}
		
		public void Launch(Vector3 destination, Vector3 startPosition, int damage, DamageReceiver damageReceiver,
			bool isHasObstacle, DamageableCollider targetCollider = null)
		{
			_currentDamageReceiver = damageReceiver;
			_currentTarget = targetCollider;
			_isHasObstacle = isHasObstacle;
			_body.position = startPosition;
			_destination = destination;
			_currentDamage = damage;
			_body.LookAt(destination);
			_effects.PlayBodyEffect();
			_isMoving = true;
		}

		private void Update()
		{
			if (_isMoving)
			{
				if (_body.position != _destination)
				{
					_body.position = Vector3.MoveTowards(
						_body.position, _destination, _moveSpeed * Time.deltaTime);
			
					_body.LookAt(_destination);

					if (_body.position == _destination)
					{
						StopMove();

						if (_currentTarget != null)
						{
							_currentTarget.TakeDamage(_currentDamage, _currentDamageReceiver);
							
							if (_currentTarget.Type == DamageableType.Generic)
								_effects.PlayCommonHitEffect(_destination);
							else
								_effects.PlayBloodHitEffect(_destination);
						}
						else if (_isHasObstacle)
						{
							_effects.PlayCommonHitEffect(_destination);
						}
					}
				}
			}
		}
	}
}
