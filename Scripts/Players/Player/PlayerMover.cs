using FAS.Players.Animations;
using UnityEngine;
using Zenject;
using System;

namespace FAS.Players
{
    public class PlayerMover : MonoBehaviour, ISpeedMultiplier
    {
        [SerializeField] private float _maxMoveSpeed = 8f;
        [SerializeField] private float _movementLerpTime = 8f;
        [SerializeField] private float _decelerationTime = 15f;

        [Inject] private CharacterController _characterController;
        [Inject] private IReadOnlyPlayerInput _input;
        [Inject] private IReadOnlyPlayerJump _jump;
        [Inject] private PlayerAnimator _animator;
        [Inject] private PlayerRotator _rotator;

        private Vector3 _autoMovementTargetPosition;
        private Vector3 _lastMoveDirection;
        private Vector2 _targetVelocity;

        private float _currentSpeedMultiplier = 1f;
        
        public bool IsMultipliedThisFrame { get; private set; }

        public Vector2 CurrentVelocity { get; private set; }
        
        public float CurrentMaxSpeed => _maxMoveSpeed * _currentSpeedMultiplier;
        
        public bool IsMovementProcess => CurrentVelocity.magnitude > 0;

        public void MoveForward(float speed)
        {
	        var forwardDirection = transform.forward;
	        _targetVelocity = new Vector2(speed, speed);
	        CurrentVelocity = Vector2.Lerp(
		        CurrentVelocity, _targetVelocity, _movementLerpTime * Time.deltaTime);

	        _characterController.Move(
		        (forwardDirection * CurrentVelocity.magnitude + _jump.GetJumpVector()) * Time.deltaTime);
	        _lastMoveDirection = forwardDirection;
        }

        public void ManualMovement()
        {
            var joystickDirection3D = _input.GetJoystickDirection3D();
            var joystickDirection2D = _input.GetJoystickDirection2D();

            if (joystickDirection2D.magnitude != 0)
	            MoveTo(joystickDirection2D, joystickDirection3D);
            else if (IsMovementProcess)
	            Inertia();
            else if (Math.Abs(_jump.GetJumpVector().y) > 0)
	            MoveTo(joystickDirection2D, joystickDirection3D);
        }

        private void MoveTo(Vector2 joystickDirection2D, Vector3 joystickDirection3D)
        {
            _targetVelocity.x = joystickDirection2D.x * (_maxMoveSpeed * _currentSpeedMultiplier);
            _targetVelocity.y = joystickDirection2D.y * (_maxMoveSpeed * _currentSpeedMultiplier);
            CurrentVelocity = Vector2.Lerp(
                CurrentVelocity, _targetVelocity, _movementLerpTime * Time.deltaTime);

            _characterController.Move(
	            (joystickDirection3D * CurrentVelocity.magnitude + _jump.GetJumpVector()) * Time.deltaTime);
            _lastMoveDirection = joystickDirection3D;
        }

        private void Inertia()
        {
            CurrentVelocity = Vector2.MoveTowards(
                CurrentVelocity, Vector2.zero, _decelerationTime * Time.deltaTime);

            _characterController.Move(
	            (_lastMoveDirection * CurrentVelocity.magnitude + _jump.GetJumpVector()) * Time.deltaTime);
        }
        
        public void SetSpeedMultiplier(float multiplier)
        {
	        IsMultipliedThisFrame = true;
            _currentSpeedMultiplier = multiplier;
        }

        private void ResetSpeedMultiplier()
        {
            _currentSpeedMultiplier = 1;
        }
        
        private void LateUpdate()
        {
	        if (IsMultipliedThisFrame)
		        IsMultipliedThisFrame = false;
	        else
				ResetSpeedMultiplier();
        }
    }
}
