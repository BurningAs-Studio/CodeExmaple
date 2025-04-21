using UnityEngine;
using System;

namespace FAS.Projectiles
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField] private ProjectileEffects _effects;
		[SerializeField] private ProjectileTrigger _trigger;
		[SerializeField] private Transform _body;
		[SerializeField] private float _moveSpeed = 15f;
		[SerializeField] private float _maxDistance = 20f;

		private DamageReceiver _currentDamageReceiver;

		private Vector3 _currentDirection;
		private Vector3 _startPosition;

		private float _timeToStopMove;
		private float _currentDamage;

		private bool _isMoving;

		private const float MAX_LIFE_TIME = 5f;

		public event Action<Projectile> OnMoveComplete;

		private void OnEnable()
		{
			_trigger.OnTriggerTarget += OnTriggerTarget;
		}

		private void OnDisable()
		{
			_trigger.OnTriggerTarget -= OnTriggerTarget;
		}

		public void Initialize()
		{
			gameObject.SetActive(false);
			_effects.Initialize(transform.parent);
		}
		
		private void OnTriggerTarget(ITarget target)
		{
			target.TakeDamage(_currentDamage, _currentDamageReceiver);
			StopMove();
		}

		private void StopMove()
		{
			_isMoving = false;
			_effects.StopBodyEffect();
			OnMoveComplete?.Invoke(this);
		}

		public void Launch(Vector3 targetPosition, float damage, DamageReceiver damageReceiver)
		{
			_timeToStopMove = Time.timeSinceLevelLoad + MAX_LIFE_TIME;
			_body.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			_currentDamageReceiver = damageReceiver;
			_body.LookAt(targetPosition);
			_currentDirection = (targetPosition - _body.transform.position).normalized;
			_currentDamage = damage;
			_startPosition = _body.position;
			_effects.PlayBodyEffect();
			_isMoving = true;
		}

		private void Update()
		{
			if (_isMoving)
			{
				_body.Translate(_currentDirection * (_moveSpeed * Time.deltaTime), Space.World);
				
				if (Time.timeSinceLevelLoad > _timeToStopMove)
					StopMove();
			}
		}
	}
}