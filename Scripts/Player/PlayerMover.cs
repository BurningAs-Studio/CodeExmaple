using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
	[Serializable]
	public class PlayerMover
	{
		[SerializeField] protected float _maxMoveSpeed = 5f;
		[SerializeField] private float _stoppingSpeed = 5f;
		[SerializeField] private float _movementLerpTime = 15f;
		[SerializeField] private float _decelerationTime = 15f;

		[Inject] private IReadOnlyCamerasSwitcher _camerasSwitcher;
		[Inject] private CharacterController _characterController;
		[Inject] private BaseAnimatorLayer _baseAnimatorLayer;
		[Inject] private IReadOnlyPlayerInput _input;
		[Inject] private IReadOnlyPlayerJump _jump;
		[Inject] private IReadOnlyPlayerAim _aim;
		[Inject] private PlayerRotator _rotator;
		[Inject] private Transform _transform;

		private Vector3 _autoMovementTargetPosition;
		private Vector3 _lastMoveDirection;

		private Vector2 _currentVelocity;
		private Vector2 _targetVelocity;
		
		private float _currentSpeedMultiplier = 1f;

		private const float MIN_MOVE_SPEED = 0;
		
		public bool IsMovementProcess => _currentVelocity.magnitude > MIN_MOVE_SPEED;
		public bool IsAutoMovement { get; private set; }

		public event Action OnAutoMovementStarted;
		public event Action OnAutoMovementCompleted;

		public void StartAutoMovement(Vector3 movementPosition)
		{
			IsAutoMovement = true;
			_autoMovementTargetPosition = movementPosition;
			OnAutoMovementStarted?.Invoke();
		}
		
		public void AutoMovement()
		{
			_autoMovementTargetPosition.y = _transform.position.y;

			var moveVector = _autoMovementTargetPosition - _transform.position;
			var moveDirectionNormalized = moveVector.normalized;
			var distanceSqrMagnitude = Vector3.SqrMagnitude(moveVector);

			MoveTo( new Vector2(Mathf.Min(distanceSqrMagnitude, 1), 0), moveDirectionNormalized);
			_rotator.RotateTo(moveDirectionNormalized);

			if (distanceSqrMagnitude < 0.1f)
				StopAutoMovement();
		}

		private void StopAutoMovement()
		{
			_baseAnimatorLayer.SetLocomotionValue(0);
			IsAutoMovement = false;
			OnAutoMovementCompleted?.Invoke();
		}

		public void ManualMovement()
		{
			var moveDirectionNormalized = _input.GetJoystickDirectionNormalized();
			var moveDirection = _input.GetJoystickDirection();
			
			if (moveDirection.magnitude != 0)
			{
				MoveTo(moveDirection, moveDirectionNormalized);

				if (!_aim.IsActive && _camerasSwitcher.IsCameraReseted)
					_rotator.RotateTo(moveDirectionNormalized);
			}
			else if (IsMovementProcess)
			{
				Inertia();
			}
			else if (Math.Abs(_jump.GetJumpVector().y) > 0)
			{
				MoveTo(moveDirection, moveDirectionNormalized);
			}
		}

		private void MoveTo(Vector2 direction, Vector3 inputNormalized)
		{
			_targetVelocity.x = direction.x * (_maxMoveSpeed * _currentSpeedMultiplier);
			_targetVelocity.y = direction.y * (_maxMoveSpeed * _currentSpeedMultiplier);
			_currentVelocity = Vector2.Lerp(
				_currentVelocity, _targetVelocity, _movementLerpTime * Time.deltaTime);

			UpdateLocomotionAnim();
			_characterController.Move((inputNormalized * _currentVelocity.magnitude + _jump.GetJumpVector()) * Time.deltaTime);
			_lastMoveDirection = _input.GetJoystickDirectionNormalized();
		}
		
		public void Inertia()
		{
			_currentVelocity = Vector2.MoveTowards(
				_currentVelocity, Vector2.zero, _decelerationTime * Time.deltaTime);
			
			UpdateLocomotionAnim();
			_characterController.Move((_lastMoveDirection * _currentVelocity.magnitude + _jump.GetJumpVector()) * Time.deltaTime);
		}
		
		private void UpdateLocomotionAnim()
		{
			if (_aim.IsActive)
			{
				_baseAnimatorLayer.SetLocomotionValue(new Vector2(
					_currentVelocity.x / (_maxMoveSpeed * _currentSpeedMultiplier),
					_currentVelocity.y / (_maxMoveSpeed * _currentSpeedMultiplier)));
			}
			else
			{
				_baseAnimatorLayer.SetLocomotionValue(_currentVelocity.magnitude / (_maxMoveSpeed * _currentSpeedMultiplier));

			}
		}

		public void MultiplySpeed(float multiplier)
		{
			_currentSpeedMultiplier = multiplier;
		}

		public void ResetSpeedMultiplier()
		{
			_currentSpeedMultiplier = 1;
		}
	}
}
